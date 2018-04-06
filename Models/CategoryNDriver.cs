using Norm_kurs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norm_kurs.Models
{
    public class CategoryNdriver : IEntity
    {
        public int Id { get; set; }
        public Driver Driver { get; set; }
        public Category Category { get; set; }
        public DateTime Start { get; set; }
    }
}
