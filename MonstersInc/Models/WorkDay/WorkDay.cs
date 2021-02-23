using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class WorkDay
    {
        [Key]
        public string WorkDayId { get; set; }
        [Required]
        public string IntimidatorId { get; set; }
        [Required]
        public DateTime Begin { get; set; }
        public DateTime End { get; set; } = DateTime.MinValue;
        public List<Door> Doors { get; set; }
        public List<DepletedDoor> DepletedDoors { get; set; }
        public int EnergyGoal { get; set; }
        public int EnergyCollected { get; set; }
    }
}
