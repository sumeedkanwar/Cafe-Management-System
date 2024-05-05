using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        private string defaultUsernamePlaceholder = "Username";
        private string defaultPasswordPlaceholder = "Password";

        public Login()
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
            if (textBox1.Text == defaultUsernamePlaceholder)
            {
                textBox1.Text = "";
            }
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = defaultUsernamePlaceholder;
            }
        }

        private void TextBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = defaultPasswordPlaceholder;
            }
        }

        private void TextBox2_Click(object sender, EventArgs e)
        {
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
            Signup form2 = new Signup();

            form2.Show();

            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            string type = CheckCredentials(username, password);

            if (type == "staff")
            {
                MessageBox.Show("Login successful!");

                Staff_Dashboard Staff_Dashboard = new Staff_Dashboard(username);

                Staff_Dashboard.Show();

                this.Hide();
                
            }
            else if (type == "admin")
            {
                MessageBox.Show("Login successful!");

                Orders Orders = new Orders(username);
                Orders.Show();
                this.Hide();
            }
            else if (type == "customer")
            {
                MessageBox.Show("Login successful!");

                Customer_Dashboard Customer_Dashboard = new Customer_Dashboard(username);

                Customer_Dashboard.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }

        }

        private string CheckCredentials(string username, string password)
        {
            string type = "";
            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";
            string query = "SELECT user_type FROM Users WHERE username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    type = (string)command.ExecuteScalar();
                    connection.Close();
                }
            }

            return type;
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
