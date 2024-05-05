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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Change_Password : Form
    {
        private string username;
        private string defaultUsernamePlaceholder = "Username";
        private SqlConnection connection;
        public Change_Password(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void Change_Password_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Profile form5 = new Profile(username);

            // Show Staff_Feedback
            form5.Show();

            this.Close(); // Hide Staff_Dashboard
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == ""|| textBox3.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                {
                    connection.Open(); // Open the connection

                    string updateQuery = "UPDATE Users SET password = @NewPassword WHERE username = @Username AND password = @OldPassword";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@NewPassword", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@OldPassword", textBox1.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Incorrect old password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating password: " + ex.Message);
                //print error
                Console.WriteLine(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
