using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Pending_Order : Form
    {
        private string username;
        public Pending_Order(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Customer_Dashboard form1 = new Customer_Dashboard(username);
            form1.Show();
            this.Hide();
        }
    }
}
