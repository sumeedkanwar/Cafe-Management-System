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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Customers : Form
    {
        private SqlConnection sqlConnection;
        private readonly string username;
        private DataTable customers;


        public Customers(string username)
        {
            this.username = username;
            InitializeComponent();
            LoadItemsFromDatabase();
        }
        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection using the existing sqlConnection object's connection string
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            customers = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT s.customer_id as [Customer Id], u.fullname as [Customer Name] FROM Customers s join Users u on u.username = s.username", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(customers);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = customers;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customer_Trends customer_Trends = new Customer_Trends(username);
            customer_Trends.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Staff staff = new Staff(username);
            staff.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders(username);
            orders.Show();
            this.Close();
        }

        private void Customers_Load(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Prompt the user for new staff information
            string newUsername = Microsoft.VisualBasic.Interaction.InputBox("Enter username:", "New Staff Information", "");
            if (IsUsernameExists(newUsername))
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return;
            }

            string newPassword = Microsoft.VisualBasic.Interaction.InputBox("Enter password:", "New Staff Information", "");
            if (newPassword.Length < 8 || !IsPasswordComplex(newPassword))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                return;
            }
            string fullName = Microsoft.VisualBasic.Interaction.InputBox("Enter full name:", "New Staff Information", "");
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Full name cannot be empty.");
                return;
            }

            int customerId = getNextCustomerId();

            // Create a new SQL connection
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                // Create a new SQL command to insert the new staff member
                using (SqlCommand command = new SqlCommand("INSERT INTO Users (username, password, fullname, user_type) VALUES (@Username, @Password, @Fullname, 'customer'); INSERT INTO Customers VALUES(@CustomerId, @Username);", connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Username", newUsername);
                    command.Parameters.AddWithValue("@Password", newPassword);
                    command.Parameters.AddWithValue("@Fullname", fullName);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    // Open the connection
                    connection.Open();

                    // Execute the command
                    command.ExecuteNonQuery();

                    // Close the connection
                    connection.Close();
                }
            }
            LoadItemsFromDatabase();
        }

        private bool IsUsernameExists(string username)
        {
            // Create a new SQL connection
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                // Create a SQL command to check if the username exists
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE username = @Username", connection))
                {
                    // Add parameter to the command
                    command.Parameters.AddWithValue("@Username", username);

                    // Open the connection
                    connection.Open();

                    // Execute the command and check if any rows are returned (username exists)
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Close the connection
                    connection.Close();

                    return count > 0;
                }
            }
        }

        private bool IsPasswordComplex(string password)
        {
            // Password complexity check (at least one uppercase, one lowercase, one digit, one special character)
            return password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit) && password.Any(c => !char.IsLetterOrDigit(c));
        }

        private int getNextCustomerId()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT MAX(customer_id) FROM Customers", connection))
                {
                    connection.Open();
                    int customerId = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    return customerId + 1;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers(username);
            suppliers.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Shipments shipments = new Shipments(username);
            shipments.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Feedbacks feedbacks = new Feedbacks(username);
            feedbacks.Show();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory(username);
            inventory.Show();
            this.Close();

        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;

                int id = Convert.ToInt32(customers.Rows[selectedIndex]["Customer Id"]);

                using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                {

                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Customers WHERE customer_id = @id;", connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@id", id);

                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                LoadItemsFromDatabase();
            }
        }

        private string getUsername(int customer_id)
        {
            string username = "";
            using (SqlConnection connection = new SqlConnection(sqlConnection.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT username FROM Customers WHERE customer_id = @CustomerId", connection))
                {
                    sqlCommand.Parameters.AddWithValue("@CustomerId", customer_id);
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            username = reader["username"].ToString();
                        }
                    }
                }
            }
            return username;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;

                int id = Convert.ToInt32(customers.Rows[selectedIndex]["Customer Id"]);
                Change_Customer_Details change_Customer_Details = new Change_Customer_Details(username, id);
                change_Customer_Details.Show();
                this.Close();

            }
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
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
