using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonstersAPI.Models
{
    public class Response<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }    
        public List<T> Content { get; set; }    
    }
}
