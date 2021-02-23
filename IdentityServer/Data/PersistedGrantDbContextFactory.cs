using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IdentityServer
{
    public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();

            var connectionString = configuration.GetConnectionString("AppDb");

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("IdentityServer"));

            return new PersistedGrantDbContext(dbContextBuilder.Options, new OperationalStoreOptions() 
            { 
                ConfigureDbContext = b => b.UseSqlServer(connectionString) 
            });
        }
    }
}
