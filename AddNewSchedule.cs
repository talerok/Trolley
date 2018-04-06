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
    public partial class AddNewSchedule : Form
    {
        private Context Context;

        private List<int> DriverID = new List<int>();
        private List<int> RouteID = new List<int>();
        private List<int> TrolleyID = new List<int>();

        private bool Edit = false;
        private Schedule SelectItem = null;
        public AddNewSchedule(Context context)
        {
            InitializeComponent();
            Context = context;

            DriverBox.DisplayMember = "Name";
            RouteBox.DisplayMember = "Name";
            TrolleyBox.DisplayMember = "Number";

            DriverBox.DataSource = Context.Drivers.ToList();
            RouteBox.DataSource = Context.Routes.ToList();
            TrolleyBox.DataSource = Context.Trollies.ToList();

            
        }

        public AddNewSchedule(Context context, int id) : this(context)
        {
            var res = context.Schedules.FirstOrDefault(x => x.Id == id);
            if (res == null) Close();
            DriverBox.SelectedItem = res.Driver;
            RouteBox.SelectedItem = res.Route;
            TrolleyBox.SelectedItem = res.Trolley;
            TimeStart.Text = res.Start.ToShortTimeString();
            TimeEnd.Text = res.End.ToShortTimeString();
            dateTimePicker.Value = res.Start;
            Edit = true;
            SelectItem = res;
            AddButton.Text = "Сохранить";
        }

        private void Set(Schedule schedule)
        {
            var Driver = DriverBox.SelectedItem as Driver;
            var Route = RouteBox.SelectedItem as Route;
            var Trolley = TrolleyBox.SelectedItem as Trolley;

            var Start = dateTimePicker.Value + TimeSpan.Parse(TimeStart.Text);
            var End = dateTimePicker.Value + TimeSpan.Parse(TimeEnd.Text);
            if (Driver != null && Route != null && Trolley != null)
            {
                schedule.Driver = Driver;
                schedule.Route = Route;
                schedule.Trolley = Trolley;
                schedule.Start = Start;
                schedule.End = End;
            }         
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {         
                Schedule schedule;
                if (Edit)
                    schedule = SelectItem;
                else
                    schedule = new Schedule();

                Set(schedule);
                if (!Edit) Context.Schedules.Add(schedule);
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
