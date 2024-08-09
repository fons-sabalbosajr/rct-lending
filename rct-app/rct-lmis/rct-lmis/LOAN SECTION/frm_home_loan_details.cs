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

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_new : Form
    {
        public string AccountID { get; set; }

        public frm_home_loan_new()
        {
            InitializeComponent();
        }

        LoadingFunction load =new LoadingFunction();

        private async void frm_home_loan_new_Load(object sender, EventArgs e)
        {
            // Display the AccountID on the label
            laccno.Text = $"{AccountID}";

            // Load data based on AccountID
            await LoadLoanDetailsAsync();

        }

        private async Task LoadLoanDetailsAsync()
        {
            // Retrieve the AccountId from the label
            string accountId = laccno.Text;

            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                // Query to find the document with the specified AccountID
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId" , AccountID);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    // Populate the text boxes with the data from the document
                    taccname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")} {document.GetValue("SuffixName", "")}".Trim();
                    taccaddress.Text = document.GetValue("Street", "").ToString();
                    taccbrgy.Text = document.GetValue("Barangay", "").ToString();
                    tacctown.Text = document.GetValue("City", "").ToString();
                    taccprov.Text = document.GetValue("Province", "").ToString();
                    
                    if (document.TryGetValue("RBLate", out BsonValue rbLateValue) && rbLateValue.IsBsonDateTime)
                    {
                        DateTime rbLateDate = rbLateValue.ToUniversalTime(); 
                        taddbirth.Text = rbLateDate.ToString("MM/dd/yyyy"); 
                    }
                    else
                    {
                        taddbirth.Text = string.Empty;
                    }
                    tacccontactno.Text = document.GetValue("CP", "").ToString();
                    taccemail.Text = document.GetValue("Email", "").ToString(); 

                    // Populate the loan status on the label
                    laccstatus.Text = document.GetValue("LoanStatus", "Not Available").ToString();
                    lloanstatus.Text = "FOR DISBURSEMENT";

                    // Populate additional fields
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")} {document.GetValue("SuffixName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Street", "")} , {document.GetValue("Barangay", "")} , {document.GetValue("City", "")} , {document.GetValue("Province", "")}".Trim();
                    trepcontact.Text = document.GetValue("CP", "").ToString(); // Example field, adjust if necessary
                    //trepidcard.Text = document.GetValue("RepIDCard", "").ToString(); // Example field, adjust if necessary
                    //trepcurrloan.Text = document.GetValue("RepCurrentLoan", "").ToString(); // Example field, adjust if necessary
                    //treploanbalance.Text = document.GetValue("RepLoanBalance", "").ToString(); // Example field, adjust if necessary
                    //treploanpenalty.Text = document.GetValue("RepLoanPenalty", "").ToString(); // Example field, adjust if necessary
                    //trepcollector.Text = document.GetValue("RepCollector", "").ToString(); // Example field, adjust if necessary
                    //treploantotal.Text = document.GetValue("RepLoanTotal", "").ToString(); // Example field, adjust if necessary
                    //treprepaydate.Text = document.GetValue("RepRepayDate", "").ToString(); // Example field, adjust if necessary

                }
                else
                {
                    MessageBox.Show("No loan details found for the specified Account ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan details: {ex.Message}");
            }
        }
    }
}
