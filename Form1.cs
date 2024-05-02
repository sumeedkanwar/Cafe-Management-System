using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                // Proceed to next form or action
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
            try
            {
                sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Encrypt=True;TrustServerCertificate=true;");
                SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM [User] WHERE username = '" + username + "' AND password = '" + password + "'", sqlConnection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int count = Convert.ToInt32(dt.Rows[0][0]);
                if (count > 0)
                {
                       isValid = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
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
    }
}
