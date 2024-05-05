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
    public partial class Shipments : Form
    {
        private string username;
        private SqlConnection sqlConnection;
        private DataTable shipments;
        public Shipments(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadItemsFromDatabase();
        }

        private void Shipments_Load(object sender, EventArgs e)
        {

        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            shipments = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT shipment_id as [Shipment Id], supplier_id as [Supplier Id], shipment_date as [Shipment Date], item_id as [Item Id], quantity as Quantity FROM Shipments", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(shipments);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = shipments;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            New_Shipment new_Shipment = new New_Shipment(username);
            new_Shipment.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory(username);
            inventory.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Feedbacks feedbacks = new Feedbacks(username);
            feedbacks.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers(username);
            suppliers.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders(username);
            orders.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers(username);
            customers.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Staff staff = new Staff(username);
            staff.Show();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Profile profile
                = new Profile(username);
            profile.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }
    }
}
