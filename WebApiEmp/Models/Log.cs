using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiEmp.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string DateTime { get; set; }
        public string Method { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
    }
}
