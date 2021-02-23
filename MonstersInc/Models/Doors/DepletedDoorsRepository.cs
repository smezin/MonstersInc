using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class DepletedDoorsRepository : RestfulBaseRepository<DepletedDoor>, IDepletedDoorsRepository
    {
        public DepletedDoorsRepository(MonstersIncDbContext monstersIncDbContext) : base (monstersIncDbContext) { }
        public DepletedDoor GetDepletedDoorIfOpen(string doorId)
        {
            return _monstersIncDbContext.DepletedDoors
                .Where(d => d.DoorId == doorId)
                .Where(d => d.ClosedAt == DateTime.MinValue)
                .FirstOrDefault();
        }
        public bool CloseDepletedDoor (string depletedDoorId)
        {
            DepletedDoor depletedDoor = _monstersIncDbContext.DepletedDoors
                .Where(d => d.DepletedDoorId == depletedDoorId)
                .FirstOrDefault();
            if (depletedDoor != null)
            {
                depletedDoor.ClosedAt = DateTime.Now;
                _monstersIncDbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
