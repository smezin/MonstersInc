using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection;
using System;
using IdentityServer4.Validation;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Action<DbContextOptionsBuilder> identityDbContextBuilder;

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string appDbConnectionString = Configuration.GetConnectionString("AppDb");
            
            identityDbContextBuilder = x => x.UseSqlServer(appDbConnectionString, options => options.MigrationsAssembly(migrationsAssembly));
            services.AddCors();
            services.AddControllers();
            //CONTEXTS
            services.AddDbContext<AppDbContext>(config =>
                {config.UseSqlServer(appDbConnectionString); });
            services.AddDbContext<PersistedGrantDbContext>(config =>
                {config.UseSqlServer(appDbConnectionString); });
            services.AddDbContext<ConfigurationDbContext>(config =>
                {config.UseSqlServer(appDbConnectionString); });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {   
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var identityServerBuilder = services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                {
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 60*60*24;
                })

                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = identityDbContextBuilder;
                })
                
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = identityDbContextBuilder;

                    // this enables automatic token cleanup. 
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddAspNetIdentity<ApplicationUser>();
         
            identityServerBuilder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseIdentityServer();

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());            
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            var apiResourceOptions = new List<ApiResource>();
            Configuration.GetSection("ApiResources").Bind(apiResourceOptions);

            var clientOptions = new List<Client>();
            Configuration.GetSection("Clients").Bind(clientOptions);

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationDbContext.Database.Migrate();

                var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                appDbContext.Database.Migrate();

                SeedData seedData = new SeedData();
                seedData.Seed(apiResourceOptions, clientOptions, configurationDbContext, appDbContext);
            }
        }
    }
}
