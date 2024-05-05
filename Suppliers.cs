using Microsoft.VisualBasic;
using System;
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
    public partial class Suppliers : Form
    {
        private SqlConnection sqlConnection;
        private DataTable supplier;
        private readonly string username;

        public Suppliers(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadItemsFromDatabase();
        }

        private void Suppliers_Load(object sender, EventArgs e)
        {
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            supplier = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT supplier_id as Id, supplier_name as Name FROM Suppliers", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(supplier);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = supplier;
        }

        private int getSupplierId()
        {
            int num = -1;
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT MAX(supplier_id) FROM Suppliers", connection))
                {

                    connection.Open();
                    num = (int)command.ExecuteScalar();
                    num++;
                }
            }
            return num;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = getSupplierId();
            string name = Interaction.InputBox("Enter the name of the supplier", "Supplier Name", "");
            if (name != "")
            {
                using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Suppliers (supplier_id, supplier_name) VALUES (@id, @name)", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@name", name);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }

                LoadItemsFromDatabase();
            }
            else
            {
                MessageBox.Show("Please enter a valid name");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;

                int id = Convert.ToInt32(supplier.Rows[selectedIndex]["Id"]);

                using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Suppliers WHERE supplier_id = @id", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }

                LoadItemsFromDatabase();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Shipments shipments = new Shipments(username);
            shipments.Show();
            this.Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Feedbacks feedbacks = new Feedbacks(username);
            feedbacks.Show();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
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

        private void button12_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory(username);
            inventory.Show();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile(username);
            profile.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
