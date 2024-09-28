using DnsClient.Protocol;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using rct_lmis.ADMIN_SECTION;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_dashboard : Form
    {
        private string _username;

        public frm_home_dashboard(string username)
        {
            InitializeComponent();

            InitializeClientsView();
            _username = username;

            dgvusersonline.ClearSelection();
            dgvbulletin.ClearSelection();

            dgvclients.ClearSelection();
            dgvbulletin.ClearSelection();
            dgvcollectionsnew.ClearSelection();
        }

        private void frm_home_dashboard_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            PopulateDataGridView();

            InitializedBulletinView();
            PopulateBulletinView();

            LoadClientTotal();
            PopulateClientsView();

            //LoadLoanTotal();
            //PopulatePendingLoansView();

            LoadCollectionTotal();
            PopulateCollectionsView();

            //LoadDueLoanTotal();
            //PopulateUpcomingPaymentsView();
        }

        private void InitializeDataGridView()
        {
            // Clear existing columns if any
            dgvusersonline.Columns.Clear();
            dgvusersonline.ClearSelection();

            // Add columns
            dgvusersonline.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name" });
            dgvusersonline.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status" });

            // Adjust column widths and other properties as needed
            dgvusersonline.Columns["Name"].Width = 175; // Adjust width for user's name column
            dgvusersonline.Columns["Status"].Width = 100; // Adjust width for status column

            // Show headers
            dgvusersonline.ColumnHeadersVisible = false;

            dgvusersonline.CellFormatting += dgvusersonline_CellFormatting;
            dgvusersonline.DataBindingComplete += dgvusersonline_DataBindingComplete;

        }

        private void InitializedBulletinView()
        {
            // Clear existing columns if any
            dgvbulletin.Columns.Clear();
            dgvbulletin.ClearSelection();

            // Add columns
            dgvbulletin.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Date" });
            dgvbulletin.Columns.Add(new DataGridViewTextBoxColumn { Name = "SubjectContent", HeaderText = "Subject and Content" });
            dgvbulletin.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ViewButton",
                HeaderText = "",
                Text = "View",
                UseColumnTextForButtonValue = true
            });

            // Adjust column widths and other properties as needed
            dgvbulletin.Columns["Date"].Width = 100;
            dgvbulletin.Columns["SubjectContent"].Width = 300; // Wider to accommodate content
            dgvbulletin.Columns["ViewButton"].Width = 50;

            dgvbulletin.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgvbulletin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            dgvbulletin.CellFormatting += dgvusersonline_CellFormatting;
            dgvbulletin.DataBindingComplete += dgvusersonline_DataBindingComplete;
        }

        private void PopulateBulletinView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<Announcement>("announcements");

                // Fetch all announcements ordered by date
                var announcements = collection.Find(Builders<Announcement>.Filter.Empty)
                                              .SortByDescending(a => a.PostedDate)
                                              .ToList();

                // Clear existing rows
                dgvbulletin.Rows.Clear();
                dgvbulletin.ClearSelection();

                // Populate DataGridView with announcements
                foreach (var announcement in announcements)
                {
                    // Combine Subject and Content into a single string with line break
                    string subjectAndContent = $"{announcement.Title}\n{announcement.Content}";

                    dgvbulletin.Rows.Add(announcement.PostedDate.ToString(), subjectAndContent);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateDataGridView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loginStatusCollection = database.GetCollection<LoginStatus>("login-status");

                // Get all users with their last login status
                var users = loginStatusCollection.AsQueryable()
                    .GroupBy(ls => ls.UserId)
                    .Select(group => group.OrderByDescending(ls => ls.LoginTime).FirstOrDefault())
                    .ToList();

                // Clear existing rows
                dgvusersonline.Rows.Clear();
                dgvusersonline.ClearSelection();

                // Create a list to hold the formatted data
                var userData = new List<UserData>();

                foreach (var user in users)
                {
                    bool isLoggedIn = user.IsLoggedIn; // Assuming IsLoggedIn property indicates current online status

                    userData.Add(new UserData
                    {
                        Name = user.Name,
                        IsLoggedIn = isLoggedIn
                    });
                }

                // Sort the userData list so that online users are at the top
                var sortedUserData = userData.OrderByDescending(u => u.IsLoggedIn).ToList();

                // Add rows to DataGridView
                foreach (var user in sortedUserData)
                {
                    string statusText = user.IsLoggedIn ? "Online" : "Offline";
                    Color statusColor = user.IsLoggedIn ? Color.Green : Color.Gray;

                    // Add row to DataGridView
                    int rowIndex = dgvusersonline.Rows.Add(user.Name, statusText);
                    dgvusersonline.Rows[rowIndex].Cells["Status"].Style.ForeColor = statusColor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching login status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClientTotal()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                // Get the total count of documents in the loan_approved collection
                var totalCount = collection.CountDocuments(Builders<BsonDocument>.Filter.Empty);

                // Display the total count in the lclienttotal label
                lclienttotal.Text = totalCount.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading client total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeClientsView()
        {
            // Clear existing columns if any
            dgvclients.Columns.Clear();

            // Add columns
            dgvclients.Columns.Add(new DataGridViewTextBoxColumn { Name = "ClientInfo", HeaderText = "Client Information" });
            dgvclients.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoanStatus", HeaderText = "Loan Status" });

            // Adjust column widths as needed
            dgvclients.Columns["ClientInfo"].Width = 400;
            dgvclients.Columns["LoanStatus"].Width = 100;

            dgvclients.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvclients.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        }

        private void PopulateClientsView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                // Fetch all approved loans
                var clients = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // Clear existing rows in the DataGridView
                dgvclients.Rows.Clear();

                // Populate DataGridView with client information
                foreach (var client in clients)
                {
                    string fullName = $"{client["FirstName"]} {client["MiddleName"]} {client["LastName"]}".Trim();
                    string address = $"{client["Street"]}, {client["Barangay"]}, {client["City"]}, {client["Province"]}".Trim();
                    string contactNumber = client.Contains("CP") ? client["CP"].ToString() : "";
                    string loanAmount = client.Contains("PrincipalAmount") ? client["PrincipalAmount"].ToString() : "";
                    string startPaymentDate = client.Contains("PaymentStartDate") ? client["PaymentStartDate"].ToString() : "";

                    // Merge information into a single string
                    string clientInfo = $"{fullName}\n{address}\n{contactNumber}\nLoan Amount: {loanAmount}\nStart Payment Date: {startPaymentDate}".Trim();

                    // Get loan status
                    string loanStatus = client.Contains("LoanStatus") ? client["LoanStatus"].ToString() : "";

                    // Add row to the DataGridView
                    dgvclients.Rows.Add(clientInfo, loanStatus);
                }
                dgvclients.DataBindingComplete += dgvclients_DataBindingComplete;

            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading clients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoanTotal()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Get the total count of documents in the loan_disbursed collection
                var totalCount = collection.CountDocuments(Builders<BsonDocument>.Filter.Empty);

                // Display the total count in the lloantotal label
                lloantotal.Text = totalCount.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading loan total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PopulatePendingLoansView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_application");

                // Fetch pending loan applications
                var pendingLoans = collection.Find(Builders<BsonDocument>.Filter.Eq("LoanStatus", "Pending")) // Assuming LoanStatus indicates if it's pending
                                             .SortByDescending(a => a["ApplicationDate"]) // Sort by application date
                                             .ToList();

                // Clear existing rows in the DataGridView
                dgvpendingloans.Rows.Clear();

                // Populate DataGridView with pending loan information
                foreach (var loan in pendingLoans)
                {
                    string fullName = $"{loan["FirstName"]} {loan["MiddleName"]} {loan["LastName"]} {loan["SuffixName"]}".Trim();
                    string applicationDate = loan.Contains("ApplicationDate") ? loan["ApplicationDate"].ToString() : "";

                    // Add row to the DataGridView
                    dgvpendingloans.Rows.Add(fullName, applicationDate);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading pending loans: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCollectionTotal()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_collections");

                // Get the total count of distinct LoanID in the loan_collections collection
                var distinctLoanIDs = collection.Distinct<string>("LoanID", Builders<BsonDocument>.Filter.Empty).ToList();
                int totalCount = distinctLoanIDs.Count;

                // Display the total count in the lcollectiontotal label
                lcollectiontotal.Text = totalCount.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading collection total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateCollectionsView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_collections");

                // Ensure the DataGridView has the necessary columns
                if (dgvcollectionsnew.Columns.Count == 0)
                {
                    dgvcollectionsnew.Columns.Add("ClientInfo", "Client Information");
                    dgvcollectionsnew.Columns.Add("LoanAmount", "Loan Amount");
                    dgvcollectionsnew.Columns.Add("AmountPaid", "Amount Paid");
                    dgvcollectionsnew.Columns.Add("CollectorInfo", "Collection Information");
                }

                // Clear existing rows before loading new data
                dgvcollectionsnew.Rows.Clear();
                dgvcollectionsnew.DataBindingComplete += dgvcollectionsnew_DataBindingComplete;

                // Query to get all loan collections
                var loanCollections = collection.Find(Builders<BsonDocument>.Filter.Empty)
                                                .ToList();

                // Populate DataGridView with collection information
                foreach (var collectionDoc in loanCollections)
                {
                    // Client Information
                    string collectionDate = collectionDoc.Contains("CollectionDate") ?
                        collectionDoc["CollectionDate"].ToUniversalTime().ToString("yyyy-MM-dd") : "";
                    string accountId = collectionDoc.Contains("AccountId") ?
                        collectionDoc["AccountId"].AsString : "";
                    string name = collectionDoc.Contains("Name") ?
                        collectionDoc["Name"].AsString : "";

                    // Concatenate Client Information fields into a single string with new lines
                    string clientInfo = $"Col. Date: {collectionDate}\nCol. No.: {accountId}\nName: {name}";

                    // Loan Information
                    string loanAmount = collectionDoc.Contains("LoanAmount") ?
                        ((double)collectionDoc["LoanAmount"].AsDecimal128).ToString("F2") : "0.00";

                    // Payment Information (optional, can be omitted if not needed)
                    string amountPaid = collectionDoc.Contains("ActualCollection") ?
                        ((double)collectionDoc["ActualCollection"].AsDecimal128).ToString("F2") : "0.00";

                    // Collection Information
                    string collector = collectionDoc.Contains("Collector") ?
                        collectionDoc["Collector"].AsString : "";
                    string area = collectionDoc.Contains("Area") ?
                        collectionDoc["Area"].AsString : "";

                    // Determine Collection Status
                    string collectionStatus = "Over Due"; // Default status
                    if (collectionDoc.Contains("CollectionDate") && collectionDoc.Contains("DateReceived"))
                    {
                        DateTime collDate = collectionDoc["CollectionDate"].ToUniversalTime();
                        DateTime receivedDate = DateTime.Parse(collectionDoc["DateReceived"].ToUniversalTime().ToString("MM/dd/yyyy"));

                        // Add logic to determine the status based on your criteria here
                        collectionStatus = collDate.Date == receivedDate.Date ? "Paid on Time" : "Over Due";
                    }

                    // Concatenate Collection Information fields into a single string with new lines
                    string collectionInfo = $"Collector: {collector}\nArea Route: {area}\nCollection Status: {collectionStatus}";

                    // Add the concatenated information to the DataGridView
                    dgvcollectionsnew.Rows.Add(clientInfo, loanAmount, amountPaid, collectionInfo);
                }

                // Enable word wrapping for the DataGridView
                foreach (DataGridViewColumn column in dgvcollectionsnew.Columns)
                {
                    column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }

                // Set column widths and other properties if needed
                dgvcollectionsnew.Columns[0].Width = 300; // Client Information
                dgvcollectionsnew.Columns[1].Width = 200; // Loan Amount
                dgvcollectionsnew.Columns[2].Width = 200; // Amount Paid
                dgvcollectionsnew.Columns[3].Width = 200; // Collection Information

                // Adjust row heights to fit content
                dgvcollectionsnew.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading collections: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadDueLoanTotal()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Get today's date for comparison
                DateTime today = DateTime.Today;

                // Fetch all documents from the loan_disbursed collection
                var dueLoans = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // Count how many PaymentStartDate entries are due today or in the future
                int dueCount = dueLoans.Count(loan =>
                {
                    if (loan.Contains("PaymentStartDate") && DateTime.TryParse(loan["PaymentStartDate"].ToString(), out DateTime paymentStartDate))
                    {
                        return paymentStartDate.Date >= today; // Check if the payment date is today or in the future
                    }
                    return false;
                });

                // Display the due count in the ldueloantotal label
                ldueloantotal.Text = dueCount.ToString();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading due loan total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateUpcomingPaymentsView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_collections");

                // Get upcoming payments sorted by the collection date (since no "NextPaymentDate" is in the sample data)
                var upcomingPayments = collection.Find(Builders<BsonDocument>.Filter.Empty)
                                                 .SortBy(c => c["CollectionDate"]) // Using CollectionDate for sorting
                                                 .ToList();

                // Clear existing rows in the DataGridView
                dgvupcomingpayments.Rows.Clear();

                // Populate DataGridView with upcoming payment information
                foreach (var payment in upcomingPayments)
                {
                    // Retrieve data from BsonDocument and handle missing fields
                    string loanId = payment.Contains("LoanID") ? payment["LoanID"].AsString : "N/A";
                    string amountDue = payment.Contains("LoantoPay") ? payment["LoantoPay"].ToString() : "0.00"; // Adjust field name
                    string collectionDate = payment.Contains("CollectionDate")
                                            ? payment["CollectionDate"].ToLocalTime().ToString("yyyy-MM-dd")
                                            : "N/A";
                    string collector = payment.Contains("Collector") ? payment["Collector"].AsString : "Unknown"; // Adjust field name

                    // Add row to the DataGridView
                    dgvupcomingpayments.Rows.Add(loanId, amountDue, collectionDate, collector);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading upcoming payments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvusersonline_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvusersonline.Columns["Status"].Index)
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Online")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else if (status == "Offline")
                    {
                        e.CellStyle.ForeColor = Color.Gray;
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvusersonline_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvusersonline.ClearSelection();
        }

        private void dgvusersonline_MouseLeave(object sender, EventArgs e)
        {
            dgvusersonline.ClearSelection();
        }


        private void dgvbulletin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "ViewButton" column
            if (e.ColumnIndex == dgvbulletin.Columns["ViewButton"].Index && e.RowIndex >= 0)
            {
                // Get the selected announcement details
                var announcement = dgvbulletin.Rows[e.RowIndex];
                string date = announcement.Cells["Date"].Value.ToString();
                string subject = announcement.Cells["SubjectContent"].Value.ToString().Split('\n')[0]; // Get the subject
                string content = announcement.Cells["SubjectContent"].Value.ToString().Split('\n')[1]; // Get the content

                // Display the details or open a new form, etc.
                MessageBox.Show($"Date: {date}\nSubject: {subject}\n\nContent: {content}", "Announcement Details", MessageBoxButtons.OK);
            }
        }

        private void dgvbulletin_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set padding and style for the View button
            if (e.ColumnIndex == dgvbulletin.Columns["ViewButton"].Index && e.RowIndex >= 0)
            {
                var cell = dgvbulletin[e.ColumnIndex, e.RowIndex] as DataGridViewButtonCell;

                // Set padding (in pixels)
                var padding = new Padding(10, 15, 10, 15); // Adjust padding as needed
                cell.Style.Padding = padding;

                // Set font size and style
                cell.Style.Font = new Font("Segoe UI", 8, FontStyle.Regular); // Adjust font family, size, and style as needed
            }
        }

        private void dgvbulletin_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvbulletin.ClearSelection();
        }

        private void dgvclients_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvclients.ClearSelection();
        }

        private void dgvcollectionsnew_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvcollectionsnew.ClearSelection();
        }
    }

    public class UserData
    {
        public string Name { get; set; }
        public bool IsLoggedIn { get; set; }
    }

    public class LoginStatus
    {
        [BsonId]
        public ObjectId Id { get; set; } // Maps to the '_id' field in MongoDB
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; } // Changed to nullable to allow for null values
    }
}
