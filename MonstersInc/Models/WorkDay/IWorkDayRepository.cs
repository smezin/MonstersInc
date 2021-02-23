using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IWorkDayRepository
    {
        IQueryable<WorkDay> WorkDays { get; }
        void SaveWorkDay(WorkDay w);
        void CreateWorkDay(WorkDay w);
        void DeleteWorkDay(WorkDay w);
        void PatchWorkDay(WorkDay w);
        //WorkDay BeginWorkDay ()
    }
}
