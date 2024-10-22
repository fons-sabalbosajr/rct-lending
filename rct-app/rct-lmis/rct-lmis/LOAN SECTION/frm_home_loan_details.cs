using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
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

        private DataTable CreateLoanDataTable()
        {
            DataTable table = new DataTable();

            // Define columns
            table.Columns.Add("Loan ID", typeof(string));
            table.Columns.Add("Loan Details", typeof(string));
            table.Columns.Add("Amortization", typeof(string));
            table.Columns.Add("Repayment", typeof(string));

            return table;
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
            lnorecorddis.Visible = false;
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    // Client Info
                    taccname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim(); // Complete address
                    taccaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim(); // Complete address
                    taccbrgy.Text = document.GetValue("Barangay", "").ToString();
                    tacctown.Text = document.GetValue("City", "").ToString();
                    taccprov.Text = document.GetValue("Province", "").ToString();
                    tacccontactno.Text = document.GetValue("ContactNumber", "").ToString();
                    taccemail.Text = document.GetValue("Email", "").ToString();

                    // Loan Info
                    string loanStatus = document.GetValue("LoanProcessStatus", "Not Available").ToString();
                    laccstatus.Text = loanStatus;

                    // Update lloanstatus based on LoanProcessStatus value
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

                    // Additional Info
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim();
                    trepcontact.Text = document.GetValue("ContactNumber", "").ToString();

                    // Loan details
                    laccno.Text = document.GetValue("LoanNo", "").ToString();
                    trepcurrloan.Text = document.GetValue("LoanAmount", "").ToString();
                    treploanbalance.Text = document.GetValue("LoanBalance", "").ToString();
                    treploanpenalty.Text = document.GetValue("Penalty", "").ToString();
                    trepcollector.Text = document.GetValue("CollectorName", "").ToString();

                    // Loan Dates
                    treprepaydate.Text = document.GetValue("StartPaymentDate", "").ToString();
                  
                    // Loading document info into DataGridView (if applicable)
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
                        //MessageBox.Show("Document data is missing or incorrect.");
                    }

                    // Count the number of rows (documents) for the AccountId and set it in treploantotal.Text
                    long loanCount = await collection.CountDocumentsAsync(filter);
                    treploantotal.Text = loanCount.ToString();

                    // Step 1: Add columns first
                    CreateLoanDataTable();

                 
                    // Step 2: Load loan details into DataGridView
                    await LoadLoanDetailsToDataGridViewAsync(accountId);

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



        private async Task LoadLoanDetailsToDataGridViewAsync(string accountId)
        {
            try
            {
                // Step 1: Setup DataTable and bind it to DataGridView
                DataTable loanDataTable = CreateLoanDataTable();
                dgvdataamort.DataSource = loanDataTable;

                // Access the MongoDB database and fetch loan details
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var documents = await collection.Find(filter).ToListAsync();

                if (documents.Count > 0)
                {
                    // Loop through all documents and add rows to the DataTable
                    foreach (var document in documents)
                    {
                        DataRow row = loanDataTable.NewRow();

                        // Populate row with MongoDB data
                        row["Loan ID"] = document.GetValue("LoanNo", "").ToString();

                        // Loan details: LoanAmount, LoanBalance, LoanTerm, LoanInterest
                        row["Loan Details"] = $"Amount: {document.GetValue("LoanAmount", "0.00")}\n" +
                                              $"Balance: {document.GetValue("LoanBalance", "0.00")}\n" +
                                              $"Term: {document.GetValue("LoanTerm", "0")}\n" +
                                              $"Interest: {document.GetValue("LoanInterest", "0%")}";

                        // Amortization: LoanAmortization, MissedDays, Penalty
                        string missedDays = "0"; // Placeholder, calculate missed days if necessary
                        row["Amortization"] = $"Amortization: {document.GetValue("LoanAmortization", "0.00")}\n" +
                                              $"Missed: {missedDays} days\n" +
                                              $"Penalty: {document.GetValue("Penalty", "0.00")}";

                        // Repayment: PaymentMode, StartPaymentDate, MaturityDate
                        row["Repayment"] = $"Mode: {document.GetValue("PaymentMode", "")}\n" +
                                           $"Start: {document.GetValue("StartPaymentDate", "")}\n" +
                                           $"Maturity: {document.GetValue("MaturityDate", "")}";

                        // Add the row to the DataTable
                        loanDataTable.Rows.Add(row);
                    }

                    // Update the total count of loans in treploantotal.Text
                    treploantotal.Text = documents.Count.ToString();
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

        private void bgemamt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgenSOA_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgenledger_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgenremind_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgendemandinit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgendemandfinal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvdataamort_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdataamort.ClearSelection();
        }
    }
}
