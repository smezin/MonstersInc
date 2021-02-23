using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IWorkDayRepository : IRestfulBaseRepository<WorkDay> 
    {
        public WorkDay GetActiveWorkDay(string intimidatorId);
        public void HarvestEnergy(string workDayId, string doorId);
        public WorkDay StartWorkDay(string intimidatorId);
        public WorkDay EndWorkDay(string workDayId);
    }
    
}
