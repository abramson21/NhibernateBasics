using NhibernateBasics.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace NhibernateBasics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadEmployeeData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadEmployeeData();
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
    }
}
