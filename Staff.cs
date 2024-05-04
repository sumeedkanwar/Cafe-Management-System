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
    public partial class Staff : Form
    {
        private SqlConnection sqlConnection;
        private DataTable staff;
        private string username;
        public Staff(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadItemsFromDatabase();
        }

        private void LoadItemsFromDatabase()
        {
            // Create a new SQL connection
            sqlConnection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;");

            // Create a new data table to store the items
            staff = new DataTable();

            // Create a new SQL command
            using (SqlCommand sqlCommand = new SqlCommand("SELECT s.staff_id as [Staff Id], u.fullname as [Staff Name], s.skill_level as [Skill Level] FROM Staff s join Users u on u.username = s.username", sqlConnection))
            {
                // Open the SQL connection
                sqlConnection.Open();

                // Create a new SQL data adapter
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    // Fill the data table with the results of the SQL command
                    sqlDataAdapter.Fill(staff);
                }

                // Close the SQL connection
                sqlConnection.Close();
            }

            // Set the data source of the data grid view to the data table
            dataGridView1.DataSource = staff;
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

                // Get the staff ID and name from the selected row
                int staffId = Convert.ToInt32(selectedRow.Cells["Staff Id"].Value);
                string staffName = Convert.ToString(selectedRow.Cells["Staff Name"].Value);

                // Ask for confirmation before deleting
                DialogResult result = MessageBox.Show("Are you sure you want to delete " + staffName + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Perform the deletion
                    DeleteStaff(staffId);
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member from the list.");
            }
        }

        private void DeleteStaff(int staffId)
        {
            // Create a new SQL connection
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                // Create a SQL command to delete the staff member
                using (SqlCommand command = new SqlCommand("DELETE FROM Staff WHERE staff_id = @StaffId", connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@StaffId", staffId);

                    // Open the connection
                    connection.Open();

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Close the connection
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Staff member deleted successfully.");
                        // Reload the data after deletion
                        LoadItemsFromDatabase();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete staff member.");
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the staff ID from the selected row
                int staffId = Convert.ToInt32(selectedRow.Cells["Staff Id"].Value);

                // Prompt the user for the new skill level
                string newSkillLevel = Microsoft.VisualBasic.Interaction.InputBox("Enter the new skill level:", "Edit Skill Level", selectedRow.Cells["Skill Level"].Value.ToString());

                // Update the staff information
                EditStaff(staffId, newSkillLevel);
            }
            else
            {
                MessageBox.Show("Please select a staff member from the list.");
            }
        }

        private void EditStaff(int staffId, string newSkillLevel)
        {
            // Create a new SQL connection
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-HFACQ64;Initial Catalog=Project;Integrated Security=True;"))
            {
                // Create a SQL command to update the staff information
                using (SqlCommand command = new SqlCommand("UPDATE Staff SET skill_level = @NewSkillLevel WHERE staff_id = @StaffId", connection))
                {
                    try
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@NewSkillLevel", newSkillLevel);
                        command.Parameters.AddWithValue("@StaffId", staffId);

                        // Open the connection
                        connection.Open();

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Close the connection
                        connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Staff information updated successfully.");
                            // Reload the data after updating
                            LoadItemsFromDatabase();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update staff information.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10(username);

            form10.Show();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inventory form8 = new Inventory(username);

            form8.Show();

            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Login form1 = new Login();

            form1.Show();

            this.Close();
        }
    }
}
