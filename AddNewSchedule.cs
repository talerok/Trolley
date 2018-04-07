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


        private bool Edit = false;
        private Schedule SelectItem = null;
        public AddNewSchedule(Context context)
        {
            InitializeComponent();
            Context = context;

            DriverBox.DisplayMember = "Name";
            RouteBox.DisplayMember = "Name";
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

            var timeStart = TimeSpan.Parse(TimeStart.Text);
            var timeEnd = TimeSpan.Parse(TimeEnd.Text);

            var Start = dateTimePicker.Value.Date;
            var End = dateTimePicker.Value.Date;

            

            Start.AddMinutes(timeStart.TotalMinutes);
            End.AddMinutes(timeEnd.TotalMinutes);

            if (Start >= End)
                throw new Exception("Начало смены раньше, чем конец");

            if (Driver != null && Route != null && Trolley != null)
            {
                schedule.Driver = Driver;
                schedule.Route = Route;
                schedule.Trolley = Trolley;
                schedule.Start = Start;
                schedule.End = End;
            }         
        }

        private bool Intersection(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 < end2 || end1 < start2) return false;
            else return true;
        }

        private void Test(Schedule schedule)
        {
            var test = Context.Schedules
                   .FirstOrDefault(x =>
                   (schedule.Start <= x.End && schedule.End >= x.Start)
                   && x.Driver.Id == schedule.Driver.Id || x.Trolley.Id == schedule.Trolley.Id);

            if (test != null)
                throw (new Exception(String.Format("Смена пересекается с другой сменой\n Id: {0}, Водитель: {1}, Тролейбус: {2}, Начало смены: {3}, Конец смены {4}", test.Id, test.Driver.Name, test.Trolley.Number, test.Start, test.End)));
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

                Test(schedule);

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

        private void TrolleyBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RouteBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
