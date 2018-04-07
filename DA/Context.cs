using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm_kurs.Models;
using System.Data.Entity;

namespace Norm_kurs.DA
{
    public class Context : DbContext
    {
        public virtual DbSet<Driver> Drivers { get; set;}
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryNdriver> CategoriesNDrivers { get; set; }
        public virtual DbSet<Payment> Paymenets { get; set; }
        public virtual DbSet<Stop> Stops { get; set; }
        public virtual DbSet<Trolley> Trollies { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RouteStop> RouteStops { get; set; }
        public Context(string connString) : base(connString)
        {
            
        }
    }
}
