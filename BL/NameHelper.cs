using Norm_kurs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Norm_kurs.BL
{
    class NameHelper
    {
        public static string Separator = "->";
        private static Regex SepRegex = new Regex(Separator);

        public static int GetNest(string name)
        {
            return SepRegex.Matches(name).Count;
        }

        public static string GetFullName(RouteStop routeStop)
        {
            var res = routeStop.Stop.Name;
            var cur = routeStop.Prev;
            while (cur != null)
            {
                res = cur.Stop.Name + Separator + res;
                cur = cur.Prev;
            }
            return res;
        }
    }
}
