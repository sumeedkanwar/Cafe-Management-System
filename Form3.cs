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
    public partial class Form3 : Form
    {
        private SqlConnection sqlConnection;
        private DataTable selectedItemsTable;
        private DataTable itemsTable;

        public Form3()
        {
            InitializeComponent();
            InitializeSelectedItemsTable();

        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

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
                    newRow["Quantity"] = 1;
                    newRow["TotalPrice"] = itemPrice;
                    selectedItemsTable.Rows.Add(newRow);
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
            }
        }
    }
}
