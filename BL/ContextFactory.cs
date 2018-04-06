using Norm_kurs.DA;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Norm_kurs.BL
{
    public class ContextFactory
    {
        private string ConnString;
        public ContextFactory(string server, string bd, string login, string password)
        {
            ConnString = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + login + ";Password=" + password;
        }

        public ContextFactory(string name)
        {
            ConnString = name;
        }

        public Context Get()
        {
            return new Context(ConnString);
        }
    }
}
