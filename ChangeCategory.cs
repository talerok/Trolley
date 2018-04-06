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
    public partial class ChangeCategory : Form
    {
        private Context Context;
        private bool Edit = false;
        private CategoryNdriver Selected;

        public ChangeCategory(Context context)
        {
            InitializeComponent();
            Context = context;

            DriverBox.DisplayMember = "Name";
            CategoryBox.DisplayMember = "Name";

            DriverBox.DataSource = Context.Drivers.ToList();
            CategoryBox.DataSource = Context.Categories.ToList();
        }

        public ChangeCategory(Context context, int Id) : this(context)
        {
            var res = context.CategoriesNDrivers.FirstOrDefault(x => x.Id == Id);
            if (res == null) Close();

            DriverBox.SelectedItem = res.Driver;
            CategoryBox.SelectedItem = res.Category;
            CategoryTimePicker.Value = res.Start;

            Edit = true;
            Selected = res;

            AddButton.Text = "Сохранить";
        }

        private void Set(CategoryNdriver item)
        {
            var Driver = DriverBox.SelectedItem as Driver;
            var Category = CategoryBox.SelectedItem as Category;
            if (Category != null && Driver != null)
            {
                item.Category = Category;
                item.Driver = Driver;
                item.Start = CategoryTimePicker.Value;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            CategoryNdriver item;
            if (Edit) item = Selected;
            else item = new CategoryNdriver();

            Set(item);

            if (!Edit) Context.CategoriesNDrivers.Add(item);

            Context.SaveChanges();

            Close();
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
