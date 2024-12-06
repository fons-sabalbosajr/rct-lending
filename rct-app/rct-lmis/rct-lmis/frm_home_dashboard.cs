using DnsClient.Protocol;
using DocumentFormat.OpenXml.Wordprocessing;
using LiveCharts.Wpf;
using LiveCharts;
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


            tdues.Interval = 24 * 60 * 60 * 1000;
            tdues.Start();

            
            dgvdata_client.ClearSelection();
            dgvcollectionsnew.ClearSelection();


            // Initialize DataGridView Columns
            InitializeDataGridViewColumns();
            InitializePendingLoansColumns();
        }

        private void InitializeDataGridViewColumns()
        {
            // Add columns to the DataGridView programmatically
            dgvupcomingpayments.Columns.Add("LoanClient", "Loan No - Client Name");
            dgvupcomingpayments.Columns.Add("AmountDueDueDate", "Amount Due - Due Date");
            dgvupcomingpayments.Columns.Add("Description", "Description");
        }

        private void frm_home_dashboard_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            PopulateDataGridView();

            InitializedBulletinView();
            PopulateBulletinView();

            LoadClientTotal();
            PopulateClientsView();

            CountUpdatedLoans();
            PopulatePendingLoansView();

            LoadCollectionTotal();
            PopulateCollectionsView();

            LoadDueLoanTotal();
            PopulateUpcomingPaymentsView();

            PopulateLoanGrowthChart();

        }

        private void PopulateLoanGrowthChart()
        {
            try
            {
                // Initialize MongoDB connection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Fetch all loan data
                var loans = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // Aggregate loan amounts by month (using Date_Encoded)
                var monthlyLoanData = loans
                    .Where(loan => loan.Contains("Date_Encoded") && !string.IsNullOrEmpty(loan["Date_Encoded"].ToString()))
                    .GroupBy(loan =>
                    {
                        var date = DateTime.Parse(loan["Date_Encoded"].ToString());
                        return new DateTime(date.Year, date.Month, 1); // Group by Year-Month
                    })
                    .Select(group => new
                    {
                        Month = group.Key,
                        TotalLoanAmount = group.Sum(loan =>
                        {
                            var amountStr = loan.Contains("LoanAmount") ? loan["LoanAmount"].ToString() : "₱0.00";
                            return decimal.TryParse(amountStr.Replace("₱", "").Replace(",", ""), out decimal loanAmount)
                                ? loanAmount
                                : 0;
                        })
                    })
                    .OrderBy(data => data.Month) // Ensure the data is sorted by month
                    .ToList();

                // Prepare data for the Cartesian chart
                var months = monthlyLoanData.Select(data => data.Month.ToString("MMM yyyy")).ToList();
                var loanAmounts = monthlyLoanData.Select(data => (double)data.TotalLoanAmount).ToList();

                // Bind data to the Cartesian chart
                chloangrowth.Series.Clear();
                chloangrowth.Series.Add(new LineSeries
                {
                    Title = "Loan Growth",
                    Values = new ChartValues<double>(loanAmounts),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10
                });

                // Configure chart labels
                chloangrowth.AxisX.Clear();
                chloangrowth.AxisX.Add(new Axis
                {
                    Title = "Month",
                    Labels = months
                });

                chloangrowth.AxisY.Clear();
                chloangrowth.AxisY.Add(new Axis
                {
                    Title = "Loan Amount (₱)",
                    LabelFormatter = value => $"₱{value:N2}"
                });

                chloangrowth.LegendLocation = LegendLocation.Right;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating loan growth chart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializePendingLoansColumns()
        {
            // Clear any existing columns
            dgvpendingloans.Columns.Clear();

            // Add columns with word wrapping enabled
            var loanInfoColumn = new DataGridViewTextBoxColumn
            {
                Name = "LoanAccountInfo",
                HeaderText = "Loan Account Info",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    WrapMode = DataGridViewTriState.True
                }
            };

            var remarksColumn = new DataGridViewTextBoxColumn
            {
                Name = "Remarks",
                HeaderText = "Remarks",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    WrapMode = DataGridViewTriState.True
                }
            };

            // Add columns to the DataGridView
            dgvpendingloans.Columns.Add(loanInfoColumn);
            dgvpendingloans.Columns.Add(remarksColumn);

            // Enable row auto-sizing
            //dgvpendingloans.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            dgvbulletin.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

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
                    System.Drawing.Color statusColor = user.IsLoggedIn ? System.Drawing.Color.Green : System.Drawing.Color.Gray;

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
            dgvdata_client.Columns.Clear();
            dgvdata_client.ClearSelection();

            // Add columns
            dgvdata_client.Columns.Add(new DataGridViewTextBoxColumn { Name = "ClientInfo", HeaderText = "Client Information" });
            dgvdata_client.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoanStatus", HeaderText = "Loan Status" });

            // Adjust column widths as needed
            dgvdata_client.Columns["ClientInfo"].Width = 400;
            dgvdata_client.Columns["LoanStatus"].Width = 100;

            dgvdata_client.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvdata_client.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
        }

        private void PopulateClientsView()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                // Fetch the most recent top 4 approved loans sorted by StartPaymentDate
                var clients = collection.Find(Builders<BsonDocument>.Filter.Empty)
                                        .Sort(Builders<BsonDocument>.Sort.Descending("StartPaymentDate"))
                                        .Limit(4) // Limit to the top 4 clients
                                        .ToList();

                dgvdata_client.Rows.Clear();
                dgvdata_client.ClearSelection();

                // Populate DataGridView with client information
                foreach (var client in clients)
                {
                    // Construct the client's full name
                    string fullName = $"{client["FirstName"]} {(client.Contains("MiddleName") ? client["MiddleName"].ToString() : "")} {client["LastName"]}".Trim();

                    // Construct the address
                    string address = $"{(client.Contains("Street") ? client["Street"].ToString() : "")}, " +
                                     $"{client["Barangay"]}, {client["City"]}, {(client.Contains("Province") ? client["Province"].ToString() : "")}".Trim();

                    // Extract contact number if available
                    string contactNumber = client.Contains("CP") ? client["CP"].ToString() : "N/A";

                    // Extract and format loan amount
                    string loanAmount = client.Contains("LoanAmount") ? client["LoanAmount"].ToString() : "N/A";

                    // Extract the start payment date
                    string startPaymentDate = client.Contains("StartPaymentDate") ? client["StartPaymentDate"].ToString() : "N/A";

                    // Construct client info string
                    string clientInfo = $"{fullName}\n{address}\nContact Number: {contactNumber}\nLoan Amount: {loanAmount}\nStart Payment Date: {startPaymentDate}".Trim();

                    // Get loan status
                    string loanStatus = client.Contains("LoanStatus") ? client["LoanStatus"].ToString() : "N/A";

                    // Add row to the DataGridView
                    dgvdata_client.Rows.Add(clientInfo, loanStatus);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error loading clients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void CountUpdatedLoans()
        {
            try
            {
                // Get the MongoDB database and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define the filter to find documents where LoanStatus is UPDATED
                var filter = Builders<BsonDocument>.Filter.Eq("LoanStatus", "UPDATED");

                // Count the number of documents matching the filter
                long updatedLoanCount = collection.CountDocuments(filter);

                // Update the loan.Text with the count of UPDATED loans
                lloantotal.Text = updatedLoanCount.ToString();
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                MessageBox.Show($"Error counting UPDATED loans: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulatePendingLoansView()
        {
            try
            {
                // Initialize the columns first
                InitializePendingLoansColumns();

                // Hide the label if no pending loans exist
                lnorecordpending.Visible = false;

                // Get the MongoDB database and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Fetch all documents from the loan_disbursed collection
                var pendingLoans = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // List to hold the pending loan data temporarily
                List<LoanPaymentDetails> pendingLoanDetailsList = new List<LoanPaymentDetails>();

                // Loop through each loan to detect missing data and prepare it for display
                foreach (var loan in pendingLoans)
                {
                    // Retrieve Loan No and Client Name
                    string loanNo = loan.Contains("LoanNo") ? loan["LoanNo"].ToString() : "N/A";
                    string clientName = loan.Contains("FirstName") && loan.Contains("LastName")
                        ? $"{loan["FirstName"]} {loan["LastName"]}"
                        : "N/A";

                    // If either LoanNo or ClientName is "N/A", skip this loan
                    if (loanNo == "N/A" || clientName == "N/A")
                        continue;

                    // Merged Loan No and Client Name with newline for separation
                    string mergedLoanClient = $"{loanNo}\n{clientName}";

                    // Dynamically check for missing fields
                    List<string> missingFields = new List<string>();

                    foreach (var field in new[] { "PaymentMode", "StartPaymentDate", "MaturityDate", "Penalty", "LoanInterest" })
                    {
                        if (!loan.Contains(field) || string.IsNullOrEmpty(loan[field]?.ToString()))
                        {
                            missingFields.Add(field.Replace("PaymentMode", "Payment Mode")
                                                   .Replace("StartPaymentDate", "Start Payment Date")
                                                   .Replace("MaturityDate", "Maturity Date")
                                                   .Replace("Penalty", "Penalty")
                                                   .Replace("LoanInterest", "Loan Interest"));
                        }
                    }

                    // If no fields are missing, skip this loan
                    if (missingFields.Count == 0)
                        continue;

                    // Prepare a description with missing fields
                    string description = "Missing:\n" + string.Join("\n- ", missingFields);

                    // Add to pending loan details
                    pendingLoanDetailsList.Add(new LoanPaymentDetails
                    {
                        LoanClient = mergedLoanClient,
                        Description = description // Dynamic description indicating missing data
                    });
                }

                // Sort the pending loans by LoanNo
                pendingLoanDetailsList = pendingLoanDetailsList
                    .OrderBy(loan => loan.LoanClient)
                    .ToList();

                // Clear existing rows in the DataGridView
                dgvpendingloans.Rows.Clear();
                dgvpendingloans.ClearSelection();

                // Add sorted rows to the DataGridView
                foreach (var loanDetail in pendingLoanDetailsList)
                {
                    dgvpendingloans.Rows.Add(loanDetail.LoanClient, loanDetail.Description);
                }

                // Show label if no pending loans are found
                lnorecordpending.Visible = pendingLoanDetailsList.Count == 0;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
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
                dgvcollectionsnew.ClearSelection();

                // Query to get all loan collections and sort them by CollectionDate (newest to oldest)
                var loanCollections = collection.Find(Builders<BsonDocument>.Filter.Empty)
                                                .Sort(Builders<BsonDocument>.Sort.Descending("CollectionDate"))
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
                        DateTime receivedDate = collectionDoc["DateReceived"].ToUniversalTime();

                        // Status based on the collection date and received date comparison
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

                // Count how many loans are due today or in the future
                int dueCount = dueLoans.Count(loan =>
                {
                    if (loan.Contains("StartPaymentDate") && DateTime.TryParse(loan["StartPaymentDate"].ToString(), out DateTime startPaymentDate))
                    {
                        string paymentMode = loan.Contains("PaymentMode") ? loan["PaymentMode"].AsString.ToUpper() : string.Empty;

                        DateTime dueDate = DateTime.MinValue;

                        // Calculate the due date based on PaymentMode
                        switch (paymentMode)
                        {
                            case "DAILY":
                                // Daily payments are due today
                                dueDate = startPaymentDate;
                                break;
                            case "WEEKLY":
                                // Weekly payments are due 3 days before the start payment date
                                dueDate = startPaymentDate.AddDays(-3);
                                break;
                            case "SEMI-MONTHLY":
                                // Semi-monthly payments are due 1 week before the start payment date
                                dueDate = startPaymentDate.AddDays(-7);
                                break;
                            case "MONTHLY":
                                // Monthly payments are due 15 days before the start payment date
                                dueDate = startPaymentDate.AddDays(-15);
                                break;
                            default:
                                break;
                        }

                        // Check if the loan is due today or in the future
                        return dueDate <= today;
                    }
                    return false;
                });

                // Display the due count in the label
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
                lnorecordpay.Visible = false;
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Get today's date for comparison
                DateTime today = DateTime.Today;

                // Fetch all documents from the loan_disbursed collection
                var dueLoans = collection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // List to hold the loan data temporarily
                List<LoanPaymentDetails> loanDetailsList = new List<LoanPaymentDetails>();

                // Populate the loanDetailsList with loan data
                foreach (var loan in dueLoans)
                {
                    // Retrieve Loan No and Client Name
                    string loanNo = loan.Contains("LoanNo") ? loan["LoanNo"].ToString() : "N/A";
                    string clientName = loan.Contains("FirstName") && loan.Contains("LastName")
                        ? $"{loan["FirstName"]} {loan["LastName"]}"
                        : "N/A";

                    // If either LoanNo or ClientName is "N/A", skip this loan
                    if (loanNo == "N/A" || clientName == "N/A")
                        continue;

                    // Use newline to separate Loan No and Client Name in the same cell
                    string mergedLoanClient = $"{loanNo}\n{clientName}";

                    // Calculate the Due Date based on the PaymentMode
                    DateTime? dueDate = null;
                    string paymentMode = loan.Contains("PaymentMode") ? loan["PaymentMode"].ToString() : string.Empty;

                    if (loan.Contains("StartPaymentDate") && DateTime.TryParse(loan["StartPaymentDate"].ToString(), out DateTime startPaymentDate))
                    {
                        switch (paymentMode.ToUpper())
                        {
                            case "DAILY":
                                dueDate = startPaymentDate.AddDays(1);
                                break;
                            case "WEEKLY":
                                dueDate = startPaymentDate.AddDays(3); // 3 days before
                                break;
                            case "SEMI-MONTHLY":
                                dueDate = startPaymentDate.AddDays(7); // 7 days before
                                break;
                            case "MONTHLY":
                                dueDate = startPaymentDate.AddDays(15); // 15 days before
                                break;
                        }
                    }

                    // If the dueDate is not available, skip this loan
                    if (!dueDate.HasValue)
                        continue;

                    // Prepare the merged Amount Due and Due Date
                    string amountDue = loan.Contains("LoanAmortization") ? loan["LoanAmortization"].ToString() : "N/A";
                    string dueDateStr = dueDate.HasValue ? dueDate.Value.ToString("MM/dd/yyyy") : "N/A";

                    // If either AmountDue or DueDate is "N/A", skip this loan
                    if (amountDue == "N/A" || dueDateStr == "N/A")
                        continue;

                    // Calculate the number of days due
                    string dueDays = "N/A";
                    int daysDifference = 0;
                    if (dueDate.HasValue)
                    {
                        daysDifference = (today - dueDate.Value).Days;

                        // If the due date is in the future
                        if (daysDifference < 0)
                        {
                            dueDays = $"{Math.Abs(daysDifference)} days until due";
                        }
                        // If the due date is in the past
                        else if (daysDifference > 0)
                        {
                            dueDays = $"{daysDifference} days overdue";
                        }
                        // If the due date is today
                        else
                        {
                            dueDays = "Due today";
                        }
                    }

                    // Use newline to separate Amount Due and Due Date in the same cell
                    string mergedAmountDueDate = $"{amountDue}\n{dueDateStr}";

                    // Description: merged with due days calculation
                    string description = $"Client is {dueDays}";

                    // Add the loan details to the list
                    loanDetailsList.Add(new LoanPaymentDetails
                    {
                        LoanClient = mergedLoanClient,
                        AmountDueDueDate = mergedAmountDueDate,
                        Description = description,
                        DueDays = daysDifference
                    });
                }

                // Sort the loanDetailsList by DueDays (recent overdue and due today first)
                loanDetailsList = loanDetailsList
                    .OrderBy(loan => loan.DueDays)
                    .ToList();

                // Clear existing rows in the DataGridView
                dgvupcomingpayments.Rows.Clear();
                dgvupcomingpayments.ClearSelection();

                // Add sorted rows to the DataGridView
                foreach (var loanDetail in loanDetailsList)
                {
                    dgvupcomingpayments.Rows.Add(loanDetail.LoanClient, loanDetail.AmountDueDueDate, loanDetail.Description);
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
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (status == "Offline")
                    {
                        e.CellStyle.ForeColor = System.Drawing.Color.Gray;
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
                cell.Style.Font = new System.Drawing.Font("Segoe UI", 8, FontStyle.Regular); // Adjust font family, size, and style as needed
            }
        }

        private void dgvbulletin_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgvbulletin.ClearSelection();
        }

        private void dgvcollectionsnew_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           //dgvcollectionsnew.ClearSelection();
        }

        private void dgvdata_client_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgvdata_client.ClearSelection();
        }

        private void dgvupcomingpayments_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgvupcomingpayments.ClearSelection();
        }

        private void tdues_Tick(object sender, EventArgs e)
        {
            PopulateUpcomingPaymentsView();
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

    public class LoanPaymentDetails
    {
        public string LoanClient { get; set; }
        public string AmountDueDueDate { get; set; }
        public string Description { get; set; }
        public int DueDays { get; set; } // Used for sorting
    }

}
