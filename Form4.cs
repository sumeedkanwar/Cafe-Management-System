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
    public partial class Form4 : Form
    {
        private SqlConnection sqlConnection;
        private DataTable feedback;
        private string username;

        public Form4(string username)
        {
            InitializeComponent();
            LoadItemsFromDatabase();
            this.username = username;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            feedback = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT feedback_id as Id, rating as Rating, feedback as Feedback FROM Feedback", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(feedback);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = feedback;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(username);

            // Show Form4
            form3.Show();

            this.Close(); // Hide Form3
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
