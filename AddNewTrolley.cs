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

namespace Norm_kurs
{
    public partial class AddNewTrolley : Form
    {
        private Context Context;
        private bool Edit = false;
        private Trolley SelectItem = null;
        public AddNewTrolley(Context context)
        {
            InitializeComponent();
            Context = context;
            ParkBox.DataSource = Context.Parks.ToList();
            ParkBox.DisplayMember = "Adr";
        }

        public AddNewTrolley(Context context, int id) : this(context)
        {
            var Trolley = context.Trollies.FirstOrDefault(x => x.Id == id);
            if (Trolley == null) Close(); 
            ParkBox.SelectedItem = Trolley.park;
            Number.Text = Trolley.Number;


            AddButton.Text = "Сохранить";
            Edit = true;
            SelectItem = Trolley;
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Set(Trolley trolley)
        {
            var park = ParkBox.SelectedItem as Park;
            if (park != null)
            {
                trolley.park = park;
                trolley.Number = Number.Text;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                Trolley trolley;
                var park = Context.Parks.FirstOrDefault(x => x.Adr == ParkBox.Text);
                if (Edit) trolley = SelectItem;
                else trolley = new Trolley();

                Set(trolley);
                if (!Edit) Context.Trollies.Add(trolley);

                Context.SaveChanges();

                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ParkBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
