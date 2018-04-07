using Norm_kurs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norm_kurs.Models
{
    public class RouteStop : IEntity
    {
        public int Id { get; set; }
        public Stop Stop { get; set;}
        public Route Route { get; set; }
        public RouteStop Prev { get; set; }
    }
}
