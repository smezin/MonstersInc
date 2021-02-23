using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IDepletedDoorsRepository : IRestfulBaseRepository<DepletedDoor> 
    {
        public DepletedDoor GetDepletedDoorIfOpen(string doorId);
        public bool CloseDepletedDoor(string depletedDoorId);
    }
   
}
