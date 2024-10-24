using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_ADMIN_annoucement : Form
    {
       
        public frm_ADMIN_annoucement()
        {
            InitializeComponent(); 
            
        }

        LoadingFunction load = new LoadingFunction();

        private void frm_ADMIN_annoucement_Load(object sender, EventArgs e)
        {

        }

        private async void bpostannoucement_Click(object sender, EventArgs e)
        {
            string title = tTitle.Text;
            string content = tContent.Text;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Both title and content are required.");
                return;
            }
            // Save the announcement data asynchronously
            await SaveAnnouncementData(title, content);

            MessageBox.Show(this, "Announcement posted successfully.");

            // Clear the input fields
            tTitle.Clear();
            tContent.Clear();

            this.Close();
        }


        private async Task SaveAnnouncementData(string title, string content)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<Announcement>("announcements");

            var announcement = new Announcement
            {
                Title = title,
                Content = content,
                PostedDate = DateTime.Now
            };

            await collection.InsertOneAsync(announcement);
        }

        private void bcancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    public class AnnouncementEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostedDate { get; set; }
    }

}
