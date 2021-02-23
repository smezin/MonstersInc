using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public interface IDoorsRepository : IRestfulBaseRepository<Door> 
    {
        public bool OpenDoor(string doorId);
        public bool CloseDoor(string doorId);
        public Door GetDoorIfIdle(string doorId);
    }
    
}
