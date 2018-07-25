using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarFetch.Model
{
    public class Routes
    {
        public int RouteID { get; set; }
        public string PointFrom { get; set; }
        public string PointTo { get; set; }
        public int Time { get; set; }
        public int Cost { get; set; }

    }
}
