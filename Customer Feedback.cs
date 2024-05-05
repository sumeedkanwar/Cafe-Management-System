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
    public partial class Customer_Feedback : Form
    {
        private string username;
        private SqlConnection connection;
        private DataTable feedbacks;
        public Customer_Feedback(string username)
        {
            this.username = username;
            InitializeComponent();
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            // Initialize the connection string
            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";

            // Create a new SQL connection with the initialized connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a new data table to store the items
                DataTable feedbacks = new DataTable();

                int customerId = getCustomerId(username);

                // Create a new SQL command to fetch orders without feedback
                string sqlQuery = "SELECT O.order_id, O.order_date, O.total " +
                                  "FROM Orders O " +
                                  "WHERE O.customer_id = @CustomerId " +
                                  "AND NOT EXISTS (SELECT 1 FROM Feedback F WHERE F.order_id = O.order_id AND F.customer_id = @CustomerId)";

                // Create a new SQL command
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@CustomerId", customerId);

                    // Open the SQL connection
                    connection.Open();

                    // Create a new SQL data adapter
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        // Fill the data table with the results of the SQL command
                        sqlDataAdapter.Fill(feedbacks);
                    }

                    // Close the SQL connection
                    connection.Close();
                }

                // Set the data source of the data grid view to the data table
                dataGridView1.DataSource = feedbacks;
            }
        }



        private int getCustomerId(string username)
        {
            using (connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT customer_id FROM Customers WHERE username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        private void Customer_Feedback_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the order ID from the selected row
                int orderId = Convert.ToInt32(selectedRow.Cells["order_id"].Value);

                // Prompt the user to enter feedback and rating
                string feedback = Microsoft.VisualBasic.Interaction.InputBox("Enter your feedback:", "Feedback");
                int rating = Convert.ToInt32(Microsoft.VisualBasic.Interaction.InputBox("Enter your rating (1-5):", "Rating"));

                // Check if the rating is within the valid range
                if (rating >= 1 && rating <= 5)
                {
                    // Update the database with the feedback and rating
                    UpdateFeedback(orderId, feedback, rating);

                    // Reload the items from the database to refresh the DataGridView
                    LoadItemsFromDatabase();
                }
                else
                {
                    MessageBox.Show("Please enter a rating between 1 and 5.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to provide feedback.");
            }
        }

        private void UpdateFeedback(int orderId, string feedback, int rating)
        {
            // Initialize the connection string
            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";

            // Create a new SQL connection with the initialized connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Get the customer ID using the username
                int customerId = getCustomerId(username);

                // Get the max feedback_id and increment it by one
                int feedbackId = GetNextFeedbackId(connection);

                // Create a new SQL command to insert feedback into the database
                string sqlQuery = "INSERT INTO Feedback (feedback_id, customer_id, rating, feedback, order_id) " +
                                  "VALUES (@FeedbackId, @CustomerId, @Rating, @Feedback, @OrderId)";

                // Create a new SQL command
                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection))
                {
                    // Add parameters to the SQL command
                    sqlCommand.Parameters.AddWithValue("@FeedbackId", feedbackId);
                    sqlCommand.Parameters.AddWithValue("@CustomerId", customerId);
                    sqlCommand.Parameters.AddWithValue("@Rating", rating);
                    sqlCommand.Parameters.AddWithValue("@Feedback", feedback);
                    sqlCommand.Parameters.AddWithValue("@OrderId", orderId);

                    // Open the SQL connection
                    connection.Open();

                    // Execute the SQL command to insert feedback
                    sqlCommand.ExecuteNonQuery();

                    // Close the SQL connection
                    connection.Close();
                }
            }
        }

        private int GetNextFeedbackId(SqlConnection connection)
        {
            int feedbackId = 0;
            string query = "SELECT MAX(feedback_id) FROM Feedback";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    feedbackId = Convert.ToInt32(result);
                }
                connection.Close();
            }

            // Increment the feedbackId by one
            return feedbackId + 1;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Customer_Dashboard Customer_Dashboard = new Customer_Dashboard(username);
            Customer_Dashboard.Show();
            this.Close();
        }
    }
}
