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
    public partial class Profile : Form
    {
        private string username;
        private string type;
        private SqlConnection connection;

        public Profile(string username)
        {
            InitializeComponent();
            this.username = username;
            label1.Text = username;
            getTypes();
        }

        private void getTypes()
        {
            using (connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT user_type FROM Users WHERE username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    type = (string)command.ExecuteScalar();
                }
            }
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
            if (type == "staff")
            {
                Staff_Dashboard Staff_Dashboard = new Staff_Dashboard(username);
                Staff_Dashboard.Show();
                this.Close();
            }
            else if (type == "admin")
            {
                Orders Orders = new Orders(username);
                Orders.Show();
                this.Close();
            }
            else
            {
                Customer_Dashboard Customer_Dashboard = new Customer_Dashboard(username);
                Customer_Dashboard.Show();
                this.Close();
            }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
