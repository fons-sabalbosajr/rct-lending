﻿using MongoDB.Bson;
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

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loand_req_details : Form
    {
        private string accountId;

        public frm_home_loand_req_details(string accountId)
        {
            InitializeComponent();
            this.accountId = accountId;
           
            InitializeDataGridView();
        }

        LoadingFunction load = new LoadingFunction();

        private void InitializeDataGridView()
        {
            dgvuploads.Columns.Clear();
            dgvuploads.Columns.Add("DocumentName", "Document Name");
            dgvuploads.Columns.Add("DocumentLink", "Document Link");
            dgvuploads.Columns.Add(new DataGridViewLinkColumn
            {
                Name = "ViewFile",
                HeaderText = "View File",
                Text = "View File",
                UseColumnTextForLinkValue = true
            });
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
                    // Load loan application details
                    tlaccountno.Text = loanApplication.GetValue("AccountId", "").AsString;

                    if (loanApplication.TryGetValue("ApplicationDate", out var applicationDate) && applicationDate.IsValidDateTime)
                    {
                        tapplicationdate.Text = applicationDate.ToLocalTime().ToString("MM-dd-yyyy");
                    }

                    tloantype.Text = loanApplication.GetValue("LoanType", "").AsString;

                    tfname.Text = $"{loanApplication.GetValue("FirstName", "").AsString} {loanApplication.GetValue("MiddleName", "").AsString} {loanApplication.GetValue("LastName", "").AsString} {loanApplication.GetValue("SuffixName", "").AsString}";
                    tgender.Text = loanApplication.GetValue("Gender", "").AsString;
                    tmaritalstatus.Text = loanApplication.GetValue("CStatus", "").AsString;
                    taddress.Text = $"{loanApplication.GetValue("Street", "").AsString}, {loanApplication.GetValue("Barangay", "").AsString}, {loanApplication.GetValue("City", "").AsString}, {loanApplication.GetValue("Province", "").AsString}";
                    thousetype.Text = loanApplication.GetValue("HouseType", "").AsString;
                    tstaylength.Text = loanApplication.GetValue("StayLength", "").AsString;
                    trfee.Text = loanApplication.GetValue("Fee", "").AsString;

                    if (loanApplication.TryGetValue("RBLate", out var rblate) && rblate.IsValidDateTime)
                    {
                        tbirth.Text = rblate.ToLocalTime().ToString("MM-dd-yyyy");
                    }

                    tbusiness.Text = loanApplication.GetValue("Business", "").AsString;
                    tcontact.Text = loanApplication.GetValue("CP", "").AsString;
                    tmincome.Text = loanApplication.GetValue("Income", "").AsString;

                    // Spouse/Co-Borrower Information
                    tsfname.Text = loanApplication.GetValue("Spouse", "").AsString;
                    tsoccupation.Text = loanApplication.GetValue("Occupation", "").AsString;
                    tsmincome.Text = loanApplication.GetValue("SpIncome", "").AsString;
                    tscontact.Text = loanApplication.GetValue("SpCP", "").AsString;

                    if (loanApplication.TryGetValue("RSDate", out var rsdate) && rsdate.IsValidDateTime)
                    {
                        tsbirth.Text = rsdate.ToLocalTime().ToString("MM-dd-yyyy");
                    }

                    // Co-Borrower Information
                    tbfname.Text = $"{loanApplication.GetValue("CBFName", "").AsString} {loanApplication.GetValue("CBMName", "").AsString} {loanApplication.GetValue("CBLName", "").AsString} {loanApplication.GetValue("CBSName", "").AsString}";
                    tbaddress.Text = $"{loanApplication.GetValue("CBStreet", "").AsString}, {loanApplication.GetValue("CBBarangay", "").AsString}, {loanApplication.GetValue("CBCity", "").AsString}, {loanApplication.GetValue("CBProvince", "").AsString}";
                    tbgender.Text = loanApplication.GetValue("CGender", "").AsString;
                    tbmaritalstatus.Text = loanApplication.GetValue("CStatus", "").AsString;
                    tbage.Text = loanApplication.GetValue("CBAge", "").AsString;
                    tbcontact.Text = loanApplication.GetValue("CBCP", "").AsString;
                    tbsincome.Text = loanApplication.GetValue("CBIncome", "").AsString;

                    // Load docs into DataGridView
                    if (loanApplication.TryGetValue("docs", out var docs) && docs.IsString && loanApplication.TryGetValue("doc-link", out var docLinks) && docLinks.IsString)
                    {
                        LoadDocsIntoDataGridView(docs.AsString, docLinks.AsString);
                    }
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

        private void ConfigureDataGridView()
        {
            // Set the wrapping for the second column
            dgvuploads.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvuploads.Columns[2].Width = 200;

            // Optional: Adjust column width to fit the content
            dgvuploads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }



        private void LoadDocsIntoDataGridView(string docs, string docLinks)
        {
            dgvuploads.Rows.Clear();

            // Split the docs and docLinks by comma, and trim any leading or trailing spaces
            var docsArray = docs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(doc => doc.Trim())
                                .ToArray();

            var docLinksArray = docLinks.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(link => link.Trim())
                                        .ToArray();

            for (int i = 0; i < docsArray.Length; i++)
            {
                // Ensure docLinksArray has an element at index i, or default to an empty string
                int rowIndex = dgvuploads.Rows.Add(docsArray[i], docLinksArray.Length > i ? docLinksArray[i] : string.Empty, "View File");

                var link = new DataGridViewLinkCell
                {
                    Value = "View File",
                    UseColumnTextForLinkValue = true
                };

                dgvuploads.Rows[rowIndex].Cells[2] = link;
            }

            // Configure the DataGridView after loading data
            ConfigureDataGridView();
        }

        private void frm_home_loand_req_details_Load(object sender, EventArgs e)
        {
            LoadDetails();
        }

        private void dgvuploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2) // Assuming the "View File" link is in the third column (index 2)
            {
                var docLink = dgvuploads.Rows[e.RowIndex].Cells[1].Value.ToString();
                try
                {
                    // Use Process.Start to open the URL in the default web browser
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = docLink,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening the file: " + ex.Message);
                }
            }
        }

        private void ApprovedLoan() 
        {
            try
            {
                // Access the loan_application and loan_approved collections
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

                // Retrieve the document based on accountId
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var loanApplication = loanAppCollection.Find(filter).FirstOrDefault();

                if (loanApplication != null)
                {
                    // Prepare the document to insert into loan_approved collection
                    var approvedLoan = new BsonDocument
            {
                { "AccountId", loanApplication.GetValue("AccountId", "") },
                { "LoanType", loanApplication.GetValue("LoanType", "") },
                { "FirstName", loanApplication.GetValue("FirstName", "") },
                { "MiddleName", loanApplication.GetValue("MiddleName", "") },
                { "LastName", loanApplication.GetValue("LastName", "") },
                { "SuffixName", loanApplication.GetValue("SuffixName", "") },
                { "Gender", loanApplication.GetValue("Gender", "") },
                { "Street", loanApplication.GetValue("Street", "") },
                { "Barangay", loanApplication.GetValue("Barangay", "") },
                { "City", loanApplication.GetValue("City", "") },
                { "Province", loanApplication.GetValue("Province", "") },
                { "HouseType", loanApplication.GetValue("HouseType", "") },
                { "StayLength", loanApplication.GetValue("StayLength", "") },
                { "Fee", loanApplication.GetValue("Fee", "") },
                { "RBLate", loanApplication.GetValue("RBLate", "") },
                { "Business", loanApplication.GetValue("Business", "") },
                { "CP", loanApplication.GetValue("CP", "") },
                { "Income", loanApplication.GetValue("Income", "") },
                { "Spouse", loanApplication.GetValue("Spouse", "") },
                { "Occupation", loanApplication.GetValue("Occupation", "") },
                { "SpIncome", loanApplication.GetValue("SpIncome", "") },
                { "SpCP", loanApplication.GetValue("SpCP", "") },
                { "RSDate", loanApplication.GetValue("RSDate", "") },
                { "CBFName", loanApplication.GetValue("CBFName", "") },
                { "CBMName", loanApplication.GetValue("CBMName", "") },
                { "CBLName", loanApplication.GetValue("CBLName", "") },
                { "CBSName", loanApplication.GetValue("CBSName", "") },
                { "CBStreet", loanApplication.GetValue("CBStreet", "") },
                { "CBBarangay", loanApplication.GetValue("CBBarangay", "") },
                { "CBCity", loanApplication.GetValue("CBCity", "") },
                { "CBProvince", loanApplication.GetValue("CBProvince", "") },
                { "CGender", loanApplication.GetValue("CGender", "") },
                { "CStatus", loanApplication.GetValue("CStatus", "") },
                { "CBAge", loanApplication.GetValue("CBAge", "") },
                { "CBCP", loanApplication.GetValue("CBCP", "") },
                { "CBIncome", loanApplication.GetValue("CBIncome", "") },
                { "ApplicationDate", loanApplication.GetValue("ApplicationDate", "") },
                { "LoanStatus", loanApplication.GetValue("LoanStatus", "Application Approved") },
            };

                    // Insert the document into loan_approved collection
                    loanApprovedCollection.InsertOne(approvedLoan);

                    // Update the Status in the loan_application collection
                    var update = Builders<BsonDocument>.Update.Set("Status", "Approved Loan");
                    loanAppCollection.UpdateOne(filter, update);
                }
                else
                {
                    MessageBox.Show("No loan application found with the provided Account ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving the loan application: " + ex.Message);
            }
        }

        private void bapproved_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(2000);
            ApprovedLoan();
            load.Close();
            MessageBox.Show(this, "Loan application approved and details updated.");
        }
    }
}