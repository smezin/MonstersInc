using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class DepletedDoor
    {
        [Key]
        public string DepletedDoorId { get; set; }
        [Required]
        public string DoorId { get; set; }
        [Required]
        public string WorkDayId { get; set; }
        [Required]
        public DateTime OpenedAt { get; set; }
        public DateTime ClosedAt { get; set; } = DateTime.MinValue;
    }
}
