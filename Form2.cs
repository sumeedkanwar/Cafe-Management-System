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
    public partial class Form2 : Form
    {
        private string defaultUsernamePlaceholder = "Username";
        private string defaultPasswordPlaceholder = "Password";
        private string defaultFullNamePlaceholder = "Full Name";
        public Form2()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
            // Clear placeholder text when username box is clicked
            if (textBox1.Text == defaultUsernamePlaceholder)
            {
                textBox1.Text = "";
            }
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            // Show placeholder text if username box is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = defaultUsernamePlaceholder;
            }
        }

        private void TextBox2_Leave(object sender, EventArgs e)
        {
            // Show placeholder text if password box is empty
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = defaultPasswordPlaceholder;
            }
        }

        private void TextBox2_Click(object sender, EventArgs e)
        {
            // Clear placeholder text when password box is clicked
            if (textBox2.Text == defaultPasswordPlaceholder)
            {
                textBox2.Text = "";
            }
        }

        private void TextBox3_Leave(object sender, EventArgs e)
        {
            // Show placeholder text if full name box is empty
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = defaultFullNamePlaceholder;
            }
        }

        private void TextBox3_Click(object sender, EventArgs e)
        {
            // Clear placeholder text when full name box is clicked
            if (textBox3.Text == defaultFullNamePlaceholder)
            {
                textBox3.Text = "";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Create an instance of Form1
            Form1 form1 = new Form1();

            // Show Form1
            form1.Show();

            this.Close(); // Hide Form2
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            // Set placeholder text for username and password
            textBox1.Text = defaultUsernamePlaceholder;
            textBox2.Text = defaultPasswordPlaceholder;
            textBox3.Text = defaultFullNamePlaceholder;

            // Add click event handlers to clear text boxes
            textBox1.Click += TextBox1_Click;
            textBox2.Click += TextBox2_Click;
            textBox3.Click += TextBox3_Click;

            // Add leave event handlers to show placeholder text if empty
            textBox1.Leave += TextBox1_Leave;
            textBox2.Leave += TextBox2_Leave;
            textBox3.Leave += TextBox3_Leave;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string fullName = textBox3.Text;

            // Check if any of the fields are empty
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Create the SQL query to insert the user into the database
            string insertQuery = "INSERT INTO Users (username, password, fullname, user_type) VALUES (@Username, @Password, @FullName, 0)";
            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@FullName", fullName);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show("User created successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}
