using DnsClient.Protocol;
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
    public partial class frm_home_ADMIN_dnloans : Form
    {
        public frm_home_ADMIN_dnloans()
        {
            InitializeComponent();
        }

        private void SetupDataGridView()
        {
            // Add columns
            dgvloandata.Columns.Add("ClientNumber", "Client Number");
            dgvloandata.Columns.Add("Name", "Name");
            dgvloandata.Columns.Add("Address", "Address");
            dgvloandata.Columns.Add("LoanType", "Loan Type");
            dgvloandata.Columns.Add("DenialDate", "Date Denied");
            dgvloandata.Columns.Add("LoanStatus", "Loan Status");
            dgvloandata.Columns.Add("Docs", "Documents");
            dgvloandata.Columns.Add("Countdown", "Days Left");

            // Add padding to "View Details" button column
            var viewDetailsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewDetails",
                HeaderText = "View Details",
                Text = "View Details",
                UseColumnTextForButtonValue = true,
                Width = 120,
                DefaultCellStyle = { Padding = new Padding(10) }
            };
            dgvloandata.Columns.Add(viewDetailsColumn);
        }

        private void LoadStatusData()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");

                // Define the aggregation pipeline
                var pipeline = new BsonDocument[]
                {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$LoanStatus" }  // Group by LoanStatus to get unique values
            }),
            new BsonDocument("$sort", new BsonDocument("_id", 1))  // Optional: Sort the results
                };

                // Execute the aggregation
                var results = loanDeniedCollection.Aggregate<BsonDocument>(pipeline).ToList();

                // Extract unique statuses from the aggregation results
                var statuses = results.Select(doc => doc["_id"].AsString).ToList();

                // Bind statuses to the ComboBox
                cbstatus.Items.Clear();
                if (statuses.Count > 0)
                {
                    cbstatus.Items.AddRange(statuses.ToArray());
                    cbstatus.SelectedIndex = 0;
                }
                else
                {
                    cbstatus.Items.Add("No data");
                    cbstatus.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading status data: " + ex.Message);
            }
        }


        private void FilterDeniedLoansByStatus(string status)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");

                // Filter the loans by the selected status
                var filter = Builders<BsonDocument>.Filter.Eq("LoanStatus", status);
                var deniedLoans = loanDeniedCollection.Find(filter).ToList();

                // Bind the filtered data to dgvloandata
                BindDeniedLoansToDataGridView(deniedLoans);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering denied loans by status: " + ex.Message);
            }
        }




        private void FilterLoansByDate(DateTime selectedDate)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");

                // Convert the selected date to the start and end of the day
                var startOfDay = selectedDate.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                // Create a filter for the DenialDate range
                var filter = Builders<BsonDocument>.Filter.Gte("DenialDate", startOfDay) &
                             Builders<BsonDocument>.Filter.Lte("DenialDate", endOfDay);

                // Add status filter if a status is selected
                if (cbstatus.SelectedItem != null)
                {
                    var statusFilter = Builders<BsonDocument>.Filter.Eq("LoanStatus", cbstatus.SelectedItem.ToString());
                    filter = filter & statusFilter;
                }

                var deniedLoans = loanDeniedCollection.Find(filter).ToList();

                // Clear existing rows
                dgvloandata.Rows.Clear();

                // Populate the DataGridView with filtered data
                foreach (var loan in deniedLoans)
                {
                    var name = $"{loan.GetValue("FirstName", "")} {loan.GetValue("LastName", "")}";
                    var address = $"{loan.GetValue("Street", "")}, {loan.GetValue("Barangay", "")}, {loan.GetValue("City", "")}, {loan.GetValue("Province", "")}";

                    // Format docs to be a single line
                    var docs = loan.GetValue("docs", "").ToString().Replace("\n", " ").Replace("\r", " ").Trim();

                    // Convert DenialDate to DateTime and then to MM/dd/yyyy format
                    var denialDate = loan.GetValue("DenialDate", BsonNull.Value);
                    var formattedDate = denialDate == BsonNull.Value ? "" : ((DateTime)denialDate).ToString("MM/dd/yyyy");

                    // Calculate the countdown for deletion
                    var daysLeft = ((DateTime)denialDate).AddDays(30) - DateTime.Now;
                    var countdown = daysLeft.TotalDays > 0 ? $"{(int)daysLeft.TotalDays} days left" : "Expired";

                    dgvloandata.Rows.Add(
                        loan.GetValue("ClientNumber", ""),
                        name,
                        address,
                        loan.GetValue("LoanType", ""),
                        formattedDate,
                        loan.GetValue("LoanStatus", ""),
                        docs,
                        countdown
                    );
                }

                // Align columns
                dgvloandata.Columns["ClientNumber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["LoanType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["LoanStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvloandata.Columns["Countdown"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

        private void LoadAllDeniedLoans()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");

                // Retrieve all documents
                var deniedLoans = loanDeniedCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();

                // Bind the data to dgvloandata
                BindDeniedLoansToDataGridView(deniedLoans);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading all denied loans: " + ex.Message);
            }
        }

        private void BindDeniedLoansToDataGridView(List<BsonDocument> deniedLoans)
        {
            if (deniedLoans.Count > 0)
            {
                dgvloandata.DataSource = deniedLoans.Select(loan =>
                {
                    var denialDateValue = loan.GetValue("DenialDate", null);
                    var denialDate = denialDateValue.IsBsonDateTime ? denialDateValue.AsDateTime.ToUniversalTime() : DateTime.MinValue;

                    return new
                    {
                        AccountId = loan.GetValue("AccountId", "").AsString,
                        ClientNumber = loan.GetValue("ClientNumber", "").AsString,
                        FullName = $"{loan.GetValue("FirstName", "")} {loan.GetValue("MiddleName", "")} {loan.GetValue("LastName", "")}",
                        LoanType = loan.GetValue("LoanType", "").AsString,
                        LoanStatus = loan.GetValue("LoanStatus", "").AsString,
                        DenialDate = denialDate.ToString("MM/dd/yyyy"),
                        Countdown = CalculateCountdown(denialDate)
                    };
                }).ToList();

                dgvloandata.Visible = true;
                lnorecord.Visible = false;
            }
            else
            {
                dgvloandata.Visible = false;
                lnorecord.Visible = true;
            }
        }

        private string CalculateCountdown(DateTime denialDate)
        {
            var expirationDate = denialDate.AddDays(30);
            var remainingDays = (expirationDate - DateTime.Now).Days;
            return remainingDays >= 0 ? $"{remainingDays} days remaining" : "Expired";
        }


        private void frm_home_ADMIN_dnloans_Load(object sender, EventArgs e)
        {
           //LoadStatusData();
            SetupDataGridView();
            FilterLoansByDate(dtdateDenied.Value.Date);
        }

        private void dtdateDenied_ValueChanged(object sender, EventArgs e)
        {
            FilterLoansByDate(dtdateDenied.Value.Date);
        }

        private void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedStatus = cbstatus.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedStatus) || selectedStatus == "No data")
                {
                    LoadAllDeniedLoans();
                }
                else
                {
                    FilterDeniedLoansByStatus(selectedStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering loans by status: " + ex.Message);
            }
        }

        private void dgvloandata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvloandata.ClearSelection();
        }
    }
}
