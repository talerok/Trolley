using Norm_kurs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norm_kurs.Models
{
    public class Schedule : IEntity
    {
        public int Id { get; set; }
        public Route Route { get; set; }
        public Driver Driver { get; set; }
        public Trolley Trolley { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

}
