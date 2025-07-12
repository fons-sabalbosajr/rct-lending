using MongoDB.Bson;
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

namespace rct_lmis
{
    public partial class frm_home_notification : Form
    {
        private IMongoCollection<BsonDocument> _loanCollection;
        public frm_home_notification()
        {
            InitializeComponent();
        }

        private void frm_home_notification_Load(object sender, EventArgs e)
        {
            LoadAnnouncements(); // Load and format data
            StyleDGV();          // Apply visual formatting
        }

        private void LoadAnnouncements()
        {
            var collection = MongoDBConnection.Instance.Database.GetCollection<Announcement>("announcements");

            var announcements = collection
                .Find(Builders<Announcement>.Filter.Empty)
                .SortByDescending(a => a.PostedDate)
                .ToList();

            DataTable table = new DataTable();
            table.Columns.Add("Announcement", typeof(string));

            foreach (var item in announcements)
            {
                string combinedText =
                    $"{item.Title}\n\n" +
                    $"{item.Content}\n\n" +
                    $"Posted On: {item.PostedDate.ToString("MMMM dd, yyyy hh:mm tt")}\n\n";

                table.Rows.Add(combinedText);
            }

            dgvbulletin.DataSource = table;
        }

        private void StyleDGV()
        {
            dgvbulletin.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvbulletin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvbulletin.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvbulletin.RowHeadersVisible = false;
            dgvbulletin.ColumnHeadersVisible = false;

            dgvbulletin.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvbulletin.DefaultCellStyle.ForeColor = Color.Black;
            dgvbulletin.DefaultCellStyle.BackColor = Color.White;
            dgvbulletin.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
            dgvbulletin.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

    }
}

public class Announcement
{
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PostedDate { get; set; }
}

