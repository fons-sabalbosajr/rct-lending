﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using rct_lmis.SIGNUP_LOGIN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_signupaccounts : Form
    {
        public frm_home_ADMIN_signupaccounts()
        {
            InitializeComponent();
            grpactivate.Enabled = false;
        }

        LoadingFunction load = new LoadingFunction();

        private async void LoadUserData() 
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<User>("usersign_up");

            var users = await collection.Find(_ => true).ToListAsync();

            if (users.Count == 0)
            {
                lnodata.Visible = true; // Show the label when there are no users
                dgvdata.DataSource = null; // Clear the DataGridView
            }
            else
            {
                lnodata.Visible = false; // Hide the label when there are users

                // Clear the selection to avoid unexpected behavior
                dgvdata.ClearSelection();

                dgvdata.DataSource = users.Select(u => new
                {
                    u.FullName,
                    u.Email,
                    u.Password
                }).ToList();
            }
        }

        private async Task<string> GenerateSystemID(string role)
        {
            string roleAbbreviation = "";
            switch (role)
            {
                case "Staff":
                    roleAbbreviation = "S";
                    break;
                case "Team Leader":
                    roleAbbreviation = "TM";
                    break;
                case "Supervisor":
                    roleAbbreviation = "SU";
                    break;
                case "Administrator":
                    roleAbbreviation = "AD";
                    break;
                default:
                    roleAbbreviation = "S"; // Default to "Staff" if role is not recognized
                    break;
            }

            // Retrieve the highest existing ID from the database
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<User>("user_accounts");

            var filter = Builders<User>.Filter.Regex("SystemID", new BsonRegularExpression($"RCT-U[0-9]+-{roleAbbreviation}"));
            var sort = Builders<User>.Sort.Descending("SystemID");

            var latestUser = await collection.Find(filter).Sort(sort).FirstOrDefaultAsync();

            int latestNumber = 1000; // Default to 1000 if no users exist
            if (latestUser != null)
            {
                string latestID = latestUser.SystemID;
                string numberPart = latestID.Split('-')[1].Substring(1); // Extract the numeric part
                latestNumber = int.Parse(numberPart);
            }

            // Increment the number
            latestNumber++;

            // Generate the system ID
            string systemID = $"RCT-U{latestNumber}-{roleAbbreviation}";

            return systemID;
        }

        private async Task SaveUserDataToMongoDB(string systemID, string name, string position, string email, string username, string password, string designation, Image photo)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<User>("user_accounts");

                byte[] photoBytes = ImageToByteArray(photo); // Convert Image to byte array

                User newUser = new User
                {
                    SystemID = systemID,
                    FullName = name,
                    Position = position,
                    Email = email,
                    Username = username,
                    Password = password,
                    Designation = designation,
                    Photo = photoBytes
                };

                await collection.InsertOneAsync(newUser);
                //MessageBox.Show("User data saved to MongoDB successfully!");
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user data to MongoDB: {ex.Message}");
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        private async Task DeleteUserFromSignupCollection(string email)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<User>("usersign_up");

                var filter = Builders<User>.Filter.Eq("Email", email);

                await collection.DeleteOneAsync(filter);
                //MessageBox.Show("User deleted from signup collection successfully!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user from signup collection: {ex.Message}");
            }
        }

        private void dgvdataload() 
        {
            if (dgvdata.SelectedRows.Count > 0)
            {
                lnodata.Visible = false;
                tname.Text = dgvdata.SelectedRows[0].Cells[0].Value.ToString();
                temail.Text = dgvdata.SelectedRows[0].Cells[1].Value.ToString();
                tpass.Text = dgvdata.SelectedRows[0].Cells[2].Value.ToString();
            }
        }

        private void frm_home_ADMIN_signupaccounts_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           
            dgvdata.ClearSelection();
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvdataload();
        }

        private void bapproveuser_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Activate Account", "Activate Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                grpactivate.Enabled = true;
                tnamenew.Text = tname.Text;
            }
        }

        private void cbuserlevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRole = cbuserlevel.SelectedItem.ToString();
            Task.Run(async () =>
            {
                string systemID = await GenerateSystemID(selectedRole);
                tsystemID.Invoke(new Action(() => tsystemID.Text = systemID));
            });
        }

        private async void bactivate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Activate Account", "Activate Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                load.Show(this);
                await Task.Delay(1000);
                try
                {

                    // Retrieve data from textboxes and comboboxes
                    string systemID = tsystemID.Text;
                    string name = tnamenew.Text;
                    string position = cbuserlevel.SelectedItem.ToString();
                    string email = temail.Text;
                    string username = tusernamenew.Text;
                    string password = tpassnew.Text;
                    string designation = cbstaffpos.SelectedItem.ToString();
                    Image photo = pbphoto.Image;

                    // Save user data to MongoDB
                    await SaveUserDataToMongoDB(systemID, name, position, email, username, password, designation, photo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error activating account: {ex.Message}");
                }
                // Close loading UI
                load.Close();

                // Confirmation message
                MessageBox.Show("Account activated successfully!");
            }
        }

        private void bupload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pbphoto.Image = Image.FromFile(filePath);
                }
            }
        }

        private void bdeluser_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Delete Account", "Delete Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    load.Show(this);
                    Thread.Sleep(1000);

                    string email = temail.Text;
                    Task.Run(async () => await DeleteUserFromSignupCollection(email)); // Delete user from signup collection
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting account: {ex.Message}");
                }
                finally
                {
                    load.Close(); 
                }
                MessageBox.Show("Account deleted successfully!");
                LoadUserData();
            }
        }
    }
    public class User
    {
        public ObjectId Id { get; set; }

        public string SystemID { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
        public byte[] Photo { get; set; }

        [BsonIgnore]
        public string IdString => Id.ToString();
    }
}