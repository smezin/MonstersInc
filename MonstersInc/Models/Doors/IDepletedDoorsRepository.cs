using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IDepletedDoorsRepository
    {
        public IQueryable<DepletedDoor> DepletedDoors { get; }
        void SaveDepletedDoor(DepletedDoor d);
        void PatchDepletedDoor(DepletedDoor d);
        void CreateDepletedDoor(DepletedDoor d);
        void DeleteDepletedDoor(DepletedDoor d);
    }
}
