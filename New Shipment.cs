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
    public partial class New_Shipment : Form
    {
        private int shipmentId;
        private int supplierId;
        private int itemId;
        private int quantityToAdd;
        private string username;
        public New_Shipment(string username)
        {
            this.username = username;
            InitializeComponent();
            UpdateDropdownFromDatabase();
        }

        private void New_Shipment_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate input values
            if (ValidateInput())
            {
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Get the next shipment ID
                        GetNextShipmentId(connection, transaction);

                        // Insert the new shipment
                        InsertNewShipment(connection, transaction);

                        // Commit the transaction
                        transaction.Commit();

                        MessageBox.Show("Shipment added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Rollback transaction if an exception occurs
                        MessageBox.Show("Error adding shipment: " + ex.Message);
                    }
                }
                Shipments Shipments = new Shipments(username);
                Shipments.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all the required fields.");
            }
        }

        private bool ValidateInput()
        {
            // Validate the input fields
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || numericUpDown1.Value == 0)
            {
                return false;
            }

            string selectedSupplierName = comboBox1.SelectedItem.ToString();
            string selectedItemName = comboBox2.SelectedItem.ToString();

            supplierId = GetSupplierId(selectedSupplierName);
            itemId = GetItemId(selectedItemName);
            quantityToAdd = Convert.ToInt32(numericUpDown1.Value);

            return true;
        }

        private int GetSupplierId(string supplierName)
        {
            int supplierId = 0;

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                connection.Open();
                string sqlQuery = "SELECT supplier_id FROM Suppliers WHERE supplier_name = @SupplierName";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@SupplierName", supplierName);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        supplierId = Convert.ToInt32(result);
                    }
                }
            }

            return supplierId;
        }

        private int GetItemId(string itemName)
        {
            int itemId = 0;

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                connection.Open();
                string sqlQuery = "SELECT item_id FROM Items WHERE item_name = @ItemName";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", itemName);
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        itemId = Convert.ToInt32(result);
                    }
                }

            }

            return itemId;
        }

        private void GetNextShipmentId(SqlConnection connection, SqlTransaction transaction)
        {
            // Get the next shipment ID from the database
            using (SqlCommand getShipmentIdCommand = new SqlCommand("SELECT ISNULL(MAX(shipment_id), 0) + 1 FROM Shipments", connection, transaction))
            {
                shipmentId = (int)getShipmentIdCommand.ExecuteScalar();
            }
        }

        private void InsertNewShipment(SqlConnection connection, SqlTransaction transaction)
        {
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

            using (SqlCommand updateStockCommand = new SqlCommand("UPDATE Stock SET quantity = quantity + @QuantityToAdd WHERE item_id = @ItemId", connection, transaction))
            {
                updateStockCommand.Parameters.AddWithValue("@QuantityToAdd", quantityToAdd);
                updateStockCommand.Parameters.AddWithValue("@ItemId", itemId);
                updateStockCommand.ExecuteNonQuery();
            }
        }

        private void UpdateDropdownFromDatabase()
        {
            // Clear existing items in the dropdown
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            // Initialize connection
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                // Open connection
                connection.Open();

                // Create and execute SQL command to select items from database
                string sqlQuery = "SELECT supplier_name FROM Suppliers";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    // Execute the command and retrieve data
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the reader has rows
                        if (reader.HasRows)
                        {
                            // Iterate through each row and add the item to the dropdown
                            while (reader.Read())
                            {
                                string supplierName = reader.GetString(0); // Assuming supplier name is in the first column
                                comboBox1.Items.Add(supplierName);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found in the database.");
                        }
                    }
                }

                string sqlQuery2 = "SELECT item_name FROM Items";
                using (SqlCommand command = new SqlCommand(sqlQuery2, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string itemName = reader.GetString(0);
                                comboBox2.Items.Add(itemName);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found in the database.");
                        }
                    }
                }
            }
        }

    }
}
