using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.ADMIN_SECTION;
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
    public partial class frm_home_loan_disburse : Form
    {
        public string AccountID { get; set; }


        private IMongoCollection<BsonDocument> collection;
        private readonly IMongoCollection<BsonDocument> loanRateCollection;
        private readonly IMongoCollection<BsonDocument> disbursedcollection;

        private DataTable dataTable = new DataTable();

        public frm_home_loan_disburse()
        {
            InitializeComponent();

            var database = MongoDBConnection.Instance.Database;
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");
            
            gpocash.Enabled = false;
            gpoonline.Enabled = false;
            gpobank.Enabled = false;

        }

        private void frm_home_loan_disburse_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
            tloanaccno.Text = AccountID;
        }

        private string GenerateIncrementId()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest current ID
                var sort = Builders<BsonDocument>.Sort.Descending("IncrementId");
                var lastRecord = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("IncrementId"))
                {
                    string lastId = lastRecord["IncrementId"].AsString;
                    int lastIdNumber = int.Parse(lastId.Substring(lastId.LastIndexOf('-') + 1));
                    nextIdNumber = lastIdNumber + 1;
                }

                // Generate new increment ID
                string newIncrementId = $"RCT-DB-{nextIdNumber:D5}";
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-DB-00001"; // Fallback ID
            }
        }

        private void FillUpCashGroupBox()
        {
            try
            {
                // Generate increment ID
                tcashno.Text = GenerateIncrementId();

                // Set current date
                tcashpodate.Text = DateTime.Now.ToString("MM/dd/yyyy, dddd hh:mm: tt");

                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tcashclnno.Text = loanRecord.Contains("ClientNumber") ? loanRecord["ClientNumber"].AsString : string.Empty;
                    tcashname.Text = loanRecord.Contains("FirstName") && loanRecord.Contains("LastName")
                        ? $"{loanRecord["FirstName"].AsString} {loanRecord["LastName"].AsString}"
                        : string.Empty;

                    // Get processing fee from trfservicefee.Text
                    double processingFee;
                    if (!double.TryParse(trfservicefee.Text.Replace("₱", "").Replace(",", "").Trim(), out processingFee))
                    {
                        MessageBox.Show("Invalid processing fee value.");
                        return;
                    }
                    tcashprofee.Text = "₱ " + processingFee.ToString("N2");

                    // Calculate the cash amount
                    double principal;
                    if (!double.TryParse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim(), out principal))
                    {
                        MessageBox.Show("Invalid principal amount value.");
                        return;
                    }
                    tcashamt.Text = "₱ " + principal.ToString("N2");
                    tcashpoamt.Text = "₱ " + (principal - processingFee).ToString("N2");
                }
                else
                {
                    MessageBox.Show("Loan record not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling up Cash GroupBox: " + ex.Message);
            }
        }

        private string GenerateIncrementIdOnline()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest current ID with the "OLP" prefix
                var sort = Builders<BsonDocument>.Sort.Descending("IncrementId");
                var filter = Builders<BsonDocument>.Filter.Regex("IncrementId", new BsonRegularExpression("^RCT-DB-OLP-"));
                var lastRecord = collection.Find(filter).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("IncrementId"))
                {
                    string lastId = lastRecord["IncrementId"].AsString;
                    int lastIdNumber = int.Parse(lastId.Substring(lastId.LastIndexOf('-') + 1));
                    nextIdNumber = lastIdNumber + 1;
                }

                // Generate new increment ID
                string newIncrementId = $"RCT-DB-OLP-{nextIdNumber:D5}";
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-DB-OLP-00001"; // Fallback ID
            }
        }


        private void FillUpOnlineGroupBoxOnline()
        {
            try
            {
                // Set cashout platform
                cbonlineplatform.Text = "select platform";

                // Set current date and time
                tonlinepodate.Text = DateTime.Now.ToString("MM/dd/yyyy, dddd hh:mm: tt");

                // Generate increment ID
                tponlinerefno.Text = GenerateIncrementIdOnline();

                // Fetch client number and name based on AccountID
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tponlineaccno.Text = loanRecord.Contains("ClientNumber") ? loanRecord["ClientNumber"].AsString : string.Empty;
                    tponlinename.Text = loanRecord.Contains("FirstName") && loanRecord.Contains("LastName")
                        ? $"{loanRecord["FirstName"].AsString} {loanRecord["LastName"].AsString}"
                        : string.Empty;

                    // Get processing fee from trfservicefee.Text
                    double processingFee;
                    if (!double.TryParse(trfservicefee.Text.Replace("₱", "").Replace(",", "").Trim(), out processingFee))
                    {
                        MessageBox.Show("Invalid processing fee value.");
                        return;
                    }

                    // Calculate the cash amount
                    double principal = double.Parse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim());
                    tponlineamt.Text = "₱ " + principal.ToString("N2");
                    tonlineprofee.Text = "₱ " + processingFee.ToString("N2");
                    tonlinepoamt.Text = "₱ " + (principal - processingFee).ToString("N2");
                }
                else
                {
                    MessageBox.Show("Loan record not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling up Online GroupBox: " + ex.Message);
            }
        }

        private string GenerateIncrementIdBank()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest current ID
                var sort = Builders<BsonDocument>.Sort.Descending("IncrementId");
                var lastRecord = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("IncrementId"))
                {
                    string lastId = lastRecord["IncrementId"].AsString;
                    int lastIdNumber = int.Parse(lastId.Substring(lastId.LastIndexOf('-') + 1));
                    nextIdNumber = lastIdNumber + 1;
                }

                // Generate new increment ID
                string newIncrementId = $"RCT-DB-BNK-{nextIdNumber:D5}";
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-DB-BNK-00001"; // Fallback ID
            }
        }

        private void FillUpBankGroupBoxBank()
        {
            try
            {
                // Set bank type platform
                cbbankplatform.Text = "Specify the bank type";

                // Set current date
                tbankpodate.Text = DateTime.Now.ToString("MM/dd/yyyy, dddd");

                // Generate increment ID
                tbankporefno.Text = GenerateIncrementIdBank();

                // Fetch client number and name based on AccountID
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tbankpoaccno.Text = loanRecord.Contains("ClientNumber") ? loanRecord["ClientNumber"].AsString : string.Empty;
                    tbankname.Text = loanRecord.Contains("FirstName") && loanRecord.Contains("LastName")
                        ? $"{loanRecord["FirstName"].AsString} {loanRecord["LastName"].AsString}"
                        : string.Empty;

                    // Get processing fee from trfservicefee.Text
                    double processingFee;
                    if (!double.TryParse(trfservicefee.Text.Replace("₱", "").Replace(",", "").Trim(), out processingFee))
                    {
                        MessageBox.Show("Invalid processing fee value.");
                        return;
                    }

                    // Calculate the cash amount
                    double principal = double.Parse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim());
                    tbankpoamt.Text = "₱ " + principal.ToString("N2");
                    tbankpoprofee.Text = "₱ " + processingFee.ToString("N2");
                    tbankamt.Text = "₱ " + (principal - processingFee).ToString("N2");
                }
                else
                {
                    MessageBox.Show("Loan record not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling up Bank GroupBox: " + ex.Message);
            }
        }




        private void LoadDataToDataGridView()
        {
            dataTable.Clear();

            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = loanRateCollection.Find(filter).ToList();

            string[] displayColumns = { "Term", "Principal", "Type", "Mode", "Interest Rate/Month", "Processing" };

            foreach (string column in displayColumns)
            {
                if (!dataTable.Columns.Contains(column))
                    dataTable.Columns.Add(column);
            }
            dataTable.Columns.Add("FullDocument", typeof(BsonDocument));

            foreach (var doc in documents)
            {
                DataRow row = dataTable.NewRow();
                foreach (string column in displayColumns)
                {
                    if (doc.Contains(column))
                    {
                        var element = doc[column];
                        if (element.IsNumeric())
                        {
                            if (column == "Principal")
                                row[column] = "₱ " + Math.Round(element.ToDouble(), 0).ToString();
                            else
                                row[column] = Math.Round(element.ToDouble(), 0);
                        }
                        else
                        {
                            row[column] = element.ToString();
                        }
                    }
                }
                row["FullDocument"] = doc;
                dataTable.Rows.Add(row);
            }

            dgvloandata.DataSource = dataTable;
            if (dgvloandata.Columns.Count > 0)
                dgvloandata.Columns["FullDocument"].Visible = false;

            foreach (DataGridViewColumn column in dgvloandata.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (column.Name != "Term" && (column.Name == "Principal" || column.Name == "Interest Rate/Month" || column.Name == "Processing"))
                    column.DefaultCellStyle.Format = "N2";
            }
        }


        private void ComputeAmortization()
        {
            try
            {
                // Check if tloanterm.Text is empty, return without doing anything
                if (string.IsNullOrWhiteSpace(tloanterm.Text))
                {
                    return;
                }

                // Parse and validate loan amount
                string loanAmtText = tloanamt.Text.Replace("₱", "").Replace(",", "").Trim();
                if (!double.TryParse(loanAmtText, out double principal))
                {
                    MessageBox.Show("Please enter a valid loan amount.");
                    return;
                }

                // Parse and validate loan term
                if (!int.TryParse(tloanterm.Text.Trim(), out int term))
                {
                    MessageBox.Show("Please enter a valid loan term (in months).");
                    return;
                }

                // Fixed interest rate per month (as percentage)
                double interestRate = 0.05; // 5%

                // Calculate total interest amount for the entire term
                double totalInterest = principal * interestRate * term;

                // Calculate the total number of weekdays (excluding weekends)
                int totalDays = 5 * 4 * term;
                tdays.Text = totalDays.ToString();

                // Calculate amortized amount considering the total interest over the term
                double amortizedAmount = (principal + totalInterest) / totalDays;

                // Display the results
                tamortizedamt.Text = amortizedAmount.ToString("N2");
                tloaninterestamt.Text = totalInterest.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error computing amortization: " + ex.Message);
            }
        }

        private void tsearchamt_TextChanged(object sender, EventArgs e)
        {
            string searchText = tsearchamt.Text.Trim();

            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    string filterExpression = string.Format(
                        "Term LIKE '%{0}%' OR Principal LIKE '%{0}%' OR [Interest Rate/Month] LIKE '%{0}%' OR Processing LIKE '%{0}%' OR Type LIKE '%{0}%' OR Mode LIKE '%{0}%'",
                        searchText.Replace("'", "''")); // Replace single quotes to avoid SQL-like errors

                    DataView dv = new DataView(dataTable);
                    dv.RowFilter = filterExpression;
                    dgvloandata.DataSource = dv;
                }
                else
                {
                    dgvloandata.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying search filter: " + ex.Message);
            }
        }


        private void dgvloandata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row index is selected
            {
                DataGridViewRow selectedRow = dgvloandata.Rows[e.RowIndex];
                if (selectedRow != null && selectedRow.Cells["FullDocument"].Value != null)
                {
                    BsonDocument fullDocument = selectedRow.Cells["FullDocument"].Value as BsonDocument;

                    if (fullDocument != null)
                    {
                        try
                        {
                            tloanamt.Text = fullDocument.Contains("Principal") ? "₱ " + fullDocument["Principal"].ToString() + ".00" : string.Empty;
                            tloanterm.Text = fullDocument.Contains("Term") ? fullDocument["Term"].ToString() : string.Empty;
                            tloaninterest.Text = fullDocument.Contains("Interest Rate/Month") ? fullDocument["Interest Rate/Month"].ToString() + "%" : string.Empty;
                            trfservicefee.Text = fullDocument.Contains("Processing Fee") ? fullDocument["Processing Fee"].ToString() + ".00" : string.Empty;

                            trfnotarialfee.Text = fullDocument.Contains("Notarial Rate") ? fullDocument["Notarial Rate"].ToString() + ".00" : string.Empty;
                            trfnotarialamt.Text = trfnotarialfee.Text;

                            trfinsurancefee.Text = fullDocument.Contains("Insurance Rate") ? fullDocument["Insurance Rate"].ToString() + ".00" : string.Empty;
                            trfinsuranceamt.Text = trfinsurancefee.Text;

                            trfannotationfee.Text = fullDocument.Contains("Annotation Rate") ? fullDocument["Annotation Rate"].ToString() + ".00" : string.Empty;
                            trfannotationmt.Text = trfannotationfee.Text;

                            trfvat.Text = fullDocument.Contains("Vat Rate") ? fullDocument["Vat Rate"].ToString() + ".00" : string.Empty;
                            trfvatamt.Text = trfvat.Text;

                            trfmisc.Text = fullDocument.Contains("Misc. Rate") ? fullDocument["Misc. Rate"].ToString() + ".00" : string.Empty;
                            trfmiscamt.Text = trfmisc.Text;

                            trfdocfee.Text = fullDocument.Contains("Doc Rate") ? fullDocument["Doc Rate"].ToString() + ".00" : string.Empty;
                            trfdocamt.Text = trfdocfee.Text;

                            // Compute amortization whenever the loan data changes
                            ComputeAmortization();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing fullDocument: " + ex.Message);
                        }
                    }
                    else
                    {
                        // Debug output
                        Console.WriteLine("FullDocument is null");
                    }
                }
                else
                {
                    // Debug output
                    Console.WriteLine("Selected row or FullDocument cell is null");
                }
            }
        }

        private void tloanamt_TextChanged(object sender, EventArgs e)
        {
            ComputeAmortization();
        }

        private void tloanterm_TextChanged_1(object sender, EventArgs e)
        {
            ComputeAmortization();
        }

        private void tloanterm_TextChanged(object sender, EventArgs e)
        {
            ComputeAmortization();
        }

        private void cbpocash_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpocash.Checked)
            {
                if (cbpoonline.Checked || cbpobank.Checked)
                {
                    MessageBox.Show("Only one option can be selected at a time. The other options will be unchecked.", 
                        "Selection Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Uncheck other checkboxes
                cbpoonline.Checked = false;
                cbpobank.Checked = false;

                // Enable the cash group box
                gpocash.Enabled = true;

                // Fill up the GroupBox when checked
                FillUpCashGroupBox();
            }
            else
            {
                // Clear the textboxes when unchecked
                tcashno.Text = string.Empty;
                tcashprofee.Text = string.Empty;
                tcashamt.Text = string.Empty;
                tcashpoamt.Text = string.Empty;
                tcashpodate.Text = string.Empty;
                tcashclnno.Text = string.Empty;
                tcashname.Text = string.Empty;

                // Disable the cash group box
                gpocash.Enabled = false;
            }
        }


        private void cbpoonline_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpoonline.Checked)
            {
                if (cbpocash.Checked || cbpobank.Checked)
                {
                    MessageBox.Show("Only one option can be selected at a time. The other options will be unchecked.",
                        "Selection Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Uncheck other checkboxes
                cbpocash.Checked = false;
                cbpobank.Checked = false;

                // Enable the online group box
                gpoonline.Enabled = true;

                // Fill up the Online GroupBox when checked
                FillUpOnlineGroupBoxOnline();
            }
            else
            {
                // Clear all text boxes in the online group box
                cbonlineplatform.Text = string.Empty;
                tonlinepodate.Text = string.Empty;
                tponlinerefno.Text = string.Empty;
                tponlineaccno.Text = string.Empty;
                tponlinename.Text = string.Empty;
                tponlineamt.Text = string.Empty;
                tonlineprofee.Text = string.Empty;
                tonlinepoamt.Text = string.Empty;

                // Disable the online group box
                gpoonline.Enabled = false;
            }
        }

        private void cbpobank_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpobank.Checked)
            {
                if (cbpocash.Checked || cbpoonline.Checked)
                {
                    MessageBox.Show("Only one option can be selected at a time. The other options will be unchecked.",
                        "Selection Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Uncheck other checkboxes
                cbpocash.Checked = false;
                cbpoonline.Checked = false;

                // Enable the bank group box
                gpobank.Enabled = true;

                // Fill up the Bank GroupBox when checked
                FillUpBankGroupBoxBank();
            }
            else
            {
                // Clear Bank GroupBox textboxes if unchecked
                cbbankplatform.Text = string.Empty;
                tbankpodate.Text = string.Empty;
                tbankporefno.Text = string.Empty;
                tbankpoaccno.Text = string.Empty;
                tbankname.Text = string.Empty;
                tbankpoamt.Text = string.Empty;
                tbankpoprofee.Text = string.Empty;
                tbankamt.Text = string.Empty;

                // Disable the bank group box
                gpobank.Enabled = false;
            }
        }
    }
}
