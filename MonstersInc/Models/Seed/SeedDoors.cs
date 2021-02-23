using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public static class SeedDoors
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            MonstersIncDbContext context = app.ApplicationServices
               .CreateScope().ServiceProvider.GetRequiredService<MonstersIncDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            Door door_1 = new Door { DoorId = System.Guid.NewGuid().ToString(), Energy = 10, Description = "big red", LastUsed = DateTime.Now };
            Door door_2 = new Door { DoorId = System.Guid.NewGuid().ToString(), Energy = 20, Description = "small blue", LastUsed = DateTime.Now };
            Door door_3 = new Door { DoorId = System.Guid.NewGuid().ToString(), Energy = 30, Description = "green gate", LastUsed = DateTime.Now.AddDays(-2) };
            Door door_4 = new Door { DoorId = System.Guid.NewGuid().ToString(), Energy = 40, Description = "painted black", LastUsed = DateTime.Now.AddHours(-10) };
            Door door_5 = new Door { DoorId = System.Guid.NewGuid().ToString(), Energy = 50, Description = "jim's door", LastUsed = DateTime.Now.AddDays(-10) };
            Door door_6 = new Door { DoorId = System.Guid.NewGuid().ToString(), Energy = 60, Description = "mistery door", LastUsed = DateTime.Now };
            if (!context.Doors.Any())
            {
                context.Doors.AddRange(
                    door_1,
                    door_2,
                    door_3,
                    door_4,
                    door_5,
                    door_6
                    );
                context.SaveChanges();
            }
        }
           
    }
}
