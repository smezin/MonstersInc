using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IdentityServer
{
    public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();

            var connectionString = configuration.GetConnectionString("AppDb");

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("IdentityServer"));

            return new ConfigurationDbContext(dbContextBuilder.Options, 
                new ConfigurationStoreOptions() 
                { 
                    ConfigureDbContext = b => b.UseSqlServer(connectionString) 
                });
        }
    }
}
