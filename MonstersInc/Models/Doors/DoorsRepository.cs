using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace MonstersAPI.Models
{
    public class DoorsRepository : RestfulBaseRepository<Door>, IDoorsRepository
    {
        public DoorsRepository(MonstersIncDbContext monstersIncDbContext) : base (monstersIncDbContext) { }        
        public bool OpenDoor (string doorId)
        {
            Door door = _monstersIncDbContext.Doors
                .Where(d => d.DoorId == doorId)
                .FirstOrDefault();
            if (door == null || door.IsOpen)
            {
                return false;
            }
            door.IsOpen = true;
            _monstersIncDbContext.SaveChanges();
            return true;
        }
        public bool CloseDoor (string doorId)
        {
            Door door = _monstersIncDbContext.Doors
                .Where(d => d.DoorId == doorId)
                .FirstOrDefault();
            if (door == null || !door.IsOpen)
            {
                return false;
            }
            door.IsOpen = false;
            door.LastUsed = DateTime.Now;
            _monstersIncDbContext.SaveChanges();
            return true;
        }
        public Door GetDoorIfIdle(string doorId)
        {
            return _monstersIncDbContext.Doors
                .Where(d => d.DoorId == doorId)
                .Where(d => d.IsOpen == false)
                .Where(d => d.LastUsed.Date != DateTime.Now.Date)
                .FirstOrDefault();
        }
       
    }
}
