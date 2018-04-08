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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Factory = new ContextFactory("MSSQL-2k8","TrolleybusPark",Login.Text, Password.Text);
            //var Factory = new ContextFactory("test3");
            try
            {
                button1.Enabled = false;
                var context = Factory.Get();

                //check 
                context.Database.Connection.Open();
                context.Database.Connection.Close();
                //-----

                var form = new Form1(Factory);

                form.Show();
                form.FormClosed += (a, b) => { this.Close(); };
                this.Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Password.Text = "";
                button1.Enabled = true;
            }
        }
    }
}
