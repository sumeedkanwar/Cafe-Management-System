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
    public partial class Pending_Order : Form
    {
        private string username;
        private SqlConnection sqlConnection;
        private DataTable orders;
        public Pending_Order(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            orders = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT order_id as [Order Id], customer_id as [Customer Id], order_date as [Order Date], total as Total FROM Orders WHERE status = 'pending'", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(orders);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = orders;
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Staff_Dashboard staff_Dashboard = new Staff_Dashboard(username);
            staff_Dashboard.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int staffId = getStaffId(username);
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Orders SET status = 'delivered', staff_id = @StaffId WHERE order_id = @OrderId", connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@StaffId", staffId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Order has been delivered");
                LoadItemsFromDatabase();
            }
        }

        private int getStaffId(string username)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT staff_id FROM Staff WHERE username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
