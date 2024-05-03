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
    public partial class Form6 : Form
    {
        private string username;
        private string defaultUsernamePlaceholder = "Username";
        private string defaultOldPasswordPlaceholder = "Old Password";
        private string defaultNewPasswordPlaceholder = "New Password";
        private SqlConnection connection;
        public Form6(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox1.Text = defaultOldPasswordPlaceholder;
            textBox2.Text = defaultNewPasswordPlaceholder;
            textBox3.Text = defaultNewPasswordPlaceholder;

            textBox1.Click += TextBox1_Click;
            textBox2.Click += TextBox2_Click;
            textBox3.Click += TextBox3_Click;

            textBox1.Leave += textBox1_Leave;
            textBox2.Leave += textBox2_Leave;
            textBox3.Leave += textBox3_Leave;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5(username);

            // Show Form4
            form5.Show();

            this.Close(); // Hide Form3
        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == defaultOldPasswordPlaceholder)
            {
                textBox1.Text = "";
            }
        }

        private void TextBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == defaultNewPasswordPlaceholder)
            {
                textBox2.Text = "";
            }
        }

        private void TextBox3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == defaultNewPasswordPlaceholder)
            {
                textBox3.Text = "";
            }
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = defaultOldPasswordPlaceholder;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = defaultNewPasswordPlaceholder;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = defaultNewPasswordPlaceholder;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == defaultOldPasswordPlaceholder || textBox2.Text == defaultNewPasswordPlaceholder || textBox3.Text == defaultNewPasswordPlaceholder)
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

    }
}
