using System;
using System.Collections;
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
    public partial class Form3 : Form
    {
        private string username;
        private SqlConnection sqlConnection;
        private DataTable selectedItemsTable;
        private DataTable itemsTable;

        public Form3(string username)
        {
            InitializeComponent();
            InitializeSelectedItemsTable();
            getOrderId();
            this.username = username;
            label6.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            itemsTable = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT item_id as ID, item_name as Name, price as Price FROM Items", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(itemsTable);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = itemsTable;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadItemsFromDatabase();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void InitializeSelectedItemsTable()
        {
            selectedItemsTable = new DataTable();
            selectedItemsTable.Columns.Add("ID", typeof(int));
            selectedItemsTable.Columns.Add("Name", typeof(string));
            selectedItemsTable.Columns.Add("Quantity", typeof(int));
            selectedItemsTable.Columns.Add("TotalPrice", typeof(decimal));
            dataGridView2.DataSource = selectedItemsTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddSelectedItem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoveSelectedItem();
        }

        private void AddSelectedItem()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)dataGridView1.SelectedRows[0].DataBoundItem;
                int itemId = (int)selectedRow["ID"];
                string itemName = (string)selectedRow["Name"];
                decimal itemPrice = (decimal)selectedRow["Price"];

                // Check if the item is already in dataGridView2
                DataRow existingRow = selectedItemsTable.AsEnumerable().FirstOrDefault(row => (int)row["ID"] == itemId);
                if (existingRow != null)
                {
                    // Increase the quantity and update the total price
                    existingRow["Quantity"] = (int)existingRow["Quantity"] + 1;
                    existingRow["TotalPrice"] = (decimal)existingRow["TotalPrice"] + itemPrice;
                }
                else
                {
                    // Add the item with quantity 1 and total price equal to item price
                    DataRow newRow = selectedItemsTable.NewRow();
                    newRow["ID"] = itemId;
                    newRow["Name"] = itemName;
                    newRow["Quantity"] = 1;
                    newRow["TotalPrice"] = itemPrice;
                    selectedItemsTable.Rows.Add(newRow);
                }
                UpdateTotalBillLabel();
            }
        }


        private void RemoveSelectedItem()
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)dataGridView2.SelectedRows[0].DataBoundItem;
                DataRow[] rows = selectedItemsTable.Select("ID = " + selectedRow["ID"]);
                if (rows.Length > 0)
                {
                    selectedItemsTable.Rows.Remove(rows[0]);
                }
                UpdateTotalBillLabel();
            }
        }
        private void UpdateTotalBillLabel()
        {
            decimal totalBill = 0;

            foreach (DataRow row in selectedItemsTable.Rows)
            {
                decimal price = Convert.ToDecimal(row["TotalPrice"]);
                totalBill += price;
            }

            label1.Text = "$" + totalBill.ToString("0.00");
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private int getStaffId(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "Username cannot be null or empty.");
            }

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT staff_id FROM Staff WHERE username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null) // Check if the result is not null before casting
                    {
                        return (int)result;
                    }
                    else
                    {
                        return -1; // Return -1 if the result is null
                    }
                }
            }
        }


        private bool checkCustomerId(int customerId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Customers WHERE customer_id = @CustomerId", connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void Checkout_Click(object sender, EventArgs e)
        {
            // Check if there are selected items to checkout
            if (selectedItemsTable.Rows.Count > 0)
            {
                int staffId = getStaffId(username);
                if (staffId == -1)
                {
                    MessageBox.Show("Staff ID not found. Please contact the administrator.", "Staff ID Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int customerId;
                bool isValidCustomerId = false;
                do
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox("Enter customer ID:", "Customer ID", "");
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return; // Exit if the input is empty
                    }

                    if (int.TryParse(input, out customerId)) // Try parsing the input as an integer
                    {
                        // Check if the customer ID exists in the database
                        isValidCustomerId = checkCustomerId(customerId);
                        if (!isValidCustomerId)
                        {
                            MessageBox.Show("Customer ID does not exist. Please enter a valid ID.", "Invalid Customer ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid input. Please enter a valid integer for Customer ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } while (!isValidCustomerId);



                // Get the current date and time
                DateTime orderDate = DateTime.Now;

                // Get the total bill from the label (assuming it's formatted as $xxx.xx)
                decimal totalBill = decimal.Parse(label1.Text.Replace("$", ""));

                int orderId;
                orderId = Convert.ToInt32(label4.Text);

                // Insert the order into the Orders table
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Orders (order_id, customer_id, order_date, staff_id, total, status) VALUES (@orderId, @customerId, @orderDate, @staffId, @totalBill, 'pending')", connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@orderDate", orderDate);
                        command.Parameters.AddWithValue("@staffId", staffId);
                        command.Parameters.AddWithValue("@totalBill", totalBill);
                        command.ExecuteNonQuery();
                    }
                }
                

                // Insert order items into Order_Items table
                foreach (DataRow row in selectedItemsTable.Rows)
                {
                    int itemId = Convert.ToInt32(row["ID"]);
                    int quantity = Convert.ToInt32(row["Quantity"]);
                    decimal totalPrice = Convert.ToDecimal(row["TotalPrice"]);

                    using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("INSERT INTO Order_Items (order_id, item_id, quantity, total) VALUES (@orderId, @itemId, @quantity, @totalPrice)", connection))
                        {
                            command.Parameters.AddWithValue("@orderId", orderId);
                            command.Parameters.AddWithValue("@itemId", itemId);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.Parameters.AddWithValue("@totalPrice", totalPrice);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                // Clear selected items table and update total bill label
                selectedItemsTable.Clear();
                UpdateTotalBillLabel();

                // Inform the user that the checkout was successful
                MessageBox.Show("Checkout successful! Order ID: " + orderId);
            }
            else
            {
                // Inform the user that there are no items to checkout
                MessageBox.Show("No items selected for checkout.");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void getOrderId()
        {

            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT MAX(order_id) FROM Orders", connection))
                {

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    count++;
                    label4.Text = count.ToString();
                }
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Staff_Feedback Staff_Feedback = new Staff_Feedback(username);

            Staff_Feedback.Show();

            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Profile form5 = new Profile(username);

            form5.Show();

            this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();

            form1.Show();

            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
