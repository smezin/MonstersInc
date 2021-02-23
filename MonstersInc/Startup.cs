using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using MonstersAPI.SwaggerConfig;
using MonstersAPI.Models;

namespace MonstersAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration.GetSection("IdentityServer").Value;
                    options.Audience = "monstersapi";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminClaim", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
                options.AddPolicy("UserClaim", policy => policy.RequireClaim(ClaimTypes.Role, "user"));
                options.AddPolicy("AdminUserClaim", policy => policy.RequireClaim(ClaimTypes.Role, "user", "admin"));
                options.AddPolicy("AdminUserRole", policy => policy.RequireRole("admin", "user"));
            });
            
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddScoped<IDoorsRepository, DoorsRepository>();
            services.AddScoped<IWorkDayRepository, WorkDayRepository>();
            services.AddScoped<IDepletedDoorsRepository, DepletedDoorsRepository>();
            services.AddDbContext<MonstersIncDbContext>(option => Configuration.GetConnectionString("MonstersIncConnection"));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Monsters API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", MonstersApiSecurityScheme.GetSecurityScheme());               
                options.AddSecurityRequirement(MonstersApiSecurityRequirement.GetOpenApiSecurityRequirement());
                var xmlPath = AppDomain.CurrentDomain.BaseDirectory + @"XMLDocumentation.xml";
                options.IncludeXmlComments(xmlPath);
            });
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
        
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Monsters API V1");
            });            
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            SeedDoors.EnsurePopulated(app);
        }
    }
    /*
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}}]
                            = new[] {"api1"}
                    }
                };
            }
        }
    }
    */
}
