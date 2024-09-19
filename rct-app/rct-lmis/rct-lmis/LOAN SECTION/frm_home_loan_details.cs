using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
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

        private async void frm_home_loan_new_Load(object sender, EventArgs e)
        {
            laccno.Text = $"{AccountID}";
            await LoadLoanDetailsAsync();
        }

        private void InitializeDataGridView()
        {
            dgvuploads.Columns.Clear();

            // Add Document Name column and set its width
            var documentNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "DocumentName",
                HeaderText = "Document Name",
                Width = 400 // Set the desired width here
            };
            dgvuploads.Columns.Add(documentNameColumn);


            var viewFileButtonColumn = new DataGridViewButtonColumn
            {
                Name = "ViewFile",
                HeaderText = "View File",
                Text = "View File",
                UseColumnTextForButtonValue = true,
                Width = 100 // Adjust width to make the button smaller
            };
            dgvuploads.Columns.Add(viewFileButtonColumn);

            dgvuploads.Columns.Add("DocumentLink", "Document Link");
            dgvuploads.Columns["DocumentLink"].Visible = false;

            // Adjust the DataGridView button's padding
            foreach (DataGridViewColumn column in dgvuploads.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(2); // Smaller padding
            }

            
           
        }


        private void ConfigureDataGridView()
        {
            // Set the wrapping for the "Document Name" column
            dgvuploads.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Hide the "Document Link" column
            dgvuploads.Columns["DocumentLink"].Visible = false;

            // Optional: Adjust column width to fit the content
            dgvuploads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }


        private void LoadDocsIntoDataGridView(string[] docsArray, string[] docLinksArray)
        {
            dgvuploads.Rows.Clear();

            // Check if both document names and links arrays are valid
            if (docsArray == null || docLinksArray == null || docsArray.Length == 0 || docLinksArray.Length == 0)
            {
                MessageBox.Show("Document names or links are missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load each document name and link into the DataGridView
            for (int i = 0; i < docsArray.Length; i++)
            {
                string docName = docsArray[i];
                string docLink = (i < docLinksArray.Length) ? docLinksArray[i] : string.Empty;

                // Add both the document name and document link to the row
                int rowIndex = dgvuploads.Rows.Add();
                dgvuploads.Rows[rowIndex].Cells["DocumentName"].Value = docName;
                dgvuploads.Rows[rowIndex].Cells["DocumentLink"].Value = docLink; // Store the document link in the hidden column
                dgvuploads.Rows[rowIndex].Cells["ViewFile"].Value = "View File"; // Set the button text
            }

            // Configure the DataGridView after loading data
            ConfigureDataGridView();
        }




        private async Task LoadLoanDetailsAsync()
        {
            string accountId = laccno.Text;

            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
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

                    string loanStatus = document.GetValue("LoanStatus", "Not Available").ToString();
                    laccstatus.Text = loanStatus;

                    // Update lloanstatus based on LoanStatus value
                    if (loanStatus == "For Releasing Loan Disbursement")
                    {
                        lloanstatus.Text = "FOR DISBURSEMENT";
                    }
                    else if (loanStatus == "Loan Released")
                    {
                        lloanstatus.Text = "ACTIVE";
                    }
                    else
                    {
                        lloanstatus.Text = "UNKNOWN STATUS"; // Default case
                    }

                    // Populate additional fields
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")} {document.GetValue("SuffixName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Street", "")} , {document.GetValue("Barangay", "")} , {document.GetValue("City", "")} , {document.GetValue("Province", "")}".Trim();
                    trepcontact.Text = document.GetValue("CP", "").ToString(); // Example field, adjust if necessary

                    // Load docs into DataGridView
                    if (document.TryGetValue("docs", out BsonValue docsValue) && document.TryGetValue("doc-link", out BsonValue docLinksValue))
                    {
                        if (docsValue.IsBsonArray && docLinksValue.IsBsonArray)
                        {
                            var docsArray = docsValue.AsBsonArray.Select(d => d.AsString).ToArray();
                            var docLinksArray = docLinksValue.AsBsonArray.Select(l => l.AsString).ToArray();

                            LoadDocsIntoDataGridView(docsArray, docLinksArray);
                        }
                        else
                        {
                            MessageBox.Show("Document data is missing or incorrect.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Document data is missing or incorrect.");
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
            // Check if the clicked cell is in the "ViewFile" button column
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvuploads.Columns["ViewFile"].Index)
            {
                // Retrieve the corresponding document link from the hidden column
                var linkCell = dgvuploads.Rows[e.RowIndex].Cells["DocumentLink"];
                if (linkCell?.Value != null && !string.IsNullOrWhiteSpace(linkCell.Value.ToString()))
                {
                    string docLink = linkCell.Value.ToString();

                    try
                    {
                        // Open the document link in the default browser
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = docLink,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening the file: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("No document link found for this file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dgvuploads_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvuploads.ClearSelection();
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
    }
}
