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
    public partial class Staff_Feedback : Form
    {
        private SqlConnection sqlConnection;
        private DataTable feedback;
        private string username;

        public Staff_Feedback(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private int getStaffId(string username)
        {
            int staffId = -1;
            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT staff_id FROM Staff WHERE username = @username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        staffId = Convert.ToInt32(result);
                    }
                }
            }
            return staffId;
        }



        private void LoadItemsFromDatabase()
        {
            int staffId = getStaffId(username);
            if (staffId == -1)
            {
                MessageBox.Show("Staff ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;";
            using (sqlConnection = new SqlConnection(connectionString))
            {
                string query = "SELECT f.feedback_id as Id, f.rating as Rating, f.feedback as Feedback " +
                               "FROM Feedback f " +
                               "JOIN Orders o ON o.order_id = f.order_id " +
                               "JOIN Staff s ON s.staff_id = o.staff_id " +
                               "WHERE s.staff_id = @staffId;";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@staffId", staffId);
                    sqlConnection.Open();

                    feedback = new DataTable();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(feedback);
                    }

                    dataGridView1.DataSource = feedback;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Staff_Dashboard Staff_Dashboard = new Staff_Dashboard(username);

            // Show Staff_Feedback
            Staff_Dashboard.Show();

            this.Close(); // Hide Staff_Dashboard
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Staff_Feedback_Load(object sender, EventArgs e)
        {
            LoadItemsFromDatabase();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
