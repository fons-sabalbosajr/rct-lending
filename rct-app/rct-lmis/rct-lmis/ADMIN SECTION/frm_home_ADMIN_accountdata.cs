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
                // Define the prefix and format
                string prefix = "RCT-AT";
                string idFormat = "D3"; // Three-digit format

                // Find the most recent ID
                var filter = Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression($"^{prefix}"));
                var sort = Builders<BsonDocument>.Sort.Descending("AccountId");
                var latestRecord = _loanAccountTitlesCollection.Find(filter).Sort(sort).FirstOrDefault();

                int nextNumber = 1; // Default to 1 if no record is found

                if (latestRecord != null)
                {
                    // Extract the number from the last ID
                    string lastId = latestRecord["AccountId"].ToString();
                    if (int.TryParse(lastId.Substring(prefix.Length), out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                // Generate the new ID
                string newId = $"{prefix}{nextNumber.ToString(idFormat)}";
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void LoadDataIntoDGV()
        {
            try
            {
                lnorecord.Visible = false;
                var filter = Builders<BsonDocument>.Filter.Empty;
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

            // Generate and display the new ID for the next record
            _ = GenerateNextId();
            taccno.Text = newId;
        }

        private void bloansave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrEmpty(taccgrp.Text) ||
                    string.IsNullOrEmpty(tacccode.Text) ||
                    string.IsNullOrEmpty(taccgrpcode.Text) ||
                    string.IsNullOrEmpty(taccname.Text))
                {
                    MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a new BSON document with the values from textboxes
                var newDocument = new BsonDocument
                {
                    { "AccountId", taccno.Text },
                    { "AccountGroup", taccgrp.Text },
                    { "AccountCode", tacccode.Text },
                    { "AccountGroupCode", taccgrpcode.Text },
                    { "AccountName", taccname.Text }
                };

                // Insert the new document into the collection
                _loanAccountTitlesCollection.InsertOne(newDocument);

                // Notify the user
                MessageBox.Show("Account data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally, reload the data into the DataGridView to reflect the new entry
                LoadDataIntoDGV();

                taccname.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving account data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception in btnSave_Click: {ex}");
            }
        }

        private void beditrate_Click(object sender, EventArgs e)
        {
            Enable();
        }
    }
}
