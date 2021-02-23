using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class MonstersIncDbContext : DbContext
    {
        private readonly IConfiguration Config;
        public MonstersIncDbContext(DbContextOptions<MonstersIncDbContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }
        public DbSet<Door> Doors { get; set; }
        public DbSet<DepletedDoor> DepletedDoors { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString("MonstersIncConnection"));
        }

    }
}
