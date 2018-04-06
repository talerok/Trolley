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
    public partial class Form1 : Form
    {
        private Context Context;
        private BindingSource BindingSource = new BindingSource();

        private string[] Entities =
        {
            "Водители",
            "Категории",
            "Категории Водителей",
            "Парки",
            "Зарплаты",
            "Расписание",
            "Тролейбусы",
            "Маршруты"
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

        private void CheckSelect(string select)
        {

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
            else if (select == Entities[3]) SetGrid(Context.Parks);
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
            else if (select == Entities[6])
            {
                var res = Context.Trollies.Include(x => x.park).Select(x =>
                    new
                    {
                        ID = x.Id,
                        Number = x.Number,
                        Park = x.park.Adr
                    }).ToList();
                dataGridView1.DataSource = res;
                Control(false);
            }
            else if (select == Entities[7])
            {
                var res = Context.Routes.Include(x => x.Start).Include(x => x.End).Select(x =>
                    new
                    {
                        ID = x.Id,
                        Name = x.Name,
                        From = x.Start.Adr,
                        To = x.End.Adr,
                    }).ToList();
                dataGridView1.DataSource = res;
                Control(false);
            }

        }



        public Form1(Context context)
        {
            Context = context;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Entities);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            Context.SaveChanges();
            var ee = Context.Drivers.ToList();
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

      
        private void AddSchedule_Click(object sender, EventArgs e)
        {
            var form = new AddNewSchedule(Context);
            form.Show();
        }

        private void AddTrolley_Click(object sender, EventArgs e)
        {
            var form = new AddNewTrolley(Context);
            form.Show();
        }

        private void AddRoute_Click(object sender, EventArgs e)
        {
            var form = new AddRoute(Context);
            form.Show();
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            var form = new ChangeCategory(Context);
            form.Show();
        }

        private void PaymentChange_Click(object sender, EventArgs e)
        {
         
        }

        private void PaymentInfo_Click(object sender, EventArgs e)
        {
            var form = new PaymentInfo(Context);
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
            Context.SaveChanges();
        }

        private void Delete(IEnumerable<int> ids)
        {
            string select = comboBox1.Text;
            if (select == Entities[2])  Delete(Context.CategoriesNDrivers, ids);
            else if (select == Entities[5]) Delete(Context.Schedules, ids);
            else if (select == Entities[6]) Delete(Context.Trollies, ids);
            else if (select == Entities[7]) Delete(Context.Routes, ids);
            
        }

        private void AddOrUpdate(int id = -1)
        {
            string select = comboBox1.Text;
            Form form = null;
            if (select == Entities[2])
            {
                if (id == -1) form = new ChangeCategory(Context);
                else form = new ChangeCategory(Context, id);
            }
            else if (select == Entities[5])
            {
                if (id == -1) form = new AddNewSchedule(Context);
                else form = new AddNewSchedule(Context, id);
            }
            else if (select == Entities[6])
            {
                if (id == -1) form = new AddNewTrolley(Context);
                else form = new AddNewTrolley(Context, id);
            }
            else if (select == Entities[7])
            {
                if (id == -1) form = new AddRoute(Context);
                else form = new AddRoute(Context, id);
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

        private void CreateButton_Click(object sender, EventArgs e)
        {
            AddOrUpdate();
        }
    }
}
