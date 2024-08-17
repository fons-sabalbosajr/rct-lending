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
    public partial class frm_home_ADMIN_aploans : Form
    {
        public frm_home_ADMIN_aploans()
        {
            InitializeComponent();
        }

        LoadingFunction load = new LoadingFunction();

        private void LoadApprovedLoans()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

                // Convert DateTimePicker value to a DateTime for filtering
                var selectedDate = dtdateApproved.Value.Date;

                // Create a filter based on selected status and date
                var filterBuilder = Builders<BsonDocument>.Filter;
                var filter = filterBuilder.Empty;

                if (cbstatus.SelectedItem != null && cbstatus.SelectedItem.ToString() != "All")
                {
                    var selectedStatus = cbstatus.SelectedItem.ToString();
                    filter &= filterBuilder.Eq("LoanStatus", selectedStatus);
                }

                if (dtdateApproved.Value != null)
                {
                    // Filter for loans with ApprovalDate on the selected date
                    var startOfDay = selectedDate;
                    var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // End of the selected day

                    filter &= filterBuilder.Gte("ApprovalDate", startOfDay) & filterBuilder.Lte("ApprovalDate", endOfDay);
                }

                // Fetch data from MongoDB
                var approvedLoans = loanApprovedCollection.Find(filter).ToList();

                // Clear existing columns
                dgvloandata.Columns.Clear();

                // Add columns
                dgvloandata.Columns.Add("ClientNumber", "Client Number");
                var nameColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Name",
                    HeaderText = "Name",
                    Width = 200 // Set the width for the Name column
                };
                dgvloandata.Columns.Add(nameColumn);

                var addressColumn = new DataGridViewTextBoxColumn
                {
                    Name = "Address",
                    HeaderText = "Address"
                };
                dgvloandata.Columns.Add(addressColumn);

                var loanTypeColumn = new DataGridViewTextBoxColumn
                {
                    Name = "LoanType",
                    HeaderText = "Loan Type",
                    Width = 150 // Set the width for the Loan Type column
                };
                dgvloandata.Columns.Add(loanTypeColumn);

                var dateApprovedColumn = new DataGridViewTextBoxColumn
                {
                    Name = "DateApproved",
                    HeaderText = "Date Approved",
                    Width = 120 // Set the width for the Date Approved column
                };
                dgvloandata.Columns.Add(dateApprovedColumn);

                dgvloandata.Columns.Add("Status", "Status");
                dgvloandata.Columns.Add("Docs", "Documents");

                // Add button column
                var buttonColumn = new DataGridViewButtonColumn
                {
                    Name = "ViewDetails",
                    HeaderText = "Action",
                    Text = "View Details",
                    UseColumnTextForButtonValue = true,
                    Width = 120,
                    DefaultCellStyle = { Padding = new Padding(10) }
                };
                dgvloandata.Columns.Add(buttonColumn);

                // Populate rows
                foreach (var loan in approvedLoans)
                {
                    var name = $"{loan.GetValue("FirstName", "")} {loan.GetValue("LastName", "")}";
                    var address = $"{loan.GetValue("Street", "")}, {loan.GetValue("Barangay", "")}, {loan.GetValue("City", "")}, {loan.GetValue("Province", "")}";

                    // Format docs to be a single line
                    var docs = loan.GetValue("docs", "").ToString().Replace("\n", " ").Replace("\r", " ").Trim();

                    // Convert ApprovalDate to DateTime and then to MM/dd/yyyy format
                    var approvalDate = loan.GetValue("ApprovalDate", BsonNull.Value);
                    var formattedDate = approvalDate == BsonNull.Value ? "" : ((DateTime)approvalDate).ToString("MM/dd/yyyy");

                    dgvloandata.Rows.Add(
                        loan.GetValue("ClientNumber", ""),
                        name,
                        address,
                        loan.GetValue("LoanType", ""),
                        formattedDate,
                        loan.GetValue("LoanStatus", ""),
                        docs,
                        "View Details"
                    );
                }

                // Align columns
                dgvloandata.Columns["ClientNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["LoanType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["ViewDetails"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvloandata.Columns["Address"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvloandata.Columns["Docs"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // Enable word wrapping
                dgvloandata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvloandata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgvloandata.Columns["Address"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvloandata.Columns["Docs"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // Adjust column widths
                dgvloandata.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvloandata.Columns["LoanType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading approved loans: " + ex.Message);
            }
        }


        private void FilterLoansByDate(DateTime selectedDate)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

                // Convert the selected date to the start and end of the day
                var startOfDay = selectedDate.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                // Create a filter for the ApprovalDate range
                var filter = Builders<BsonDocument>.Filter.Gte("ApprovalDate", startOfDay) &
                             Builders<BsonDocument>.Filter.Lte("ApprovalDate", endOfDay);

                var approvedLoans = loanApprovedCollection.Find(filter).ToList();

                // Clear existing rows
                dgvloandata.Rows.Clear();

                // Populate the DataGridView with filtered data
                foreach (var loan in approvedLoans)
                {
                    var name = $"{loan.GetValue("FirstName", "")} {loan.GetValue("LastName", "")}";
                    var address = $"{loan.GetValue("Street", "")}, {loan.GetValue("Barangay", "")}, {loan.GetValue("City", "")}, {loan.GetValue("Province", "")}";

                    // Format docs to be a single line
                    var docs = loan.GetValue("docs", "").ToString().Replace("\n", " ").Replace("\r", " ").Trim();

                    // Convert ApprovalDate to DateTime and then to MM/dd/yyyy format
                    var approvalDate = loan.GetValue("ApprovalDate", BsonNull.Value);
                    var formattedDate = approvalDate == BsonNull.Value ? "" : ((DateTime)approvalDate).ToString("MM/dd/yyyy");

                    dgvloandata.Rows.Add(
                        loan.GetValue("ClientNumber", ""),
                        name,
                        address,
                        loan.GetValue("LoanType", ""),
                        formattedDate,
                        loan.GetValue("LoanStatus", ""),
                        docs,
                        "View Details"
                    );
                }

                // Align columns
                dgvloandata.Columns["ClientNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["LoanType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["ViewDetails"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvloandata.Columns["Address"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvloandata.Columns["Docs"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // Enable word wrapping
                dgvloandata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvloandata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgvloandata.Columns["Address"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvloandata.Columns["Docs"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // Adjust column widths
                dgvloandata.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvloandata.Columns["LoanType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering loans by date: " + ex.Message);
            }
        }


        private void LoadStatusData()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

                // Get distinct statuses from the loan_approved collection
                var statuses = loanApprovedCollection.Distinct<string>("LoanStatus", new BsonDocument()).ToList();

                // Load statuses into the ComboBox
                cbstatus.Items.Clear();
                cbstatus.Items.Add("All"); // Add an option to show all loans
                cbstatus.Items.AddRange(statuses.ToArray());

                cbstatus.SelectedIndex = 0; // Select "All" by default
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statuses: " + ex.Message);
            }
        }

        private void FilterApprovedLoansByStatus()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

                // Create a filter based on the selected status
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;

                if (cbstatus.SelectedItem.ToString() != "All")
                {
                    filter = Builders<BsonDocument>.Filter.Eq("LoanStatus", cbstatus.SelectedItem.ToString());
                }

                // Retrieve filtered loans
                var filteredLoans = loanApprovedCollection.Find(filter).ToList();

                // Bind the filtered data to the DataGridView
                dgvloandata.DataSource = filteredLoans.Select(loan => new
                {
                    AccountId = loan.GetValue("AccountId", "").AsString,
                    FirstName = loan.GetValue("FirstName", "").AsString,
                    LastName = loan.GetValue("LastName", "").AsString,
                    LoanType = loan.GetValue("LoanType", "").AsString,
                    ApprovalDate = loan.GetValue("ApprovalDate", DateTime.MinValue).ToUniversalTime()
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering approved loans: " + ex.Message);
            }
        }


        private void frm_home_ADMIN_aploans_Load(object sender, EventArgs e)
        {
            //LoadStatusData();
            LoadApprovedLoans();
        }

        private void dtdateApproved_ValueChanged(object sender, EventArgs e)
        {
         
            FilterLoansByDate(dtdateApproved.Value.Date);
        }

        private void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterApprovedLoansByStatus();
        }
    }
}
