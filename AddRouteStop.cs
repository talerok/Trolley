using Norm_kurs.BL;
using Norm_kurs.DA;
using Norm_kurs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Norm_kurs
{
    public partial class AddRouteStop : Form
    {
        private bool Edit = false;
        private RouteStop Selected = null;

        Context Context;

        public AddRouteStop(Context context)
        {
            InitializeComponent();
            Context = context;

            RouteBox.DisplayMember = "Name";
            StopBox.DisplayMember = "Name";

            RouteBox.DataSource = Context.Routes.ToList();
            StopBox.DataSource = Context.Stops.ToList();          
        }

        public AddRouteStop(Context context, int id) : this(context)
        {
            var res = Context.RouteStops.FirstOrDefault(x => x.Id == id);
            if (res == null) Close();
            RouteBox.SelectedItem = res.Route;
            StopBox.SelectedItem = res.Stop;
            PrevStopBox.SelectedValue = res.Prev;

            Selected = res;
            Edit = true;

            AddButton.Text = "Изменить";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Test(RouteStop prev, Route route)
        {
            if (prev != null) return;
            var res = Context.RouteStops.FirstOrDefault(x => x.Prev == null && x.Route.Id == route.Id);
            if (res != null) throw (new Exception("Начальнаяя остановка на этом маршруте уже существует"));
        }

        private void Set(RouteStop routeStop)
        {
            var route = RouteBox.SelectedItem as Route;
            var stop = StopBox.SelectedItem as Stop;
            var prev = PrevStopBox.SelectedValue as RouteStop;

            Test(prev, route);


            if (route != null && stop != null)
            {
                routeStop.Route = route;
                routeStop.Stop = stop;
                routeStop.Prev = prev;
            }

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                RouteStop res;
                if (Edit) res = Selected;
                else res = new RouteStop();

                Set(res);

                if (!Edit) Context.RouteStops.Add(res);

                Context.SaveChanges();
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        

        private void RouteBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var route = RouteBox.SelectedItem as Route;
            if (route == null) return;
            PrevStopBox.DataSource = null;
            PrevStopBox.DisplayMember = "DisplayMember";
            PrevStopBox.ValueMember = "ValueMember";
            PrevStopBox.DataSource = Context.RouteStops
                .Include(x => x.Stop).Include(x => x.Prev).Where(x => x.Route.Id == route.Id).ToList()
                .Select(x =>
                    new
                    {
                        DisplayMember = NameHelper.GetFullName(x),
                        ValueMember = x
                    }
                )
                .OrderBy(x => NameHelper.GetNest(x.DisplayMember)).ToList();
            PrevStopBox.SelectedItem = null;
        }
    }
}
