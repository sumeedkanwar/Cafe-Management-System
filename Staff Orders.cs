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
    public partial class Staff_Orders : Form
    {
        string username;
        private SqlConnection sqlConnection;
        private DataTable orders;

        public Staff_Orders(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadItemsFromDatabase();
            

        }
        private int getStaffId(string username)
        {
            int staffId = -1;
            string connectionString = "Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT staff_id FROM Staff WHERE username = @username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        staffId = Convert.ToInt32(result);
                    }
                }
            }
            return staffId;
        }


        private void LoadItemsFromDatabase()
        {
            int staffId = getStaffId(username);

            // Create a new SQL connection
            string connectionString = "Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;";
            using (sqlConnection = new SqlConnection(connectionString))
            {
                string query = "SELECT O.order_id AS OrderID,O.order_date AS OrderDate, C.username AS [Customer Name], O.total AS Total,F.feedback AS Feedback FROM Orders O LEFT JOIN Customers C ON O.customer_id = C.customer_id LEFT JOIN Feedback F ON O.order_id = F.order_id WHERE O.staff_id = @staffId";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@staffId", staffId);
                    sqlConnection.Open();

                    orders = new DataTable();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(orders);
                    }

                    dataGridView1.DataSource = orders;
                }
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Staff_Orders_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Staff_Dashboard staff_Dashboard = new Staff_Dashboard(username);
            staff_Dashboard.Show();
            this.Hide();
        }
    }
}
