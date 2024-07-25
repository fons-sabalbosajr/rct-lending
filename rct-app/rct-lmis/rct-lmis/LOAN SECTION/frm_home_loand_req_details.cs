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
    public partial class frm_home_loand_req_details : Form
    {
        private string accountId;

        public frm_home_loand_req_details(string accountId)
        {
            InitializeComponent();
            this.accountId = accountId;
            LoadDetails();
        }

        private void LoadDetails()
        {
            try
            {
                // Access the loan_application collection
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");

                // Retrieve the document based on accountId
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var loanApplication = loanAppCollection.Find(filter).FirstOrDefault();

                if (loanApplication != null)
                {
                    // Populate text boxes with the retrieved data
                    tlaccountno.Text = loanApplication.GetValue("AccountId", "").AsString;
                    tapplicationdate.Text = loanApplication.GetValue("RSDate", "").ToLocalTime().ToString("yyyy-MM-dd");
                    tfname.Text = $"{loanApplication.GetValue("FirstName", "").AsString} {loanApplication.GetValue("MiddleName", "").AsString} {loanApplication.GetValue("LastName", "").AsString} {loanApplication.GetValue("SuffixName", "").AsString}";
                    tgender.Text = loanApplication.GetValue("Gender", "").AsString;
                    tmaritalstatus.Text = loanApplication.GetValue("CStatus", "").AsString;
                    taddress.Text = $"{loanApplication.GetValue("Street", "").AsString}, {loanApplication.GetValue("Barangay", "").AsString}, {loanApplication.GetValue("City", "").AsString}, {loanApplication.GetValue("Province", "").AsString}";
                    thousetype.Text = ""; // Assuming no direct field in the provided data
                    trfee.Text = loanApplication.GetValue("Fee", "").AsString;
                    tstaylength.Text = loanApplication.GetValue("StayLength", "").AsString;
                    tbirth.Text = loanApplication.GetValue("RBLate", "").ToLocalTime().ToString("yyyy-MM-dd");
                    tcontact.Text = loanApplication.GetValue("CP", "").AsString;
                    tbusiness.Text = loanApplication.GetValue("Business", "").AsString;
                    tmincome.Text = loanApplication.GetValue("Income", "").AsString;

                    // Spouse/Co-Borrower Information
                    tsfname.Text = loanApplication.GetValue("Spouse", "").AsString;
                    tsoccupation.Text = loanApplication.GetValue("Occupation", "").AsString;
                    tsmincome.Text = loanApplication.GetValue("SpIncome", "").AsString;
                    tscontact.Text = loanApplication.GetValue("SpCP", "").AsString;
                    tsbirth.Text = ""; // Assuming no direct field in the provided data

                    // Co-Borrower Information
                    tbfname.Text = $"{loanApplication.GetValue("CBFName", "").AsString} {loanApplication.GetValue("CBMName", "").AsString} {loanApplication.GetValue("CBLName", "").AsString} {loanApplication.GetValue("CBSName", "").AsString}";
                    tbaddress.Text = $"{loanApplication.GetValue("CBStreet", "").AsString}, {loanApplication.GetValue("CBBarangay", "").AsString}, {loanApplication.GetValue("CBCity", "").AsString}, {loanApplication.GetValue("CBProvince", "").AsString}";
                    tbgender.Text = loanApplication.GetValue("CGender", "").AsString;
                    tbmaritalstatus.Text = loanApplication.GetValue("CStatus", "").AsString;
                    tbage.Text = loanApplication.GetValue("CBAge", "").AsString;
                    tbcontact.Text = loanApplication.GetValue("CBCP", "").AsString;
                    tbsincome.Text = loanApplication.GetValue("CBIncome", "").AsString;
                }
                else
                {
                    MessageBox.Show("No loan application found with the provided Account ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading loan application details: " + ex.Message);
                MessageBox.Show("Error loading loan application details. Please check the console for details.");
            }
        }


        private void frm_home_loand_req_details_Load(object sender, EventArgs e)
        {

        }
    }
}
