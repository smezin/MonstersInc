using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class WorkDayRepository : RestfulBaseRepository<WorkDay>, IWorkDayRepository
    {
        public WorkDayRepository(MonstersIncDbContext monstersIncDbContext) : base(monstersIncDbContext) { }
        public WorkDay GetActiveWorkDay(string intimidatorId)
        {
            return _monstersIncDbContext.WorkDays
                .Where(w => w.IntimidatorId == intimidatorId)
                .Where(w => w.End == DateTime.MinValue)
                .FirstOrDefault();
        }
        public void HarvestEnergy(string workDayId, string doorId)
        {
            WorkDay workDay = _monstersIncDbContext.WorkDays
                .Where(w => w.WorkDayId == workDayId)
                .FirstOrDefault();
            Door door = _monstersIncDbContext.Doors
                .Where(d => d.DoorId == doorId)
                .FirstOrDefault();
            if (workDay != null && door != null)
            {
                workDay.EnergyCollected += door.Energy;
                _monstersIncDbContext.SaveChanges();               
            }
        }
        public WorkDay StartWorkDay(string intimidatorId)
        {
            return new WorkDay
            {
                WorkDayId = System.Guid.NewGuid().ToString(),
                Begin = DateTime.Now,
                IntimidatorId = intimidatorId,
                EnergyGoal = 100
            };
        }
        public WorkDay EndWorkDay(string workDayId)
        {
            WorkDay workDay = _monstersIncDbContext.WorkDays
                .Where(w => w.WorkDayId == workDayId)
                .FirstOrDefault();
            if (workDay != null)
            {
                workDay.End = DateTime.Now;
                _monstersIncDbContext.SaveChanges();
                return workDay;
            }
            return null;
        }

    }
}
