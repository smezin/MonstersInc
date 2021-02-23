using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class MonstersIncDbContextFactory : IDesignTimeDbContextFactory<MonstersIncDbContext>
    {        
        public MonstersIncDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<MonstersIncDbContext>();
            var connectionString = configuration.GetConnectionString("MonstersIncConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new MonstersIncDbContext(optionsBuilder.Options, configuration);
        }
    }
}
