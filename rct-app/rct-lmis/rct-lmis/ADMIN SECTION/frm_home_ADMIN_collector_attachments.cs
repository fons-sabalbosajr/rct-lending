using Microsoft.Identity.Client;
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

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_collector_attachments : Form
    {

        public static string StoredAccountID { get; set; }

        public frm_home_ADMIN_collector_attachments()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dgvuploads.Columns.Clear();
            dgvuploads.Columns.Add("DocumentName", "Document Name");
            dgvuploads.Columns.Add("DocumentLink", "Document Link");
            dgvuploads.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "ViewFile",
                HeaderText = "View File",
                Text = "View File",
                UseColumnTextForLinkValue = true
            });
        }

        private void ConfigureDataGridView()
        {
            // Set the wrapping for the second column
            dgvuploads.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvuploads.Columns[2].Width = 200;

            // Optional: Adjust column width to fit the content
            dgvuploads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void LoadDocsIntoDataGridView(string docs, string docLinks)
        {
            dgvuploads.Rows.Clear();

            // Split the docs and docLinks by comma, and trim any leading or trailing spaces
            var docsArray = docs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(doc => doc.Trim())
                                .ToArray();

            var docLinksArray = docLinks.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(link => link.Trim())
                                        .ToArray();

            for (int i = 0; i < docsArray.Length; i++)
            {
                // Ensure docLinksArray has an element at index i, or default to an empty string
                int rowIndex = dgvuploads.Rows.Add(docsArray[i], docLinksArray.Length > i ? docLinksArray[i] : string.Empty, "View File");

                var link = new DataGridViewLinkCell
                {
                    Value = "View File",
                    UseColumnTextForLinkValue = true
                };

                dgvuploads.Rows[rowIndex].Cells[2] = link;
            }

            // Configure the DataGridView after loading data
            ConfigureDataGridView();
        }

      
        private async void frm_home_ADMIN_collector_attachments_Load(object sender, EventArgs e)
        {
            laccountid.Text = StoredAccountID;
            await LoadLoanDetailsAsync();
        }

        private async Task LoadLoanDetailsAsync()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_collectors");

                // Query to find the document with the specified AccountID
                var filter = Builders<BsonDocument>.Filter.Eq("GeneratedIDNumber", laccountid.Text);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    // Load docs into DataGridView
                    if (document.TryGetValue("docs", out var docs) && docs.IsString && document.TryGetValue("doc-link", out var docLinks) && docLinks.IsString)
                    {
                        LoadDocsIntoDataGridView(docs.AsString, docLinks.AsString);
                    }

                }
                else
                {
                    MessageBox.Show("No loan details found for the specified Account ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan details: {ex.Message}");
            }
        }

        private void dgvuploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2) // Assuming the "View File" link is in the third column (index 2)
            {
                var docLink = dgvuploads.Rows[e.RowIndex].Cells[1].Value.ToString();
                try
                {
                    // Use Process.Start to open the URL in the default web browser
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = docLink,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening the file: " + ex.Message);
                }
            }
        }

        private void dgvuploads_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvuploads.ClearSelection();
        }
    }
}
