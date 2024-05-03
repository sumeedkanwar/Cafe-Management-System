using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form6 : Form
    {
        private string username;
        private string defaultUsernamePlaceholder = "Username";
        private string defaultOldPasswordPlaceholder = "Old Password";
        private string defaultNewPasswordPlaceholder = "New Password";
        public Form6(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5(username);

            // Show Form4
            form5.Show();

            this.Close(); // Hide Form3
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == defaultOldPasswordPlaceholder)
            {
                textBox1.Text = "";
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox1.Text = defaultOldPasswordPlaceholder;
            textBox2.Text = defaultNewPasswordPlaceholder;
            textBox3.Text = defaultNewPasswordPlaceholder;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == defaultNewPasswordPlaceholder)
            {
                textBox2.Text = "";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == defaultNewPasswordPlaceholder)
            {
                textBox3.Text = "";
            }
        }
    }
}
