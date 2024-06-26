﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Customer_Dashboard : Form
    {
        private SqlConnection sqlConnection;
        private DataTable itemsTable;
        private string username;
        private DataTable selectedItemsTable;
        public Customer_Dashboard(string username)
        {
            this.username = username;
            InitializeComponent();
            LoadItemsFromDatabase();
            InitializeSelectedItemsTable();
            label6.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

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

        private void InitializeSelectedItemsTable()
        {
            selectedItemsTable = new DataTable();
            selectedItemsTable.Columns.Add("ID", typeof(int));
            selectedItemsTable.Columns.Add("Name", typeof(string));
            selectedItemsTable.Columns.Add("Quantity", typeof(int));
            selectedItemsTable.Columns.Add("TotalPrice", typeof(decimal));
            dataGridView2.DataSource = selectedItemsTable;
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

        private void UpdateTotalBillLabel()
        {
            decimal totalBill = 0;

            foreach (DataRow row in selectedItemsTable.Rows)
            {
                decimal price = Convert.ToDecimal(row["TotalPrice"]);
                totalBill += price;
            }

            label7.Text = "$" + totalBill.ToString("0.00");
        }

        private int getCustomerId(string username)
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT customer_id FROM Customers WHERE username = @username", sqlConnection))
            {
                // Add the parameter for the username
                sqlCommand.Parameters.AddWithValue("@username", username);

                // Open the SQL connection
                sqlConnection.Open();
                var result = sqlCommand.ExecuteScalar();
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

        private int getOrderId()
        {

            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT MAX(order_id) FROM Orders", connection))
                {

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    count++;
                    return count;
                }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Customer_Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void Checkout_Click(object sender, EventArgs e)
        {
            int customerId = getCustomerId(username);
            int orderId = getOrderId();

            // Check if there are selected items to checkout
            if (selectedItemsTable.Rows.Count > 0)
            {
                // Get the current date and time
                DateTime orderDate = DateTime.Now;

                // Get the total bill from the label (assuming it's formatted as $xxx.xx)
                decimal totalBill = decimal.Parse(label7.Text.Replace("$", ""));

                // Insert the order into the Orders table
                using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Orders (order_id, customer_id, order_date, total, status) VALUES (@orderId, @customerId, @orderDate, @totalBill, 'pending')", connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@orderDate", orderDate);
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

                    using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
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


        private void button1_Click(object sender, EventArgs e)
        {
            AddSelectedItem();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoveSelectedItem();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Profile form5 = new Profile(username);

            form5.Show();

            this.Close();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Customer_Feedback form6 = new Customer_Feedback(username);
            form6.Show();
            this.Close();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();
            form1.Show();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile(username);
            profile.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Customer_Feedback customer_Feedback = new Customer_Feedback(username);
            customer_Feedback.Show();
            this.Close();
        }
    }
}
