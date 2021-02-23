using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class Door
    {
        [Required]
        [Key]
        public string DoorId { get; set; }
        [Required]
        public string Description { get; set; }
        public int Energy { get; set; }
        public DateTime LastUsed { get; set; }
        public bool IsOpen { get; set; }
        
     
    }
}
