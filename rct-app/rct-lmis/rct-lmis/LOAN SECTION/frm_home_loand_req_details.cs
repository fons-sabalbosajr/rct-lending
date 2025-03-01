using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loand_req_details : Form
    {
        private string accountId;
        LoadingFunction load = new LoadingFunction();

        public frm_home_loand_req_details(string accountId)
        {
            InitializeComponent();
            this.accountId = accountId;

            LoadLoanDetails();
        }


       

        private void LoadLoanDetails()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var document = loanAppCollection.Find(filter).FirstOrDefault();

                if (document != null)
                {
                    tlaccountno.Text = document.Contains("AccountId") ? document["AccountId"].ToString() : "N/A";
                    tfname.Text = document.Contains("ClientName") ? document["ClientName"].ToString() : "N/A";
                    taddress.Text = document.Contains("Address") ? document["Address"].ToString() : "N/A";

                    LoadUploadedDocuments(document);
                }
                else
                {
                    MessageBox.Show("Loan details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUploadedDocuments(BsonDocument document)
        {
            dgvuploads.Columns.Clear();
            dgvuploads.Columns.Add("DocumentName", "Document Name");
            dgvuploads.Columns.Add("DocumentLink", "Document Link");

            DataGridViewLinkColumn viewFileColumn = new DataGridViewLinkColumn
            {
                Name = "ViewFile",
                HeaderText = "View File",
                Text = "View File",
                UseColumnTextForLinkValue = true
            };
            dgvuploads.Columns.Add(viewFileColumn);

            dgvuploads.Rows.Clear();

            if (document.Contains("UploadedDocs") && document["UploadedDocs"].IsBsonArray)
            {
                var uploadedDocs = document["UploadedDocs"].AsBsonArray;
                foreach (var doc in uploadedDocs)
                {
                    if (doc.IsBsonDocument)
                    {
                        var docBson = doc.AsBsonDocument;
                        string fileName = docBson.Contains("file_name") ? docBson["file_name"].ToString() : "Unknown File";
                        string fileLink = docBson.Contains("file_link") ? docBson["file_link"].ToString() : "No Link Available";
                        dgvuploads.Rows.Add(fileName, fileLink, "View File");
                    }
                }
            }

            // Enable text wrapping in Document Link column
            dgvuploads.Columns["DocumentLink"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvuploads.Columns["DocumentName"].Width = 200; // Set width for Document Link column
            dgvuploads.Columns["DocumentLink"].Width = 400; // Set width for Document Link column
            dgvuploads.Columns["ViewFile"].Width = 100; // Set width for View File column

            dgvuploads.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }



        private void frm_home_loand_req_details_Load(object sender, EventArgs e)
        {
          
        }


        

        private void dgvuploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvuploads.Columns["ViewFile"].Index && e.RowIndex >= 0)
            {
                string fileLink = dgvuploads.Rows[e.RowIndex].Cells["DocumentLink"].Value?.ToString();

                if (string.IsNullOrEmpty(fileLink))
                {
                    MessageBox.Show("File link not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    Process.Start(new ProcessStartInfo { FileName = fileLink, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open file link.\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bapproved_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(2000);
            load.Close();
            MessageBox.Show(this, "Loan application approved and details updated.");
        }

        private void bdeny_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(2000);
            load.Close();
            MessageBox.Show(this, "Loan application denied. \n The data has been stored in the Denied List");
        }
    }
}
