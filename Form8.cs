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
    public partial class Form8 : Form
    {
        private SqlConnection sqlConnection;
        private DataTable inventory;
        private string username;

        public Form8(string username)
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
            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the item id, quantity, and supplier id from the user
                int itemId = Convert.ToInt32(selectedRow.Cells["Item Id"].Value);
                string inputQuantity = Microsoft.VisualBasic.Interaction.InputBox("Enter the quantity to add:", "Add Quantity", "0");
                if (!int.TryParse(inputQuantity, out int quantityToAdd) || quantityToAdd <= 0)
                {
                    MessageBox.Show("Invalid quantity.");
                    return;
                }

                string inputSupplierId = Microsoft.VisualBasic.Interaction.InputBox("Enter the supplier ID:", "Supplier ID", "0");
                if (!int.TryParse(inputSupplierId, out int supplierId) || supplierId <= 0)
                {
                    MessageBox.Show("Invalid supplier ID.");
                    return;
                }

                int shipmentId;

                // Create a new SQL connection
                using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                {
                    // Open the connection
                    connection.Open();

                    // Start a SQL transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Update the Stock table
                        using (SqlCommand updateStockCommand = new SqlCommand("UPDATE Stock SET quantity = quantity + @QuantityToAdd WHERE item_id = @ItemId", connection, transaction))
                        {
                            updateStockCommand.Parameters.AddWithValue("@QuantityToAdd", quantityToAdd);
                            updateStockCommand.Parameters.AddWithValue("@ItemId", itemId);
                            int rowsUpdated = updateStockCommand.ExecuteNonQuery();

                            if (rowsUpdated == 0)
                            {
                                // If no rows were updated, item not found in Stock table
                                throw new Exception("Item not found in stock.");
                            }
                        }

                        // Get the shipment ID
                        using (SqlCommand getShipmentIdCommand = new SqlCommand("SELECT MAX(shipment_id) FROM Shipments", connection, transaction))
                        {
                            shipmentId = (int)getShipmentIdCommand.ExecuteScalar() + 1;
                        }

                        // Insert a new shipment in the Shipments table
                        using (SqlCommand insertShipmentCommand = new SqlCommand("INSERT INTO Shipments (shipment_id, supplier_id, shipment_date, item_id, quantity) VALUES (@ShipmentId, @SupplierId, @ShipmentDate, @ItemId, @Quantity)", connection, transaction))
                        {
                            insertShipmentCommand.Parameters.AddWithValue("@ShipmentId", shipmentId);
                            insertShipmentCommand.Parameters.AddWithValue("@SupplierId", supplierId);
                            insertShipmentCommand.Parameters.AddWithValue("@ShipmentDate", DateTime.Now);
                            insertShipmentCommand.Parameters.AddWithValue("@ItemId", itemId);
                            insertShipmentCommand.Parameters.AddWithValue("@Quantity", quantityToAdd);
                            insertShipmentCommand.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        MessageBox.Show("Quantity added and shipment created successfully.");
                        // Refresh the DataGridView to reflect the updated quantity
                        LoadItemsFromDatabase();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    finally
                    {
                        // Close the connection
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item from the list.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9(username);

            form9.Show();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10(username);

            form10.Show();

            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            form1.Show();

            this.Close();
        }
    }
}
