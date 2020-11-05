using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ISession session = SessionFactory.OpenSession;
            
            using (session)
            {
                IQuery query = session.CreateQuery("FROM Employee");
                IList<Model.Employee> empInfo = query.List<Model.Employee>();
                dataGridView1.DataSource = empInfo;
            }
        }
    }
}
