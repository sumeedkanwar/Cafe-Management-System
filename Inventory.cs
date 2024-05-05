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
    public partial class Inventory : Form
    {
        private SqlConnection sqlConnection;
        private DataTable inventory;
        private string username;

        public Inventory(string username)
        {
            this.username = username;
            InitializeComponent();
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            inventory = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT stock_id as [Stock Id], item_id as [Item Id], quantity as Quantity FROM Stock", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(inventory);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = inventory;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form8_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            string item_id = Microsoft.VisualBasic.Interaction.InputBox("Enter Item Id", "Remove Expired Ites", "0", 0, 0);
            if (doesItemExist(Convert.ToInt32(item_id)))
            {
                int quantity = Convert.ToInt32(Microsoft.VisualBasic.Interaction.InputBox("Enter Quantity to Discard", "Remove Expired Item", "0", 0, 0));
                if (quantity > 0)
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Stock SET quantity = quantity - @quantity WHERE item_id = @item_id", connection))
                        {
                            connection.Open();
                            command.Parameters.AddWithValue("@item_id", item_id);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    MessageBox.Show("Item discarded successfully");
                    LoadItemsFromDatabase();
                }
            }
            else
            {
                MessageBox.Show("Item does not exist");
            }
        }

        private bool doesItemExist(int itemId)
        {             using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Stock WHERE item_id = @itemId", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@itemId", itemId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Staff form9 = new Staff(username);

            form9.Show();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orders Orders = new Orders(username);

            Orders.Show();

            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();

            form1.Show();

            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Shipments Shipments = new Shipments(username);
            Shipments.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers(username);
            customers.Show();
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
    }
}
