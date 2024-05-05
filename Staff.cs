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
            sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;");

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
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
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
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
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
            Orders Orders = new Orders(username);

            Orders.Show();

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

        private void Staff_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers(username);
            customers.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Feedbacks feedbacks = new Feedbacks(username);
            feedbacks.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Shipments shipments = new Shipments(username);
            shipments.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers(username);
            suppliers.Show();
            this.Close();
        }

        private void button13_Click(object sender, EventArgs e)
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

            string skillLevelInput = Microsoft.VisualBasic.Interaction.InputBox("Enter skill level (1-5):", "New Staff Information", "");
            // Validate skill level input
            if (!int.TryParse(skillLevelInput, out int skillLevel) || skillLevel < 1 || skillLevel > 5)
            {
                MessageBox.Show("Skill level must be a number between 1 and 5.");
                return;
            }

            // Insert the new staff member into the database
            InsertStaff(newUsername, newPassword, fullName, skillLevel);
        }

        private void InsertStaff(string username, string password, string fullName, int skillLevel)
        {
            int staffId = getNextStaffId();
            // Create a new SQL connection
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                // Create a SQL command to insert the new staff member
                using (SqlCommand command = new SqlCommand("INSERT INTO Users (username, password, fullname, user_type) VALUES (@Username, @Password, @FullName, 'staff'); INSERT INTO Staff (staff_id, username, skill_level) VALUES (@StaffId, @Username, @SkillLevel)", connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    command.Parameters.AddWithValue("@SkillLevel", skillLevel);

                    try
                    {
                        // Open the connection
                        connection.Open();

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Close the connection
                        connection.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("New staff member added successfully.");
                            // Reload the data after insertion
                            LoadItemsFromDatabase();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add new staff member.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
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

        private int getNextStaffId()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT MAX(staff_id) FROM Staff", connection))
                {
                    connection.Open();
                    int staffId = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    return staffId + 1;
                }
            }
        }


    }
}
