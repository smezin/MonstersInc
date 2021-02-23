using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class WorkDayRepository : IWorkDayRepository
    {
        private MonstersIncDbContext _context;
        public WorkDayRepository (MonstersIncDbContext context)
        {
            _context = context;
        }

        public IQueryable<WorkDay> WorkDays => _context.WorkDays;

        public void CreateWorkDay(WorkDay w)
        {
            _context.WorkDays.Add(w);
            _context.SaveChanges();
        }

        public void DeleteWorkDay(WorkDay w)
        {
            _context.WorkDays.Remove(w);
            _context.SaveChanges();
        }

        public void PatchWorkDay(WorkDay w)
        {
            _context.Attach(w);
            _context.Entry(w).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void SaveWorkDay(WorkDay w)
        {
            _context.SaveChanges();
        }
    }
}
