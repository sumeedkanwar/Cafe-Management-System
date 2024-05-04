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
            label1.Text = username;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Change_Password Change_Password = new Change_Password(username);

            // Show Staff_Feedback
            Change_Password.Show();

            this.Close(); // Hide Form3
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(username);

            // Show Staff_Feedback
            form3.Show();

            this.Close(); // Hide Form3
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form11 form11 = new Form11(username);
            form11.Show();
            this.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
    }
}
