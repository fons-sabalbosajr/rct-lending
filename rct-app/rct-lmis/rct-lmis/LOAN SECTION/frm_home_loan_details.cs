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

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_new : Form
    {
        public string AccountID { get; set; }

        public frm_home_loan_new()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        LoadingFunction load =new LoadingFunction();

        private async void frm_home_loan_new_Load(object sender, EventArgs e)
        {
            // Display the AccountID on the label
            laccno.Text = $"{AccountID}";

            // Load data based on AccountID
            await LoadLoanDetailsAsync();

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



        private async Task LoadLoanDetailsAsync()
        {
            // Retrieve the AccountId from the label
            string accountId = laccno.Text;

            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                // Query to find the document with the specified AccountID
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId" , AccountID);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    // Populate the text boxes with the data from the document
                    taccname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")} {document.GetValue("SuffixName", "")}".Trim();
                    taccaddress.Text = document.GetValue("Street", "").ToString();
                    taccbrgy.Text = document.GetValue("Barangay", "").ToString();
                    tacctown.Text = document.GetValue("City", "").ToString();
                    taccprov.Text = document.GetValue("Province", "").ToString();
                    
                    if (document.TryGetValue("RBLate", out BsonValue rbLateValue) && rbLateValue.IsBsonDateTime)
                    {
                        DateTime rbLateDate = rbLateValue.ToUniversalTime(); 
                        taddbirth.Text = rbLateDate.ToString("MM/dd/yyyy"); 
                    }
                    else
                    {
                        taddbirth.Text = string.Empty;
                    }
                    tacccontactno.Text = document.GetValue("CP", "").ToString();
                    taccemail.Text = document.GetValue("Email", "").ToString(); 

                    // Populate the loan status on the label
                    laccstatus.Text = document.GetValue("LoanStatus", "Not Available").ToString();
                    lloanstatus.Text = "FOR DISBURSEMENT";

                    // Populate additional fields
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")} {document.GetValue("SuffixName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Street", "")} , {document.GetValue("Barangay", "")} , {document.GetValue("City", "")} , {document.GetValue("Province", "")}".Trim();
                    trepcontact.Text = document.GetValue("CP", "").ToString(); // Example field, adjust if necessary
                    //trepidcard.Text = document.GetValue("RepIDCard", "").ToString(); // Example field, adjust if necessary
                    //trepcurrloan.Text = document.GetValue("RepCurrentLoan", "").ToString(); // Example field, adjust if necessary
                    //treploanbalance.Text = document.GetValue("RepLoanBalance", "").ToString(); // Example field, adjust if necessary
                    //treploanpenalty.Text = document.GetValue("RepLoanPenalty", "").ToString(); // Example field, adjust if necessary
                    //trepcollector.Text = document.GetValue("RepCollector", "").ToString(); // Example field, adjust if necessary
                    //treploantotal.Text = document.GetValue("RepLoanTotal", "").ToString(); // Example field, adjust if necessary
                    //treprepaydate.Text = document.GetValue("RepRepayDate", "").ToString(); // Example field, adjust if necessary

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

        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            // Get the text from the Label control
            string accNo = laccno.Text;

            // Copy the text to the clipboard
            Clipboard.SetText(accNo);

            // Show a message box to notify the user
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
