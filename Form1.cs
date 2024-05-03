using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string defaultUsernamePlaceholder = "User1";
        private string defaultPasswordPlaceholder = "Pass123!";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set placeholder text for username and password
            textBox1.Text = defaultUsernamePlaceholder;
            textBox2.Text = defaultPasswordPlaceholder;

            // Add click event handlers to clear text boxes
            textBox1.Click += TextBox1_Click;
            textBox2.Click += TextBox2_Click;

            // Add leave event handlers to show placeholder text if empty
            textBox1.Leave += TextBox1_Leave;
            textBox2.Leave += TextBox2_Leave;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Form2 form2 = new Form2();

            // Show Form2
            form2.Show();

            // Hide Form1
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Check if the username and password match in the user table
            if (CheckCredentials(username, password))
            {
                // Successful login
                MessageBox.Show("Login successful!");

                // Create an instance of Form2
                Form3 form3 = new Form3(username);

                // Show Form3
                form3.Show();

                // Hide Form1
                this.Hide();
            }
            else
            {
                // Invalid credentials
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private bool CheckCredentials(string username, string password)
        {
            bool isValid = false;
            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";
            string query = "SELECT COUNT(*) FROM Users WHERE username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    isValid = count > 0;
                }
            }

            return isValid;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !textBox2.UseSystemPasswordChar;

            // Change icon based on password visibility
            if (textBox2.UseSystemPasswordChar)
            {
                pictureBox1.Image = Properties.Resources.eye_hidden;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.eye_visible;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
