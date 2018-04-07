using System;
using Norm_kurs.DA;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Windows.Forms;
using Norm_kurs.BL;
using Norm_kurs.Models;

namespace Norm_kurs
{
    public partial class PaymentInfo : Form
    {
        private Context Context;
        public PaymentInfo(Context context)
        {
            Context = context;
            InitializeComponent();
            foreach (var a in Context.Drivers) DriverBox.Items.Add(a.Name);
        }

        private void PaymentInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double Sum = 0;

            var Categories = Context.CategoriesNDrivers.Include(x => x.Category).Include(x => x.Driver).Where(x => x.Driver.Name == DriverBox.Text).ToList();
            var Payements = Context.Paymenets.ToList();
            var Res = Context.Schedules.Where(
                x =>
                x.Driver.Name == DriverBox.Text &&
                x.Start >= StartTimePicker.Value &&
                x.End <= EndTimePicker.Value);
            foreach (var a in Res)
            {
                var Category = Categories.LastOrDefault(x => a.Start >= x.Start);
                var Payement = Payements.LastOrDefault(x => a.Start >= x.Start);
                if (Category == null) continue;
                Sum += (a.End - a.Start).TotalHours * Category.Category.Coef * Payement.PerHour;
            }
            SummText.Text = Sum.ToString();
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
