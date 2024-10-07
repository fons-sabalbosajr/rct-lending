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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TesseractOCR.Font;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_collectors : Form
    {

        private string _currentId;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> loanCollectorsCollection;

        public frm_home_ADMIN_collectors()
        {
            InitializeComponent();
            database = MongoDBConnection.Instance.Database;
            loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_area_routes");
            tidno.Enabled = false;
        }


        LoadingFunction load = new LoadingFunction();


        private void DisableElements()
        {
            cbarea.Enabled = false;
            tidno.Enabled = false;
            tname.Enabled = false;
            taddress.Enabled = false;
            tcontactno.Enabled = false;
            tcontactnoalt.Enabled = false;
            temail.Enabled = false;
            cbempstatus.Enabled = false;
            dtdateemp.Enabled = false;
            trole.Enabled = false;
            buploaddoc.Enabled = false;
        }

        private void EnableElements()
        {
            cbarea.Enabled = true;
           // tidno.Enabled = true;
            tname.Enabled = true;
            taddress.Enabled = true;
            tcontactno.Enabled = true;
            tcontactnoalt.Enabled = true;
            temail.Enabled = true;
            cbempstatus.Enabled = true;
            dtdateemp.Enabled = true;
            trole.Enabled = true;
            buploaddoc.Enabled = true;
        }

        private string GetIncrementId()
        {
            var database = MongoDBConnection.Instance.Database;
            var loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

            // Update the regex to match 'RCT-COL'
            var filter = Builders<BsonDocument>.Filter.Regex("GeneratedIDNumber", new BsonRegularExpression("^RCT-COL"));
            var sort = Builders<BsonDocument>.Sort.Descending("GeneratedIDNumber");

            // Find the document with the highest ID number
            var highestDoc = loanCollectorsCollection.Find(filter).Sort(sort).FirstOrDefault();

            if (highestDoc != null)
            {
                // Extract the current highest ID and increment it
                string highestId = highestDoc["GeneratedIDNumber"].AsString;

                // Extract the numeric part after 'COL'
                string numericPart = highestId.Substring(highestId.LastIndexOf("COL") + 3);

                // Convert to integer, increment, and format back to a 4-digit string
                int currentNumber = int.Parse(numericPart);
                int nextNumber = currentNumber + 1;

                return nextNumber.ToString("D4"); // Return the ID in the format C0001, C0002, etc.
            }
            else
            {
                // If no existing ID is found, start with C0001
                return "0001";
            }
        }


        private void GenerateCollectorID()
        {
            string incrementId = GetIncrementId();
            tidno.Text = $"RCT-COL{incrementId}";
        }


        private void ClearForm()
        {
            tidno.Text = string.Empty;
            tname.Text = string.Empty;
            taddress.Text = string.Empty;
            tcontactno.Text = string.Empty;
            tcontactnoalt.Text = string.Empty;
            temail.Text = string.Empty;
            cbarea.SelectedIndex = -1;
            cbempstatus.SelectedIndex = -1;
            dtdateemp.Value = DateTime.Now;
            trole.Text = string.Empty;

            // Clear the listbox items
            listarea_routes.Items.Clear();

            // Reset any other fields if needed
            _currentId = null;
        }


        private void SaveCollector()
        {
            var database = MongoDBConnection.Instance.Database;
            var loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

            // Create a list to hold the area routes from the listbox
            var areaRoutesList = new BsonArray();

            foreach (var item in listarea_routes.Items)
            {
                areaRoutesList.Add(item.ToString());
            }

            // Create a BsonDocument with the collector's data
            var collectorData = new BsonDocument
             {
                 { "GeneratedIDNumber", tidno.Text },
                 { "Name", tname.Text },
                 { "Address", taddress.Text },
                 { "ContactNumber", tcontactno.Text },
                 { "AlternateContactNumber", tcontactnoalt.Text },
                 { "EmploymentStatus", cbempstatus.SelectedItem?.ToString() },
                 { "Email", temail.Text },
                 { "EmploymentDate", dtdateemp.Value.ToString("d") },
                 { "Role", trole.Text },
                 { "ReceiptBookNo", trecbookno.Text },
                 { "BankAccount", tbankaccountno.Text },
                 { "AreaRoutes", areaRoutesList } // Add the list of area routes to the document
             };

            var filter = Builders<BsonDocument>.Filter.Eq("GeneratedIDNumber", tidno.Text);
            loanCollectorsCollection.ReplaceOne(filter, collectorData);

            // Optionally, refresh the DataGridView or perform other post-save actions here
            LoadDataToDataGridView(); // Reload the data into the DataGridView after save/update
            ClearForm();
        }


        private void AddCollector()
        {
            var database = MongoDBConnection.Instance.Database;
            var loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

            // Create a list to hold the area routes from the listbox
            var areaRoutesList = new BsonArray();

            foreach (var item in listarea_routes.Items)
            {
                areaRoutesList.Add(item.ToString());
            }

            // Create a BsonDocument with the collector's data
            var collectorData = new BsonDocument
             {
                 { "GeneratedIDNumber", tidno.Text },
                 { "Name", tname.Text },
                 { "Address", taddress.Text },
                 { "ContactNumber", tcontactno.Text },
                 { "AlternateContactNumber", tcontactnoalt.Text },
                 { "EmploymentStatus", cbempstatus.SelectedItem?.ToString() },
                 { "Email", temail.Text },
                 { "EmploymentDate", dtdateemp.Value.ToString("MM/dd/yyyy") },
                 { "Role", trole.Text },
                 { "AreaRoutes", areaRoutesList } // Add the list of area routes to the document
             };

            loanCollectorsCollection.InsertOne(collectorData);

            // Optionally, refresh the DataGridView or perform other post-save actions here
            LoadDataToDataGridView(); // Reload the data into the DataGridView after save/update
            ClearForm();    // Clear input fields after save/update
        }


        private void SetupDataGridView()
        {
            dgvdatacollector.Columns.Clear();
            dgvdatacollector.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvdatacollector.AllowUserToAddRows = false;
            dgvdatacollector.AllowUserToDeleteRows = false;
            dgvdatacollector.AllowUserToOrderColumns = false;
            dgvdatacollector.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Add columns
            dgvdatacollector.Columns.Add("AreaRoute", "Area Route");
            dgvdatacollector.Columns.Add("IDNumber", "ID Number/Receip Book Info.");
            dgvdatacollector.Columns.Add("ContactInfo", "Collector Information");
            dgvdatacollector.Columns.Add("EmploymentStatus", "Employment Status");

            if (dgvdatacollector.Columns["btnAction"] == null)
            {
                DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn
                {
                    Name = "Action",
                    HeaderText = "Action",
                    Text = "Generate Report",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Standard
                };
                dgvdatacollector.Columns.Add(actionColumn);
            }

            if (dgvdatacollector.Columns["btnPayroll"] == null)
            {
                DataGridViewButtonColumn payrollColumn = new DataGridViewButtonColumn
                {
                    Name = "Payroll",
                    HeaderText = "",
                    Text = "Generate Payroll Form",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Standard
                };
                dgvdatacollector.Columns.Add(payrollColumn);
            }

            if (dgvdatacollector.Columns["btnSuspend"] == null)
            {
                DataGridViewButtonColumn suspendColumn = new DataGridViewButtonColumn
                {
                    Name = "Documents",
                    HeaderText = "",
                    Text = "View Documents",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Standard,
                };
               dgvdatacollector.Columns.Add(suspendColumn);
            }

            var btnColumn = dgvdatacollector.Columns["Action"];
            if (btnColumn != null)
            {
                btnColumn.Width = 120;
                btnColumn.DefaultCellStyle.Padding = new Padding(17, 20, 17, 20);
                btnColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumn.DefaultCellStyle.Font = new Font("Arial", 8);
            }

            var btnColumn2 = dgvdatacollector.Columns["Payroll"];
            if (btnColumn2 != null)
            {            
                btnColumn2.Width = 120;
                btnColumn2.DefaultCellStyle.Padding = new Padding(17, 20, 17, 20);
                btnColumn2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumn2.DefaultCellStyle.Font = new Font("Arial", 8);
            }

            var btnColumn3 = dgvdatacollector.Columns["Documents"];
            if (btnColumn3 != null)
            {            
                btnColumn3.Width = 120;
                btnColumn3.DefaultCellStyle.Padding = new Padding(17, 20, 17, 20);
                btnColumn3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumn3.DefaultCellStyle.Font = new Font("Arial", 8);
            }

            // Adjust column widths
            dgvdatacollector.Columns["AreaRoute"].Width = 100;
            dgvdatacollector.Columns["IDNumber"].Width = 120;
            dgvdatacollector.Columns["ContactInfo"].Width = 250;
            dgvdatacollector.Columns["EmploymentStatus"].Width = 150;

           
            // Auto-size columns mode and row height mode
            dgvdatacollector.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvdatacollector.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvdatacollector.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }


        private void LoadDataToDataGridView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

                var filter = new BsonDocument(); // Fetch all documents
                var loanCollectors = loanCollectorsCollection.Find(filter).ToList();

                dgvdatacollector.Rows.Clear();

                foreach (var doc in loanCollectors)
                {
                    string areaRoute = doc.Contains("AreaRoutes")
                        ? string.Join("\n", doc["AreaRoutes"].AsBsonArray.Select(a => a.AsString))
                        : "N/A"; // Combine area routes into a single string or set to "N/A"

                    string idNumber = doc["GeneratedIDNumber"].AsString;
                    string name = doc["Name"].AsString;
                    string address = doc["Address"].AsString;
                    string contactNo = doc["ContactNumber"].AsString;
                    string contactNoAlt = doc["AlternateContactNumber"].AsString;
                    string email = doc["Email"].AsString;
                    string empdateStr = doc["EmploymentDate"].AsString;
                    string receiptbook = doc.Contains("ReceiptBookNo") ? doc["ReceiptBookNo"].AsString : "N/A";
                    string role = doc["Role"].AsString;
                    string bankacc = doc.Contains("BankAccount") ? doc["BankAccount"].AsString : "N/A";
                    string employmentStatus = doc["EmploymentStatus"].AsString;
                    string idinfo = $"{idNumber}\n{receiptbook}";
                    string contactInfo = $"{name}\n{address}\n{contactNo}\n{contactNoAlt}\n{email}\n{role}\n{empdateStr}\n{bankacc}";

                    dgvdatacollector.Rows.Add(areaRoute, idinfo, contactInfo, employmentStatus);

                    // Convert the EmploymentDate to DateTime and set the dtdataemp value
                    if (DateTime.TryParseExact(empdateStr, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime empdate))
                    {
                        dtdateemp.Value = empdate;
                    }
                    else
                    {
                        // Handle the case where the date is invalid or in an unexpected format
                        MessageBox.Show($"Invalid Employment Date format for ID {idNumber}: {empdateStr}", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Check if the DataGridView is empty and show/hide the label accordingly
                lnorecord.Visible = dgvdatacollector.Rows.Count == 0; // Show the label if no records
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error, show a message box)
                MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadAreaIntoComboBox()
        {
            try
            {
                // Fetch all areas from the loan_area_routes collection
                var documents = loanCollectorsCollection.Find(new BsonDocument()).ToList();

                // Populate the combo box with the 'Area' field
                cbarea.Items.Clear(); // Clear any previous items
                cbarea.Items.Add("--Select Area--"); // Add default item
                foreach (var document in documents)
                {
                    string area = document["Area"].AsString;
                    cbarea.Items.Add(area); // Add each area to the combo box
                }

                cbarea.SelectedIndex = 0; // Select the default item
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void frm_home_ADMIN_collectors_Load(object sender, EventArgs e)
        {
            DisableElements();
            SetupDataGridView(); // Ensure columns are set up
            LoadDataToDataGridView();

            LoadAreaIntoComboBox();
        }

        private void buploaddoc_Click(object sender, EventArgs e)
        {
            frm_home_ADMIN_collectors_upload upload = new frm_home_ADMIN_collectors_upload();

            load.Show(this);
            Thread.Sleep(1000);
            load.Close();
            upload.Show(this);
        }

      

        private void badd_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(1000);
            AddCollector();
            load.Close();
            MessageBox.Show(this, "Collector information added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataToDataGridView();
            badd.Enabled = false;
        }

        private void cbarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the list box when the default selection is active
            if (cbarea.SelectedItem?.ToString() == "--Select Area--")
            {
                listarea_routes.Items.Clear(); // Clear the list box
            }
            else
            {
                // Add selected area route to the list box
                string selectedAreaRoute = cbarea.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(selectedAreaRoute))
                {
                    listarea_routes.Items.Add(selectedAreaRoute);
                }
            }
        }

        private void bsave_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(1000);
            SaveCollector();
            load.Close();
            MessageBox.Show(this, "Collector information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Optionally, clear the form after saving
            //ClearForm();
        }

        private void dgvdatacollector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var columnName = dgvdatacollector.Columns[e.ColumnIndex].Name;
               
                // Check which button was clicked
                if (columnName == "Action")
                {
                    // Handle "Generate Report"
                    MessageBox.Show(this, "This function is not yet supported in this version.", 
                        "Feature not yet supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (columnName == "Payroll")
                {
                    // Handle "Generate Payroll Form"
                    MessageBox.Show(this, "This function is not yet supported in this version.",
                       "Feature not yet supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (columnName == "Documents")
                {
                   frm_home_ADMIN_collector_attachments attachments = new frm_home_ADMIN_collector_attachments();
                    attachments.Show(this);
                }
            }
        }

        private void dgvdatacollector_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdatacollector.ClearSelection();
        }



        private void dgvdatacollector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EnableElements();
            bsave.Enabled = true;
            bedit.Enabled = true;

            // Check if the click is on a valid row
            if (e.RowIndex >= 0 && e.RowIndex < dgvdatacollector.Rows.Count)
            {
                var row = dgvdatacollector.Rows[e.RowIndex];

                // Load data into textboxes
                cbarea.SelectedItem = row.Cells["AreaRoute"].Value?.ToString();

                // Clear the listbox before populating it
                listarea_routes.Items.Clear();

                // Add selected area route to the listbox
                string selectedAreaRoute = row.Cells["AreaRoute"].Value?.ToString();
                if (!string.IsNullOrEmpty(selectedAreaRoute))
                {
                    listarea_routes.Items.Add(selectedAreaRoute);
                }

                var contactInfo = row.Cells["ContactInfo"].Value?.ToString();
                if (!string.IsNullOrEmpty(contactInfo))
                {
                    var contactLines = contactInfo.Split(new[] { '\n' }, StringSplitOptions.None);
                    if (contactLines.Length >= 8) // Adjusted to match the expected number of fields
                    {
                        tname.Text = contactLines[0]; // Name
                        taddress.Text = contactLines[1]; // Address
                        tcontactno.Text = contactLines[2]; // Contact Number
                        tcontactnoalt.Text = contactLines[3]; // Alternate Contact Number
                        temail.Text = contactLines[4]; // Email
                        trole.Text = contactLines[5]; // Role
                        dtdateemp.Text = contactLines[6]; // Employment Date
                        tbankaccountno.Text = contactLines[7]; // Bank Account
                    }
                }

                var idInfo = row.Cells["IDNumber"].Value?.ToString();
                if (!string.IsNullOrEmpty(idInfo))
                {
                    var idLines = idInfo.Split(new[] { '\n' }, StringSplitOptions.None);
                    if (idLines.Length >= 2)
                    {
                        tidno.Text = idLines[0]; // Generated ID Number
                        trecbookno.Text = idLines[1]; // Receipt Book Number
                    }
                }

                // Set employment status
                cbempstatus.SelectedItem = row.Cells["EmploymentStatus"].Value?.ToString();

                // Pass the IDNumber to the forms for attachments
                frm_home_ADMIN_collectors_upload.StoredAccountID = tidno.Text;
                frm_home_ADMIN_collector_attachments.StoredAccountID = tidno.Text;
            }
        }


        private void bedit_Click(object sender, EventArgs e)
        {
            EnableElements();
            cbarea.Focus(); // Focus on the area selection
        }

        private void bcreate_Click(object sender, EventArgs e)
        {
            bcreate.Enabled = true;
            cbarea.Enabled = true;
            bremoveroute.Enabled = true;
            EnableElements();
            cbarea.Focus();

            ClearForm();

            badd.Enabled = true;
        }

        private void brefresh_Click(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
        }

        private void bremoveroute_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the listarea_routes
            if (listarea_routes.SelectedItem != null)
            {
                // Remove the selected item from the listbox
                listarea_routes.Items.Remove(listarea_routes.SelectedItem);
            }
            else
            {
                // Display a message if no item is selected
                MessageBox.Show("Please select an item to remove.");
            }
        }

        private void tname_Enter(object sender, EventArgs e)
        {
            GenerateCollectorID();
        }
    }
}
