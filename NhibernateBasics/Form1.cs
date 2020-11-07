using NhibernateBasics.Model;

using System;
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
            this.LoadEmployeeData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.LoadEmployeeData();
        }

        private void LoadEmployeeData()
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

#if DEBUG
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
#endif
    }
}
