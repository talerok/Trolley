using Norm_kurs.DA;
using System;
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
using Norm_kurs.Models.Interfaces;

namespace Norm_kurs
{
    public partial class AddPayment : Form
    {
        private bool Edit = false;
        private Payment Selected = null;
        private Context Context;
        public AddPayment(Context context)
        {
            InitializeComponent();
            Context = context;
        }

        public AddPayment(Context context, int id) : this(context)
        {
            var res = Context.Paymenets.FirstOrDefault(x => x.Id == id);
            if (res == null) Close();

            PayementdateTimePicker.Value = res.Start;
            PayementTextBox.Text = res.PerHour.ToString();

            Edit = true;
            Selected = res;
           
        }

        private void AddButton_Click(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {

        }
    }
}
