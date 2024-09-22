using ExcelDataReader;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
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

            //Disable();
            ConfigureAutocompleteForTextBoxes();

            //taccno.ReadOnly = true;
        }

        LoadingFunction load = new LoadingFunction();
        private string selectedDocumentId;

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

        private string GenerateUniqueId()
        {
            var maxDocument = _loanAccountTitlesCollection.Find(Builders<BsonDocument>.Filter.Empty)
                .Sort(Builders<BsonDocument>.Sort.Descending("AccountId"))
                .Limit(1)
                .FirstOrDefault();

            if (maxDocument != null && maxDocument.Contains("AccountId"))
            {
                if (int.TryParse(maxDocument["AccountId"].AsString, out int maxId))
                {
                    return (maxId + 1).ToString();
                }
            }

            // If no documents or unable to parse, start from 1
            return "1";
        }


        private DataTable ReadFile(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string extension = Path.GetExtension(filePath);
            DataTable dataTable = new DataTable();

            if (extension == ".csv")
            {
                using (var reader = new StreamReader(filePath))
                {
                    string[] headers = reader.ReadLine().Split(',');
                    foreach (var header in headers)
                    {
                        dataTable.Columns.Add(header.Trim());
                    }
                    while (!reader.EndOfStream)
                    {
                        var rows = reader.ReadLine().Split(',');
                        var row = dataTable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            row[i] = rows[i].Trim();
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            else if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true // Use the first row as column headers
                            }
                        });
                        dataTable = result.Tables[0]; // Assuming the data is in the first sheet
                    }
                }
            }

            return dataTable;
        }

        private void SaveDataToMongoDB(DataTable dataTable)
        {
            pbloading.Minimum = 0;
            pbloading.Maximum = dataTable.Rows.Count;
            pbloading.Value = 0;
            pbloading.Visible = true; // Show progress bar

            foreach (DataRow row in dataTable.Rows)
            {
                var document = new BsonDocument();

                // Generate a unique AccountId
                document["AccountId"] = GenerateUniqueId(); // Replace with your ID generation logic

                // Add other fields from the row
                foreach (DataColumn column in dataTable.Columns)
                {
                    document[column.ColumnName] = BsonValue.Create(row[column.ColumnName]);
                }

                _loanAccountTitlesCollection.InsertOne(document);
                pbloading.Value += 1; // Update progress bar
            }

            pbloading.Visible = false; // Hide the progress bar
        }


        private void LoadDataToDataGridView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var systemMetadataCollection = database.GetCollection<BsonDocument>("system_metadata");
                var loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

                var updateFlag = systemMetadataCollection.Find(Builders<BsonDocument>.Filter.Eq("updateApplied", true)).FirstOrDefault();

                // Check if the update has already been applied
                if (updateFlag != null)
                {
                    // Load the data normally without applying the fix
                    LoadDataWithoutFix();
                    return;
                }

                // If update not applied, proceed with fixing AccountId
                var documents = loanAccountTitlesCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // Sort the documents by the current AccountId as integers
                var sortedDocuments = documents.OrderBy(doc =>
                {
                    int accountId;
                    return int.TryParse(doc["AccountId"].ToString(), out accountId) ? accountId : 0;
                }).ToList();

                DataTable dataTable = new DataTable();
                int newAccountId = 1; // Start fixing AccountId from 1

                if (sortedDocuments.Count > 0)
                {
                    // Dynamically create columns based on the first document
                    foreach (var element in sortedDocuments[0].Elements)
                    {
                        dataTable.Columns.Add(element.Name);
                    }

                    // Loop through each document and fix AccountId
                    foreach (var doc in sortedDocuments)
                    {
                        DataRow row = dataTable.NewRow();
                        row["AccountId"] = newAccountId;
                        doc["AccountId"] = newAccountId.ToString(); // Update the document in-memory

                        foreach (var element in doc.Elements)
                        {
                            if (element.Name != "AccountId") // We've already updated AccountId
                            {
                                row[element.Name] = doc[element.Name].ToString();
                            }
                        }

                        // Add the row to the DataTable
                        dataTable.Rows.Add(row);

                        // Update the document in MongoDB with the fixed AccountId
                        var filter = Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]);
                        var update = Builders<BsonDocument>.Update.Set("AccountId", newAccountId.ToString());
                        loanAccountTitlesCollection.UpdateOne(filter, update);

                        newAccountId++; // Increment to next AccountId
                    }
                }

                dgvdata.DataSource = dataTable; // Bind to DataGridView

                // Hide the _id column if it exists
                if (dgvdata.Columns.Contains("_id"))
                {
                    dgvdata.Columns["_id"].Visible = false;
                }

                lnorecord.Visible = dataTable.Rows.Count == 0; // Show or hide message if no records

                // Set the flag in system_metadata collection after fixing
                var flagDocument = new BsonDocument { { "updateApplied", true } };
                systemMetadataCollection.InsertOne(flagDocument);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataWithoutFix()
        {
            var database = MongoDBConnection.Instance.Database;
            var loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

            var documents = loanAccountTitlesCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
            DataTable dataTable = new DataTable();

            if (documents.Count > 0)
            {
                foreach (var element in documents[0].Elements)
                {
                    dataTable.Columns.Add(element.Name);
                }

                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var element in doc.Elements)
                    {
                        row[element.Name] = doc[element.Name].ToString();
                    }
                    dataTable.Rows.Add(row);
                }
            }

            dgvdata.DataSource = dataTable; // Bind to DataGridView

            if (dgvdata.Columns.Contains("_id"))
            {
                dgvdata.Columns["_id"].Visible = false;
            }

            lnorecord.Visible = dataTable.Rows.Count == 0; // Show or hide message if no records
        }

        private void LoadFilteredData(string searchTerm)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

                // Build a filter to match fields based on the search term (case-insensitive)
                var filter = Builders<BsonDocument>.Filter.Or(
                    Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression(searchTerm, "i")),
                    Builders<BsonDocument>.Filter.Regex("Account Group", new BsonRegularExpression(searchTerm, "i")),
                    Builders<BsonDocument>.Filter.Regex("Account Code", new BsonRegularExpression(searchTerm, "i")),
                    Builders<BsonDocument>.Filter.Regex("Account Group Code", new BsonRegularExpression(searchTerm, "i")),
                    Builders<BsonDocument>.Filter.Regex("Account Name", new BsonRegularExpression(searchTerm, "i"))
                );

                // If search term is empty, reset to loading all data (no filter)
                var documents = string.IsNullOrEmpty(searchTerm)
                    ? loanAccountTitlesCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList()
                    : loanAccountTitlesCollection.Find(filter).ToList();

                DataTable dataTable = new DataTable();

                // Load columns based on MongoDB document keys (just like before)
                foreach (var column in documents.First().Names)
                {
                    dataTable.Columns.Add(column);
                }

                // Populate rows from the filtered documents
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var column in doc.Names)
                    {
                        row[column] = doc.Contains(column) ? doc[column].ToString() : string.Empty;
                    }
                    dataTable.Rows.Add(row);
                }

                // Set the DataGridView's DataSource to the filtered results
                dgvdata.DataSource = dataTable;

                // Show or hide the no record message based on the result count
                lnorecord.Visible = dataTable.Rows.Count == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading filtered data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            taccno.Text = string.Empty;
            taccgrp.Text = string.Empty;
            tacccode.Text = string.Empty;
            taccgrpcode.Text = string.Empty;
            taccname.Text = string.Empty;
        }


        private void frm_home_ADMIN_accountdata_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();

            dgvdata.ClearSelection();
        }

        private void bloansave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedDocumentId))
                {
                    MessageBox.Show("Please select a record to update.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var database = MongoDBConnection.Instance.Database;
                var loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

                // Create a filter to locate the document by its _id
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(selectedDocumentId));

                // Create an update definition with the new values from the textboxes
                var update = Builders<BsonDocument>.Update
                    .Set("AccountId", taccno.Text)
                    .Set("Account Group", taccgrp.Text)
                    .Set("Account Code", tacccode.Text)
                    .Set("Account Group Code", taccgrpcode.Text)
                    .Set("Account Name", taccname.Text);

                // Perform the update operation
                var result = loanAccountTitlesCollection.UpdateOne(filter, update);

                if (result.ModifiedCount > 0)
                {
                    MessageBox.Show("Record updated successfully!", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView(); // Refresh the data in the DataGridView
                }
                else
                {
                    MessageBox.Show("No record was updated. Please check the data.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating record: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredData(tsearch.Text);
        }

        private void bdel_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedDocumentId))
                {
                    MessageBox.Show("Please select a record to delete.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var database = MongoDBConnection.Instance.Database;
                var loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

                // Create a filter to locate the document by its _id
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(selectedDocumentId));

                // Confirm delete action
                var confirmation = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmation == DialogResult.Yes)
                {
                    // Perform the delete operation
                    var result = loanAccountTitlesCollection.DeleteOne(filter);

                    if (result.DeletedCount > 0)
                    {
                        MessageBox.Show("Record deleted successfully!", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToDataGridView(); // Refresh the data in the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record was deleted. Please check the data.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvdata.Rows.Count)
            {
                DataGridViewRow row = dgvdata.Rows[e.RowIndex];

                // Store the selected document's ID
                selectedDocumentId = row.Cells["_id"].Value?.ToString();

                // Populate the textboxes with the values from the selected row
                taccno.Text = row.Cells["AccountId"].Value?.ToString() ?? string.Empty;
                taccgrp.Text = row.Cells["Account Group"].Value?.ToString() ?? string.Empty;
                tacccode.Text = row.Cells["Account Code"].Value?.ToString() ?? string.Empty;
                taccgrpcode.Text = row.Cells["Account Group Code"].Value?.ToString() ?? string.Empty;
                taccname.Text = row.Cells["Account Name"].Value?.ToString() ?? string.Empty;
            }
        }

        private void bupload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|CSV Files|*.csv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DataTable dataTable = ReadFile(filePath);

                    // Optional: Show loading screen
                    load.Show(this);
                    Thread.Sleep(1000);
                    SaveDataToMongoDB(dataTable);
                    LoadDataToDataGridView();
                    load.Close();

                    MessageBox.Show("Data uploaded successfully!");
                }
            }
        }



        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void badd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs before adding a new record
                if (string.IsNullOrWhiteSpace(taccno.Text) ||
                    string.IsNullOrWhiteSpace(taccgrp.Text) ||
                    string.IsNullOrWhiteSpace(tacccode.Text) ||
                    string.IsNullOrWhiteSpace(taccgrpcode.Text) ||
                    string.IsNullOrWhiteSpace(taccname.Text))
                {
                    MessageBox.Show("Please fill in all fields before adding a new record.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var database = MongoDBConnection.Instance.Database;
                var loanAccountTitlesCollection = database.GetCollection<BsonDocument>("loan_account_titles");

                // Create a new document
                var newDocument = new BsonDocument
                 {
                     { "AccountId", taccno.Text },
                     { "Account Group", taccgrp.Text },
                     { "Account Code", tacccode.Text },
                     { "Account Group Code", taccgrpcode.Text },
                     { "Account Name", taccname.Text }
                 };

                // Insert the new document into the MongoDB collection
                loanAccountTitlesCollection.InsertOne(newDocument);

                // Show success message
                MessageBox.Show("New record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the textboxes after adding the record
                ClearInputFields();

                // Refresh the DataGridView to show the new data
                LoadDataToDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bhelp_Click(object sender, EventArgs e)
        {
            // Define the message content
            string formInfo = "Form: Loan Account Titles Management\n" +
                              "Description: This form allows users to manage loan account titles, including adding, updating, deleting, and searching records.\n\n" +
                              "Fields:\n" +
                              "- AccountId: Unique identifier for the account\n" +
                              "- Account Group: Grouping category for the account\n" +
                              "- Account Code: Code representing the account\n" +
                              "- Account Group Code: Numeric code for the account group\n" +
                              "- Account Name: Name of the account\n\n" +
                              "Instructions:\n" +
                              "- Use the Add button to add a new record.\n" +
                              "- Use the Update button to modify an existing record.\n" +
                              "- Use the Delete button to remove a record.\n" +
                              "- You can search by any field using the search bar.";

            // Show the message box with the form information
            MessageBox.Show(formInfo, "Form Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bexport_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            try
            {
                // Check if there is data to export
                if (dgvdata.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to export.", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a SaveFileDialog to specify the Excel file location
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var package = new ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("Loan Account Titles");

                            // Add Title and Date
                            worksheet.Cells["A1"].Value = "Loan Account Titles Management";
                            worksheet.Cells["A2"].Value = $"Date: {DateTime.Now.ToShortDateString()}";

                            // Add headers, excluding the _id column
                            int headerRowIndex = 4;
                            for (int i = 0; i < dgvdata.Columns.Count; i++)
                            {
                                if (dgvdata.Columns[i].HeaderText != "_id") // Exclude _id
                                {
                                    worksheet.Cells[headerRowIndex, i + 1].Value = dgvdata.Columns[i].HeaderText;
                                }
                            }

                            // Add data, excluding the _id column
                            for (int row = 0; row < dgvdata.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvdata.Columns.Count; col++)
                                {
                                    if (dgvdata.Columns[col].HeaderText != "_id") // Exclude _id
                                    {
                                        worksheet.Cells[row + headerRowIndex + 1, col + 1].Value = dgvdata.Rows[row].Cells[col].Value?.ToString();
                                    }
                                }
                            }

                            // Auto-fit columns
                            worksheet.Cells.AutoFitColumns();

                            // Save the file
                            File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());
                            MessageBox.Show("Data exported successfully to Excel!", "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Open the folder where the file was saved
                            string folderPath = Path.GetDirectoryName(sfd.FileName);
                            Process.Start("explorer.exe", folderPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
