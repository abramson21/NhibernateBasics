using NHibernate;
using NhibernateBasics.Model;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
#if DEBUG
using System.Runtime.InteropServices;
#endif
using System.Windows.Forms;

namespace NhibernateBasics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
#if DEBUG
            AllocConsole();
#endif
            this.loadEmployeeData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.loadEmployeeData();
        }

        private void loadEmployeeData()
        {
            try
            {
                using var session = SessionFactory.OpenSession;
                this.dataGridView1.DataSource = session.Query<Employee>().ToList();
            }
            catch (Exception ex)
            {
                var message = ex.Message + Environment.NewLine + ex.StackTrace;
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
        }

        private void Insert_Click(object sender, EventArgs e)
        {
            Model.Employee empData = new Model.Employee();
            SetEmployeInfo(empData);

            ISession session = SessionFactory.OpenSession;

            using (session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(empData);
                        transaction.Commit();
                        loadEmployeeData();

                        FirstName.Text = "";
                        LastName.Text = "";
                        Email.Text = "";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        throw ex;
                    }
                }
            }
        }

        private void SetEmployeInfo(Model.Employee emp)
        {
            emp.FirstName = FirstName.Text;
            emp.LastName = LastName.Text;
            emp.Email = Email.Text;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            //to update data we will load current data to our textbox and then update
            ISession session = SessionFactory.OpenSession;

            using (session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        IQuery query = session.CreateQuery("FROM Employee WHERE Id = '" + IdTxt.Text + "'");
                        Model.Employee empData = query.List<Model.Employee>()[0];
                        SetEmployeInfo(empData);
                        session.Update(empData);
                        transaction.Commit();

                        loadEmployeeData();

                        IdTxt.Text = "";
                        FirstName.Text = "";
                        LastName.Text = "";
                        Email.Text = "";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount <= 1 || e.RowIndex < 0)
                return;
            string id = dataGridView1[0, e.RowIndex].Value.ToString();

            if (id == "")
                return;

            IList<Model.Employee> empInfo = getDataFromEmployee(id);

            IdTxt.Text = empInfo[0].Id.ToString();
            FirstName.Text = empInfo[0].FirstName.ToString();
            LastName.Text = empInfo[0].LastName.ToString();
            Email.Text = empInfo[0].Email.ToString();
        }

        private IList<Model.Employee> getDataFromEmployee(string id)
        {
            ISession session = SessionFactory.OpenSession;

            using (session)
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        IQuery query = session.CreateQuery("FROM Employee WHERE Id = '" + id + "'");
                        return query.List<Model.Employee>();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        throw ex;
                    }
                }
            }
        }

#if DEBUG
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
#endif
    }
}
