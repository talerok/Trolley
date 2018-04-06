using Norm_kurs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norm_kurs.Models
{
    public class Trolley : IEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public Park park { get; set; }
    }
}
