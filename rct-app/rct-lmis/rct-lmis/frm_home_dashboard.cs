using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_dashboard : Form
    {
        private string _username;

        public frm_home_dashboard(string username)
        {
            InitializeComponent();
            _username = username;

            dgvusersonline.ClearSelection();
        }

        private void frm_home_dashboard_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            PopulateDataGridView();

           
        }

        private void InitializeDataGridView()
        {
            // Clear existing columns if any
            dgvusersonline.Columns.Clear();
            dgvusersonline.ClearSelection();

            // Add columns
            dgvusersonline.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name" });
            dgvusersonline.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status" });

            // Adjust column widths and other properties as needed
            dgvusersonline.Columns["Name"].Width = 175; // Adjust width for user's name column
            dgvusersonline.Columns["Status"].Width = 100; // Adjust width for status column

            // Show headers
            dgvusersonline.ColumnHeadersVisible = false;

            dgvusersonline.CellFormatting += dgvusersonline_CellFormatting;
            dgvusersonline.DataBindingComplete += dgvusersonline_DataBindingComplete;

        }

        private void PopulateDataGridView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loginStatusCollection = database.GetCollection<LoginStatus>("login-status");

                // Get all users with their last login status
                var users = loginStatusCollection.AsQueryable()
                    .GroupBy(ls => ls.UserId)
                    .Select(group => group.OrderByDescending(ls => ls.LoginTime).FirstOrDefault())
                    .ToList();

                // Clear existing rows
                dgvusersonline.Rows.Clear();
                dgvusersonline.ClearSelection();

                // Create a list to hold the formatted data
                var userData = new List<UserData>();

                foreach (var user in users)
                {
                    bool isLoggedIn = user.IsLoggedIn; // Assuming IsLoggedIn property indicates current online status

                    userData.Add(new UserData
                    {
                        Name = user.Name,
                        IsLoggedIn = isLoggedIn
                    });
                }

                // Sort the userData list so that online users are at the top
                var sortedUserData = userData.OrderByDescending(u => u.IsLoggedIn).ToList();

                // Add rows to DataGridView
                foreach (var user in sortedUserData)
                {
                    string statusText = user.IsLoggedIn ? "Online" : "Offline";
                    Color statusColor = user.IsLoggedIn ? Color.Green : Color.Gray;

                    // Add row to DataGridView
                    int rowIndex = dgvusersonline.Rows.Add(user.Name, statusText);
                    dgvusersonline.Rows[rowIndex].Cells["Status"].Style.ForeColor = statusColor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching login status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvusersonline_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvusersonline.Columns["Status"].Index)
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Online")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else if (status == "Offline")
                    {
                        e.CellStyle.ForeColor = Color.Gray;
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvusersonline_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvusersonline.ClearSelection();
        }

        private void dgvusersonline_MouseLeave(object sender, EventArgs e)
        {
            dgvusersonline.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm_test_upload tu = new frm_test_upload();
            tu.ShowDialog();
        }
    }

    public class UserData
    {
        public string Name { get; set; }
        public bool IsLoggedIn { get; set; }
    }

    public class LoginStatus
    {
        [BsonId]
        public ObjectId Id { get; set; } // Maps to the '_id' field in MongoDB
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; } // Changed to nullable to allow for null values
    }
}
