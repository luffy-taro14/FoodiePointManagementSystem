using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FoodiePointManagementSystem
{
    public partial class ManagerForm : Form
    {
        private int currentUserID; // Store the current logged-in Manager's UserID

        public ManagerForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }

        // Load the Menu Data
        private void LoadMenuData()
        {
            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MenuID, MenuName, Price, Category FROM Menu";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvMenu.DataSource = dataTable; // Bind data to DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Load the Hall Data
        private void LoadHallData()
        {
            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT HallID, HallName, Capacity, PartyType, Price FROM Hall";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvHall.DataSource = dataTable; // Bind data to DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Load the Reservation Data
        private void LoadReservationData()
        {
            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            Reservations.ReservationID, 
                            Users.UserName AS CustomerName, 
                            Hall.HallName, 
                            Reservations.EventType, 
                            Reservations.Status
                        FROM 
                            Reservations
                        JOIN 
                            Users ON Reservations.CustomerID = Users.UserID
                        JOIN 
                            Hall ON Reservations.HallID = Hall.HallID";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvReservations.DataSource = dataTable; // Bind data to DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Update Manager's Profile
        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Users SET UserName = @UserName, Email = @Email, Password = @Password WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@UserID", currentUserID);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Profile updated successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Add New Menu Item
        private void btnAddMenu_Click(object sender, EventArgs e)
        {
            string menuName = txtMenuName.Text;
            decimal price = decimal.Parse(txtPrice.Text);
            string category = txtCategory.Text;

            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Menu (MenuName, Price, Category) VALUES (@MenuName, @Price, @Category)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuName", menuName);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Category", category);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Menu Added Successfully");
                    LoadMenuData(); // Reload the menu data
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Delete Menu Item
        private void btnDeleteMenu_Click(object sender, EventArgs e)
        {
            int menuId = int.Parse(txtMenuID.Text);

            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Menu WHERE MenuID = @MenuID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuID", menuId);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Menu Deleted Successfully");
                    LoadMenuData(); // Reload the menu data
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Update Menu Item
        private void btnUpdateMenu_Click(object sender, EventArgs e)
        {
            int menuId = int.Parse(txtMenuID.Text);
            string menuName = txtMenuName.Text;
            decimal price = decimal.Parse(txtPrice.Text);
            string category = txtCategory.Text;

            string connectionString = "Server=LENO\\SQLEXPRESS;Database=FoodiePointDB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Menu SET MenuName = @MenuName, Price = @Price, Category = @Category WHERE MenuID = @MenuID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuName", menuName);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@MenuID", menuId);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Menu Updated Successfully");
                    LoadMenuData(); // Reload the menu data
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Form Load
        private void ManagerForm_Load(object sender, EventArgs e)
        {
            LoadMenuData(); // Load all menu data into DataGridView
            LoadHallData(); // Load all hall data into DataGridView
            LoadReservationData(); // Load all reservation data into DataGridView
        }
    }
}

