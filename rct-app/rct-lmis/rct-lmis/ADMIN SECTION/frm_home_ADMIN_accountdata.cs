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
    public partial class frm_home_ADMIN_accountdata : Form
    {
        private IMongoCollection<BsonDocument> _loanAccountTitlesCollection;

        public frm_home_ADMIN_accountdata()
        {
            InitializeComponent();
            var database = MongoDBConnection.Instance.Database;
            _loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

            Disable();
            ConfigureAutocompleteForTextBoxes();

            taccno.ReadOnly = true;
        }

        private void ConfigureAutocompleteForTextBoxes()
        {
            // Call MongoDB and populate autocomplete for each textbox
            taccno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            taccno.AutoCompleteSource = AutoCompleteSource.CustomSource;
            taccgrp.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            taccgrp.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tacccode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tacccode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            taccgrpcode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            taccgrpcode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            taccname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            taccname.AutoCompleteSource = AutoCompleteSource.CustomSource;

            // Populate autocomplete suggestions
            PopulateAutocompleteSuggestions();
        }

        private void PopulateAutocompleteSuggestions()
        {
            try
            {
                var accNoSuggestions = _loanAccountTitlesCollection.Distinct<string>("AccountId", Builders<BsonDocument>.Filter.Empty).ToList();
                var accGrpSuggestions = _loanAccountTitlesCollection.Distinct<string>("AccountGroup", Builders<BsonDocument>.Filter.Empty).ToList();
                var accCodeSuggestions = _loanAccountTitlesCollection.Distinct<string>("AccountCode", Builders<BsonDocument>.Filter.Empty).ToList();
                var accGrpCodeSuggestions = _loanAccountTitlesCollection.Distinct<string>("AccountGroupCode", Builders<BsonDocument>.Filter.Empty).ToList();
                var accNameSuggestions = _loanAccountTitlesCollection.Distinct<string>("AccountName", Builders<BsonDocument>.Filter.Empty).ToList();

                // Create AutoCompleteStringCollection for each textbox
                AutoCompleteStringCollection accNoCollection = new AutoCompleteStringCollection();
                accNoCollection.AddRange(accNoSuggestions.ToArray());
                taccno.AutoCompleteCustomSource = accNoCollection;

                AutoCompleteStringCollection accGrpCollection = new AutoCompleteStringCollection();
                accGrpCollection.AddRange(accGrpSuggestions.ToArray());
                taccgrp.AutoCompleteCustomSource = accGrpCollection;

                AutoCompleteStringCollection accCodeCollection = new AutoCompleteStringCollection();
                accCodeCollection.AddRange(accCodeSuggestions.ToArray());
                tacccode.AutoCompleteCustomSource = accCodeCollection;

                AutoCompleteStringCollection accGrpCodeCollection = new AutoCompleteStringCollection();
                accGrpCodeCollection.AddRange(accGrpCodeSuggestions.ToArray());
                taccgrpcode.AutoCompleteCustomSource = accGrpCodeCollection;

                AutoCompleteStringCollection accNameCollection = new AutoCompleteStringCollection();
                accNameCollection.AddRange(accNameSuggestions.ToArray());
                taccname.AutoCompleteCustomSource = accNameCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting autocomplete: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception in PopulateAutocompleteSuggestions: {ex}");
            }
        }

        private void Disable() 
        {
            tacccode.Enabled = false;
            taccgrp.Enabled = false;
            taccgrpcode.Enabled = false;
            taccname.Enabled = false;
        }

        private void Enable()
        {
            tacccode.Enabled = true;
            taccgrp.Enabled = true;
            taccgrpcode.Enabled = true;
            taccname.Enabled = true;
        }

        private string GenerateNextId()
        {
            try
            {
                // Find the most recent ID
                var filter = Builders<BsonDocument>.Filter.Empty;
                var sort = Builders<BsonDocument>.Sort.Descending("AccountId");
                var latestRecord = _loanAccountTitlesCollection.Find(filter).Sort(sort).FirstOrDefault();

                int nextNumber = 1; // Default to 1 if no record is found

                if (latestRecord != null)
                {
                    // Extract the number from the last ID
                    string lastId = latestRecord["AccountId"].ToString();
                    if (int.TryParse(lastId, out int lastNumber))
                    {
                        nextNumber = lastNumber + 3;
                    }
                    else
                    {
                        // Handle cases where lastId is not an integer
                        MessageBox.Show("The latest AccountId is not in a valid numeric format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return string.Empty;
                    }
                }

                // Generate the new ID
                string newId = nextNumber.ToString();
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void LoadDataIntoDGV(string searchText = "")
        {
            try
            {
                lnorecord.Visible = false;

                // If searchText is provided, apply a filter
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;
                if (!string.IsNullOrEmpty(searchText))
                {
                    filter = Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression(searchText, "i")),
                        Builders<BsonDocument>.Filter.Regex("AccountGroup", new BsonRegularExpression(searchText, "i")),
                        Builders<BsonDocument>.Filter.Regex("AccountCode", new BsonRegularExpression(searchText, "i")),
                        Builders<BsonDocument>.Filter.Regex("AccountGroupCode", new BsonRegularExpression(searchText, "i")),
                        Builders<BsonDocument>.Filter.Regex("AccountName", new BsonRegularExpression(searchText, "i"))
                    );
                }

                // Fetch and filter data from MongoDB
                var loanAccountTitles = _loanAccountTitlesCollection.Find(filter).ToList();

                if (loanAccountTitles.Count > 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("AccountId");
                    dataTable.Columns.Add("AccountGroup");
                    dataTable.Columns.Add("AccountCode");
                    dataTable.Columns.Add("AccountGroupCode");
                    dataTable.Columns.Add("AccountName");

                    foreach (var document in loanAccountTitles)
                    {
                        DataRow row = dataTable.NewRow();
                        row["AccountId"] = document["AccountId"].ToString();
                        row["AccountGroup"] = document["AccountGroup"].ToString();
                        row["AccountCode"] = document["AccountCode"].ToString();
                        row["AccountGroupCode"] = document["AccountGroupCode"].ToString();
                        row["AccountName"] = document["AccountName"].ToString();
                        dataTable.Rows.Add(row);
                    }

                    dgvdata.DataSource = dataTable;

                    // Set custom column headers
                    dgvdata.Columns["AccountId"].HeaderText = "Account ID";
                    dgvdata.Columns["AccountGroup"].HeaderText = "Account Group";
                    dgvdata.Columns["AccountCode"].HeaderText = "Account Code";
                    dgvdata.Columns["AccountGroupCode"].HeaderText = "Account Group Code";
                    dgvdata.Columns["AccountName"].HeaderText = "Account Name";
                }
                else
                {
                    dgvdata.DataSource = null;
                    lnorecord.Text = "No records found.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception in LoadDataIntoDGV: {ex}");
            }
        }

        private void frm_home_ADMIN_accountdata_Load(object sender, EventArgs e)
        {
            // Generate and display the new ID
            string newId = GenerateNextId();
            taccno.Text = newId;

            // Load data into DataGridView
            LoadDataIntoDGV();
        }

        private void bloansave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(taccgrp.Text) ||
                    string.IsNullOrEmpty(tacccode.Text) ||
                    string.IsNullOrEmpty(taccgrpcode.Text) ||
                    string.IsNullOrEmpty(taccname.Text) ||
                    string.IsNullOrEmpty(taccno.Text))
                {
                    MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string accountId = taccno.Text;
                if (!int.TryParse(accountId, out int _))
                {
                    MessageBox.Show("Invalid AccountId format. Must be numeric.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if an existing document with the same AccountId already exists
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var existingDocument = _loanAccountTitlesCollection.Find(filter).FirstOrDefault();

                if (existingDocument != null)
                {
                    // Notify user that the AccountId already exists
                    MessageBox.Show("An account with this ID already exists. No new data saved.", "Duplicate Account ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit the method to prevent saving
                }

                // Insert a new document if no existing document with the same AccountId is found
                var newDocument = new BsonDocument
                {
                    { "AccountId", accountId },
                    { "AccountGroup", taccgrp.Text },
                    { "AccountCode", tacccode.Text },
                    { "AccountGroupCode", taccgrpcode.Text },
                    { "AccountName", taccname.Text }
                };

                _loanAccountTitlesCollection.InsertOne(newDocument);

                MessageBox.Show("Account data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload the data into DataGridView
                LoadDataIntoDGV();

                // Clear the input fields
                taccno.Clear();
                taccgrp.Clear();
                tacccode.Clear();
                taccgrpcode.Clear();
                taccname.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving account data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ReorderAccountIds()
        {
            try
            {
                // Retrieve all remaining documents, sorted by the current AccountId
                var allDocuments = _loanAccountTitlesCollection.Find(new BsonDocument())
                    .Sort(Builders<BsonDocument>.Sort.Ascending("AccountId"))
                    .ToList();

                // Start from 1 to assign new AccountIds
                int newAccountIdNumber = 1;

                foreach (var document in allDocuments)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]);

                    // Generate the new AccountId as a string
                    string newAccountId = newAccountIdNumber.ToString();

                    // Update the document's AccountId with the new value
                    var updateDefinition = Builders<BsonDocument>.Update.Set("AccountId", newAccountId);
                    _loanAccountTitlesCollection.UpdateOne(filter, updateDefinition);

                    // Increment the number for the next AccountId
                    newAccountIdNumber++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reordering AccountIds: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception in ReorderAccountIds: {ex}");
            }
        }

        private void beditrate_Click(object sender, EventArgs e)
        {
            Enable();
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            LoadDataIntoDGV(tsearch.Text);
        }

        private void bdel_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure the AccountId is provided
                if (string.IsNullOrEmpty(taccno.Text))
                {
                    MessageBox.Show("Please enter a valid AccountId to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a filter to identify the document to delete based on AccountId
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", taccno.Text);

                // Find the document to delete
                var documentToDelete = _loanAccountTitlesCollection.Find(filter).FirstOrDefault();
                if (documentToDelete == null)
                {
                    MessageBox.Show("AccountId not found.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Delete the document
                _loanAccountTitlesCollection.DeleteOne(filter);
                MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // After deletion, reorder the remaining documents' AccountId
                ReorderAccountIds();

                // Reload the data into the DataGridView to reflect the changes
                LoadDataIntoDGV();

                // Clear the input fields
                taccno.Clear();
                taccgrp.Clear();
                tacccode.Clear();
                taccgrpcode.Clear();
                taccname.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting account data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception in bdel_Click: {ex}");
            }
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the click is not on the header row and the index is valid
            if (e.RowIndex >= 0)
            {
                // Retrieve the selected row
                DataGridViewRow row = dgvdata.Rows[e.RowIndex];

                // Populate the textboxes with the row data
                taccno.Text = row.Cells["AccountId"].Value.ToString();
                taccgrp.Text = row.Cells["AccountGroup"].Value.ToString();
                tacccode.Text = row.Cells["AccountCode"].Value.ToString();
                taccgrpcode.Text = row.Cells["AccountGroupCode"].Value.ToString();
                taccname.Text = row.Cells["AccountName"].Value.ToString();

                // Enable the textboxes for editing
                Enable();
            }
        }
    }
}
