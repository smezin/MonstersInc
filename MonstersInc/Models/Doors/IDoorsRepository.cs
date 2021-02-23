using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IDoorsRepository 
    {
        public IQueryable<Door> Doors { get; }
        void SaveDoor(Door d);
        void PatchDoor(Door d);
        void CreateDoor(Door d);
        void DeleteDoor(Door d);
        
    }
}
