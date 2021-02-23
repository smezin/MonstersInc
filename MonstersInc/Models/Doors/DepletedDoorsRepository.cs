using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class DepletedDoorsRepository : IDepletedDoorsRepository
    {
        private MonstersIncDbContext _context;
        public DepletedDoorsRepository(MonstersIncDbContext context)
        {
            _context = context;
        }
        public IQueryable<DepletedDoor> DepletedDoors => _context.DepletedDoors;

        public void CreateDepletedDoor(DepletedDoor d)
        {
            _context.Add(d);
            _context.SaveChanges();
        }

        public void DeleteDepletedDoor(DepletedDoor d)
        {
            _context.Remove(d);
            _context.SaveChanges();
        }

        public void PatchDepletedDoor(DepletedDoor d)
        {
            _context.Attach(d);
            _context.Entry(d).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void SaveDepletedDoor(DepletedDoor d)
        {
            _context.SaveChanges();
        }
    }
}
