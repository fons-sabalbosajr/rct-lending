using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.ACCOUNTING
{
    public partial class frm_home_accounting : Form
    {
        public frm_home_accounting()
        {
            InitializeComponent();

            LoadAccountGroups();
            InitializeLoansView();
            dtdateloan.Value = DateTime.Now;
        }

        LoadingFunction load = new LoadingFunction();

        private void LoadAccountGroups()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_account_titles");

                // Clear existing items in the ComboBox
                cbaccounttitle.Items.Clear();

                // Add default item
                cbaccounttitle.Items.Add("--all--");

                // Get distinct Account Groups
                var distinctGroups = collection.Distinct<string>("Account Group", Builders<BsonDocument>.Filter.Empty)
                                               .ToList();

                // Add distinct groups to the ComboBox
                foreach (var group in distinctGroups)
                {
                    cbaccounttitle.Items.Add(group);
                }

                // Optionally select the default item
                cbaccounttitle.SelectedIndex = 0; // Select "All" by default
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading account groups: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeLoansView()
        {
            // Clear existing columns if any
            dgvloans.Columns.Clear();

            // Add columns
            dgvloans.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Date" });
            dgvloans.Columns.Add(new DataGridViewTextBoxColumn { Name = "Reference", HeaderText = "Reference" });
            dgvloans.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccountTitle", HeaderText = "Account Title" });
            dgvloans.Columns.Add(new DataGridViewTextBoxColumn { Name = "Debit", HeaderText = "Debit" });
            dgvloans.Columns.Add(new DataGridViewTextBoxColumn { Name = "Credit", HeaderText = "Credit" });

        }
        
        private void PopulateLoansView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_account_data");

                // Fetch all loan account data
                var loans = collection.Find(Builders<BsonDocument>.Filter.Empty)
                                      .SortByDescending(l => l["Date"]) // Adjust sorting as needed
                                      .ToList();

                // Clear existing rows in the DataGridView
                dgvloans.Rows.Clear();

                // Populate DataGridView with loan account data
                foreach (var loan in loans)
                {
                    string accountTitle = loan.Contains("AccountTitle") ? loan["AccountTitle"].ToString() : "";
                    string debit = loan.Contains("Debit") ? loan["Debit"].ToString() : "0";
                    string credit = loan.Contains("Credit") ? loan["Credit"].ToString() : "0";
                    string reference = loan.Contains("Reference") ? loan["Reference"].ToString() : "";
                    string date = loan.Contains("Date") ? loan["Date"].ToLocalTime().ToString("g") : ""; // Format date

                    // Add row to the DataGridView
                    dgvloans.Rows.Add(date, reference, accountTitle, debit, credit);

                    lnorecord.Visible = dgvloans.Rows.Count == 0;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading loan data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterLoans()
        {
            string filterValue = tsearchloan.Text.ToLower(); // Get the filter text

            foreach (DataGridViewRow row in dgvloans.Rows)
            {
                // Check if the row contains the filter value in any of the relevant columns
                bool isVisible = row.Cells["AccountTitle"].Value.ToString().ToLower().Contains(filterValue) ||
                                 row.Cells["Debit"].Value.ToString().ToLower().Contains(filterValue) ||
                                 row.Cells["Credit"].Value.ToString().ToLower().Contains(filterValue) ||
                                 row.Cells["Reference"].Value.ToString().ToLower().Contains(filterValue) ||
                                 row.Cells["Date"].Value.ToString().ToLower().Contains(filterValue);

                // Show or hide the row based on whether it matches the filter
                row.Visible = isVisible;
            }
        }
        
        private void FilterLoansByDate()
        {
            try
            {
                var selectedDate = dtdateloan.Value.Date; // Get the selected date (ignore time)

                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_account_data");

                // Fetch documents matching the selected date
                var filter = Builders<BsonDocument>.Filter.Eq("Date", selectedDate);
                var filteredLoans = collection.Find(filter).ToList();

                // Clear existing rows in the DataGridView
                dgvloans.Rows.Clear();

                // Populate DataGridView with filtered loans
                foreach (var loan in filteredLoans)
                {
                    dgvloans.Rows.Add(loan["AccountTitle"].ToString(),
                                      loan["Debit"].ToString(),
                                      loan["Credit"].ToString(),
                                      loan["Reference"].ToString(),
                                      loan["Date"].ToUniversalTime().ToString("MM/dd/yyyy HH:mm")); // Format date if needed
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error filtering loans by date: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterAccountTitles()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_account_data");

                // Get the selected account group
                string selectedGroup = cbaccounttitle.SelectedItem?.ToString() ?? "";

                // Populate DataGridView
                dgvloans.Rows.Clear(); // Clear existing rows

                if (selectedGroup == "--all--")
                {
                    // Fetch all account titles without filtering
                    var allAccountTitles = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                    foreach (var title in allAccountTitles)
                    {
                        string accountTitle = title.Contains("AccountTitle") ? title["AccountTitle"].ToString() : "";
                        string debit = title.Contains("Debit") ? title["Debit"].ToString() : "0";
                        string credit = title.Contains("Credit") ? title["Credit"].ToString() : "0";
                        string reference = title.Contains("Reference") ? title["Reference"].ToString() : "";
                        string date = title.Contains("Date") ? title["Date"].ToLocalTime().ToString("g") : ""; // Format date

                        // Add the row to the DataGridView with all relevant data
                        dgvloans.Rows.Add(date, reference, accountTitle, debit, credit);
                    }
                }
                else
                {
                    // Build the filter based on the selected group
                    var filter = Builders<BsonDocument>.Filter.Empty; // Default filter

                    // Apply filters based on the selected account group
                    switch (selectedGroup)
                    {
                        case "Assets":
                            filter = Builders<BsonDocument>.Filter.Regex("AccountTitle", new BsonRegularExpression(@"^A.*"));
                            break;
                        case "Liability":
                            filter = Builders<BsonDocument>.Filter.Regex("AccountTitle", new BsonRegularExpression(@"^L.*"));
                            break;
                        case "Capital":
                            filter = Builders<BsonDocument>.Filter.Regex("AccountTitle", new BsonRegularExpression(@"^C.*"));
                            break;
                        case "Income":
                            filter = Builders<BsonDocument>.Filter.Regex("AccountTitle", new BsonRegularExpression(@"^I.*"));
                            break;
                        case "Expenses":
                            filter = Builders<BsonDocument>.Filter.Regex("AccountTitle", new BsonRegularExpression(@"^E.*"));
                            break;
                    }

                    // Fetch filtered account titles
                    var accountTitles = collection.Find(filter).ToList();

                    foreach (var title in accountTitles)
                    {
                        string accountTitle = title.Contains("AccountTitle") ? title["AccountTitle"].ToString() : "";
                        string debit = title.Contains("Debit") ? title["Debit"].ToString() : "0";
                        string credit = title.Contains("Credit") ? title["Credit"].ToString() : "0";
                        string reference = title.Contains("Reference") ? title["Reference"].ToString() : "";
                        string date = title.Contains("Date") ? title["Date"].ToLocalTime().ToString("g") : ""; // Format date

                        // Add the row to the DataGridView with all relevant data
                        dgvloans.Rows.Add(date, reference, accountTitle, debit, credit);
                    }
                }

                lnorecord.Visible = dgvloans.Rows.Count == 0;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error filtering account titles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void frm_home_accounting_Load(object sender, EventArgs e)
        {
            PopulateLoansView();
        }

        private void tsearchloan_TextChanged(object sender, EventArgs e)
        {
            FilterLoans();
        }

        private void dtdateloan_ValueChanged(object sender, EventArgs e)
        {
            FilterLoansByDate();
        }

        private void cbaccounttitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAccountTitles();
        }
    }
}
