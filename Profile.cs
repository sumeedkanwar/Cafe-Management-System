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
    public partial class Profile : Form
    {
        private string username;
        public Profile(string username)
        {
            InitializeComponent();
            this.username = username;
            //label1.Text = username;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Change_Password Change_Password = new Change_Password(username);

            // Show Staff_Feedback
            Change_Password.Show();

            this.Close(); // Hide Staff_Dashboard
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Staff_Dashboard Staff_Dashboard = new Staff_Dashboard(username);

            // Show Staff_Feedback
            Staff_Dashboard.Show();

            this.Close(); // Hide Staff_Dashboard
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Change_Name Change_Name = new Change_Name(username);
            Change_Name.Show();
            this.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
    }
}
