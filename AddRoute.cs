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
    public partial class AddRoute : Form
    {
        private Context Context;
        private bool Edit = false;
        private Route Selected = null;


        public AddRoute(Context context)
        {
            InitializeComponent();
            Context = context;

            var Parks = Context.Parks.ToList();

            Park1.DisplayMember = "Adr";
            Park2.DisplayMember = "Adr";

            Park1.DataSource = Parks;
            Park2.DataSource = Parks;
        }

        public AddRoute(Context context, int id) : this(context)
        {
            var Route = Context.Routes.FirstOrDefault(x => x.Id == id);
            if (Route == null) Close();

            Park1.SelectedItem = Route.Start;
            Park2.SelectedItem = Route.End;
            textBox1.Text = Route.Name;

            AddButton.Text = "Сохранить";

            Edit = true;
            Selected = Route;
        }

        private void Set(Route route)
        {
            var ParkOne = Park1.SelectedItem as Park;
            var ParkTwo = Park2.SelectedItem as Park;
            if (ParkOne != null && ParkTwo != null)
            {
                route.Start = ParkOne;
                route.End = ParkTwo;
                route.Name = textBox1.Text;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                Route route;
                if (Selected != null) route = Selected;
                else route = new Route();

                Set(route);

                if (!Edit) Context.Routes.Add(route);
                Context.SaveChanges();

                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
