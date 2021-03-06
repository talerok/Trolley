﻿using Norm_kurs.DA;
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
    public class RouteStopView 
    {
        public string Route { get; set; }
        public int RouteId { get; set; }
        public string Stop { get; set; }
        public int Id { get; set; }  
        public string FullRoute { get; set; }
    }

    public class RouteStopViewCompare : IComparer<RouteStopView>
    {
        public int Compare(RouteStopView one, RouteStopView two)
        {
            if (one.RouteId > two.RouteId) return 1;
            if (one.RouteId < two.RouteId) return -1;
            else
            {
                int Nest1 = NameHelper.GetNest(one.FullRoute);
                int Nest2 = NameHelper.GetNest(two.FullRoute);
                if (Nest1 > Nest2) return 1;
                else if (Nest1 < Nest2) return -1;
            }
            return 0;
        }
    }

    public partial class Form1 : Form
    {
        private ContextFactory ContextFactory;
        private BindingSource BindingSource = new BindingSource();

        private string[] Entities =
        {
            "Водители",
            "Категории",
            "Категории Водителей",
            "Остановки",
            "Зарплаты",
            "Расписание",
            "Тролейбусы",
            "Маршруты",
            "Маршруты и остановки"
        };

        private void Control(bool Modifiable)
        {
            SaveButton.Enabled = Modifiable;
            DeleteButton.Enabled = !Modifiable;
            CreateButton.Enabled = !Modifiable;
            EditButton.Enabled = !Modifiable;
            UpdateButton.Enabled = true;
        }

        private void SetGrid<T>(DbSet<T> Entity) where T : class
        {
            Entity.Load();
            BindingSource.DataSource = Entity.Local.ToBindingList();
            dataGridView1.DataSource = BindingSource;
            Control(true);
        }

        private Context GlobalContext;

        private void CheckSelect(string select)
        {
            var Context = ContextFactory.Get();
            if (select == Entities[0]) SetGrid(Context.Drivers);
            else if (select == Entities[1]) SetGrid(Context.Categories);
            else if (select == Entities[2])
            {
                var res = Context.CategoriesNDrivers
                    .Include(x => x.Driver)
                    .Include(x => x.Category)
                    .Select(x =>
                    new
                    {
                        ID = x.Id,
                        Driver = x.Driver.Name,
                        Category = x.Category.Name,
                        Cf = x.Category.Coef,
                        x.Start
                    }).ToList();
                dataGridView1.DataSource = res;
                Control(false);
            }
            else if (select == Entities[3]) SetGrid(Context.Stops);
            else if (select == Entities[4]) SetGrid(Context.Paymenets);
            else if (select == Entities[5])
            {
                var res = Context.Schedules
                    .Include(x => x.Driver)
                    .Include(x => x.Trolley)
                    .Include(x => x.Route)
                    .Select(x =>
                    new
                    {
                        ID = x.Id,
                        Driver = x.Driver.Name,
                        Route = x.Route.Name,
                        x.Start,
                        x.End,
                        Trolley = x.Trolley.Number
                    }).ToList();
                dataGridView1.DataSource = res;
                Control(false);
            }
            else if (select == Entities[6]) SetGrid(Context.Trollies);
            else if (select == Entities[7]) SetGrid(Context.Routes);
            else if (select == Entities[8])
            {
                var data = Context.RouteStops
                    .Include(x => x.Route)
                    .Include(x => x.Prev)
                    .Include(x => x.Stop).ToList();

                var compare = new RouteStopViewCompare();

                dataGridView1.DataSource = data.Select(
                    x => new RouteStopView
                    {
                        Id = x.Id,
                        RouteId = x.Route.Id,
                        Route = x.Route.Name,
                        Stop = x.Stop.Name,
                        FullRoute = NameHelper.GetFullName(x)
                    }
                ).OrderBy(s => s, compare).ToList();
                Control(false);
            }
            else return;
            GlobalContext = Context;
        }

        public Form1(ContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Entities);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            GlobalContext.SaveChanges();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            CheckSelect(comboBox1.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void PaymentChange_Click(object sender, EventArgs e)
        {
         
        }

        private void PaymentInfo_Click(object sender, EventArgs e)
        {
            var form = new PaymentInfo(ContextFactory.Get());
            form.Show();
        }

        private void Delete<T>(DbSet<T> Entity, IEnumerable<int> ids) where T : class, IEntity
        {
            List<T> Elems = new List<T>();
            foreach(var id in ids)
            {
                var Elm = Entity.FirstOrDefault(x => x.Id == id);
                if (Elm != null) Elems.Add(Elm);
            }
            Entity.RemoveRange(Elems);
            
        }

        private void Delete(IEnumerable<int> ids)
        {
            try
            {
                string select = comboBox1.Text;
                var Context = ContextFactory.Get();
                if (select == Entities[2]) Delete(Context.CategoriesNDrivers, ids);
                else if (select == Entities[5]) Delete(Context.Schedules, ids);
                else if (select == Entities[8]) Delete(Context.RouteStops, ids);
                else return;
                Context.SaveChanges();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddOrUpdate(int id = -1)
        {
            string select = comboBox1.Text;
            Form form = null;
            if (select == Entities[2])
            {
                if (id == -1) form = new ChangeCategory(ContextFactory.Get());
                else form = new ChangeCategory(ContextFactory.Get(), id);
            }
            else if (select == Entities[5])
            {
                if (id == -1) form = new AddNewSchedule(ContextFactory.Get());
                else form = new AddNewSchedule(ContextFactory.Get(), id);
            }
            else if (select == Entities[8])
            {
                if (id == -1) form = new AddRouteStop(ContextFactory.Get());
                else form = new AddRouteStop(ContextFactory.Get(), id);
            }
            if (form != null) form.Show();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count != 0)
            {
                List<int> ids = new List<int>();
                foreach(DataGridViewRow row in dataGridView1.SelectedRows)
                    ids.Add(Convert.ToInt32(row.Cells[0].Value.ToString()));
                Delete(ids);
                RefreshData();
            }
        }

        private int GetId()
        {
            return Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0) AddOrUpdate(GetId());

        }

        private void PaymentButton_Click(object sender, EventArgs e)
        {
            var form = new PaymentInfo(ContextFactory.Get());
            form.Show();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            AddOrUpdate();
        }
    }
}
