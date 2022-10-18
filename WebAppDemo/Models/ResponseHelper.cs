using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppDemo.Models
{
    public class ResponseHelper
    {
        public Int16 Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }  
    }
}
