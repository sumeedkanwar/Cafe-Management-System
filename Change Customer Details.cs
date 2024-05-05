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
    public partial class Change_Customer_Details : Form
    {
        private string username;
        private int customer_id;
        private SqlConnection sqlConnection;
        public Change_Customer_Details(string username, int customer_id)
        {
            InitializeComponent();
            this.username = username;
            this.customer_id = customer_id;
            GetCustomerDetails();
        }

        private void GetCustomerDetails()
        {
            using (sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT c.username, u.fullname FROM Customers c JOIN Users u on u.username = c.username WHERE customer_id = @CustomerId", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@CustomerId", customer_id);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label4.Text = reader["username"].ToString();
                            textBox1.Text = reader["fullname"].ToString();
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                using (sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET fullname = @FullName WHERE username = @username", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@FullName", textBox1.Text);
                        sqlCommand.Parameters.AddWithValue("@CustomerId", customer_id);
                        sqlCommand.Parameters.AddWithValue("@username", label4.Text);
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Customer details updated successfully");
                    }
                }
                Customers customers = new Customers(username);
                customers.Show();
                this.Close();
            }
            else if (textBox3.Text != textBox2.Text)
            {
                MessageBox.Show("Passwords do not match");
            }
            else
            {
                using (sqlConnection = new SqlConnection("Data Source=SUMEED;Initial Catalog=Project;Integrated Security=True;"))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE Users SET fullname = @FullName, password = @Password WHERE username = @username", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@FullName", textBox1.Text);
                        sqlCommand.Parameters.AddWithValue("@Password", textBox2.Text);
                        sqlCommand.Parameters.AddWithValue("@Username", label4.Text);
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Customer details updated successfully");
                    }
                }
                Customers customers = new Customers(username);
                customers.Show();
                this.Close();
                
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers(username);
            customers.Show();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Change_Customer_Details_Load(object sender, EventArgs e)
        {

        }
    }
}
