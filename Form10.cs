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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Form10 : Form
    {
        private string username;
        private SqlConnection sqlConnection;
        private DataTable orders;
        public Form10(string username)
        {
            InitializeComponent();
            LoadItemsFromDatabase();
            this.username = username;
        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            orders = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT order_id as [Order Id], customer_id as [Customer Id], order_date as [Order Date], staff_id as [Staff Id], total as Total FROM Orders", sqlConnection))
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

        private void button3_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9(username);

            form9.Show();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8(username);

            form8.Show();

            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Give total Sales
            double totalSales = 0;
            int totalOrders = 0;
            foreach (DataRow row in orders.Rows)
            {
                totalSales += Convert.ToDouble(row["Total"]);
                totalOrders++;
            }
            MessageBox.Show("Total Sales: " + totalSales + "\nTotal Orders: " + totalOrders);
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            form1.Show();

            this.Close();
        }
    }
}
