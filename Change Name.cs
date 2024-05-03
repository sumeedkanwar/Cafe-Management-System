using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Change_Name : Form
    {
        private string username;
        private string defaultUsernamePlaceholder;
        private SqlConnection connection;
        public Change_Name(string username)
        {
            InitializeComponent();
            this.username = username;
            this.defaultUsernamePlaceholder = getFullName();
        }
        
        private string getFullName()
        {
            string fullName = "";
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT fullname FROM Users WHERE username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    fullName = (string)command.ExecuteScalar();
                }
            }
            return fullName;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5(username);

            form5.Show();

            this.Close();
        }

        private void Change_Name_Load(object sender, EventArgs e)
        {
            textBox1.Text = defaultUsernamePlaceholder;
            textBox1.Leave += TextBox1_Leave;
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = defaultUsernamePlaceholder;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == defaultUsernamePlaceholder)
            {
                MessageBox.Show("Please enter a new name.");
                return;
            }

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("UPDATE Users SET fullname = @FullName WHERE username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@FullName", textBox1.Text);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Name changed successfully.");
                }
            }
        }
    }
}
