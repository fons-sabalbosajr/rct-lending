using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.WinForms.Helpers.GraphicsHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_useraccounts : Form
    {
        public frm_home_ADMIN_useraccounts()
        {
            InitializeComponent();
        }
        LoadingFunction load = new LoadingFunction();

        private async void LoadUserData()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<User>("user_accounts");

            var users = await collection.Find(_ => true).ToListAsync();

            if (users.Count == 0)
            {
                lnodata.Visible = true; // Show the label when there are no users
                dgvdata.DataSource = null; // Clear the DataGridView
            }
            else
            {
                lnodata.Visible = false; // Hide the label when there are users

                dgvdata.ClearSelection();

                var userViewModels = users.Select(u => new
                {
                    u.SystemID,
                    u.FullName,
                    u.Position,
                    u.Email,
                    //u.Username,
                    //u.Password,
                    //Photo = LoadPhoto(u.Photo) // Load photo as Image
                }).ToList();

                // Assigning the data source to the DataGridView
                dgvdata.DataSource = userViewModels;
            }
        }

        // Method to load photo as Image
        private Image LoadPhoto(byte[] photoData)
        {
            if (photoData == null || photoData.Length == 0)
            {
                return null; // Return null if photo data is empty
            }
            else
            {
                // Convert byte array to Image
                using (MemoryStream ms = new MemoryStream(photoData))
                {
                    return Image.FromStream(ms);
                }
            }
        }

        private void LoadDataAndPhotoBySystemID(string systemID)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("SystemID", systemID);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Get user details
                string fullName = user.GetValue("FullName").AsString;
                string position = user.GetValue("Position").AsString;
                string email = user.GetValue("Email").AsString;
                string username = user.GetValue("Username").AsString;
                string password = user.GetValue("Password").AsString;
                string designation = user.GetValue("Designation").AsString;


                // Set user details to text boxes
                tsystemId.Text = systemID;
                tname.Text = fullName;
                tpos.Text = position;
                temail.Text = email;
                tusername.Text = username;
                tpass.Text = password;
                tsystempos.Text = designation;

                // Load the photo
                if (user.Contains("Photo"))
                {
                    var photoData = user.GetValue("Photo").AsBsonBinaryData; // Assuming "Photo" is the field name storing the binary data of the photo

                    // Load the photo from binary data
                    using (var ms = new MemoryStream(photoData.Bytes))
                    {
                        pbphoto.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    // Clear the photo if not available
                    pbphoto.Image = null;
                }
            }
            else
            {
                // Clear all text boxes and picture box if user not found
                ClearTextBoxes();
                pbphoto.Image = null;
            }
        }

        private async Task UpdateUserData(string systemID, string fullName, string position, string email, string username, string password, string designation, Image photo)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

                var filter = Builders<BsonDocument>.Filter.Eq("SystemID", systemID);

                // Create update definition
                var update = Builders<BsonDocument>.Update
                    .Set("FullName", fullName)
                    .Set("Position", position)
                    .Set("Email", email)
                    .Set("Username", username)
                    .Set("Password", password)
                    .Set("Designation", designation);

                // Check if photo is provided and update if necessary
                if (photo != null)
                {
                    byte[] photoBytes = ImageToByteArray(photo);
                    update = update.Set("Photo", photoBytes);
                }

                load.Show(this);
                Thread.Sleep(1000);
                await collection.UpdateOneAsync(filter, update);
                load.Close();

                MessageBox.Show(this, "Data updated successfully!");
                disable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user data: {ex.Message}");
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


        private void ClearTextBoxes()
        {
            // Clear all text boxes
            tsystemId.Clear();
            tname.Clear();
            tpos.Clear();
            temail.Clear();
            tusername.Clear();
            tsystempos.Clear();
            tpass.Clear();
        }

        private void disable()
        {
            tsystemId.Enabled = false;
            tname.Enabled = false;
            tpos.Enabled = false;
            temail.Enabled = false;
            tusername.Enabled = false;
            tsystempos.Enabled = false;
            tpass.Enabled = false;

            bupload.Visible = false;
        }

        private void enable()
        {
            //tsystemId.Enabled = true;
            tname.Enabled = true;
            tpos.Enabled = true;
            temail.Enabled = true;
            tusername.Enabled = true;
            tsystempos.Enabled = true;
            tpass.Enabled = true;
            bupload.Visible = true;
        }


        private void frm_home_ADMIN_useraccounts_Load(object sender, EventArgs e)
        {
            LoadUserData();
            disable();
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Check if a valid row is clicked
            {
                // Get the SystemID value of the clicked row
                string systemID = dgvdata.Rows[e.RowIndex].Cells[0].Value.ToString(); // Assuming SystemID is in the first column (index 0)

                // Load data and photo based on the SystemID value
                LoadDataAndPhotoBySystemID(systemID);
            }
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void bedit_Click(object sender, EventArgs e)
        {
            enable();
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

        private async void bupdate_Click(object sender, EventArgs e)
        {

            string systemID = tsystemId.Text;
            string fullName = tname.Text;
            string position = tpos.Text;
            string email = temail.Text;
            string username = tusername.Text;
            string password = tpass.Text;
            string designation = tsystempos.Text;
            Image photo = pbphoto.Image;

            // Call the method to update user data with loading
            await UpdateUserData(systemID, fullName, position, email, username, password, designation, photo);
        }
    }
}
