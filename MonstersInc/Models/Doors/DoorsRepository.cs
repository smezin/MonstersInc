using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class DoorsRepository : IDoorsRepository
    {
        private MonstersIncDbContext _context;
        public DoorsRepository (MonstersIncDbContext context)
        {
            _context = context;
        }
        public IQueryable<Door> Doors => _context.Doors;

        public void CreateDoor(Door d)
        {
            _context.Doors.Add(d);
            _context.SaveChanges();
        }

        public void DeleteDoor(Door d)
        {
            _context.Doors.Remove(d);
            _context.SaveChanges();
        }

        public void PatchDoor(Door d)
        {
            _context.Attach(d);
            _context.Entry(d).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void SaveDoor(Door d)
        {
            _context.SaveChanges();
        }
    }
}
