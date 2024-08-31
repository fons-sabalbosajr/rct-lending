using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_voucher : Form
    {
        private string _cashClnNo;

        public frm_home_loan_voucher(string cashClnNo)
        {
            InitializeComponent();
            _cashClnNo = cashClnNo;
            bsoa.Enabled = false;
            bvoucher.Enabled = false;
        }

        LoadingFunction load = new LoadingFunction();

        private void LoadCollectors()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_collectors");
            var names = collection.Find(new BsonDocument())
                                  .Project(Builders<BsonDocument>.Projection.Include("Name"))
                                  .ToList()
                                  .Select(doc => doc["Name"].AsString)
                                  .ToList();

            // Add the default item
            names.Insert(0, "--select collector--");

            cbcollector.Items.Clear();
            cbcollector.Items.AddRange(names.ToArray());

            // Set the default selection to the first item
            cbcollector.SelectedIndex = 0;
        }

        // Method to fetch the document
        private BsonDocument FetchDocument(string cashClnNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("cashClnNo", cashClnNo);
            var document = collection.Find(filter).FirstOrDefault();

            return document;
        }

        private void LoadDataToUI(string cashClnNo)
        {
            var document = FetchDocument(cashClnNo);

            if (document != null)
            {
                // Populate common fields
                loanLNno.Text = document.Contains("LoanIDNo") ? document["LoanIDNo"].AsString : "N/A";
                lloanamt.Text = document.Contains("loanAmt") ? FormatCurrency(ConvertToDouble(document["loanAmt"])) : "N/A";
                lloanterm.Text = document.Contains("loanTerm") ? document["loanTerm"].AsString + " months" : "N/A";
                lloaninterest.Text = document.Contains("loanInterest") ? document["loanInterest"].AsString : "N/A";
                lamotperiod.Text = document.Contains("days") ? document["days"].AsString + " days" : "N/A";

                lloantype.Text = document.Contains("LoanType") ? document["LoanType"].AsString : "N/A";
                lloanmode.Text = document.Contains("Mode") ? document["Mode"].AsString : "N/A";
                lloanprocessfee.Text = document.Contains("cashProFee") ? FormatCurrency(ConvertToDouble(document["cashProFee"])) :
                                         (document.Contains("onlineProFee") ? FormatCurrency(ConvertToDouble(document["onlineProFee"])) :
                                         (document.Contains("bankProFee") ? FormatCurrency(ConvertToDouble(document["bankProFee"])) : "N/A"));
                lloannotarialrate.Text = document.Contains("rfNotarialFee") ? FormatCurrency(ConvertToDouble(document["rfNotarialFee"])) : "N/A";
                lloaninsurancerate.Text = document.Contains("rfInsuranceFee") ? FormatCurrency(ConvertToDouble(document["rfInsuranceFee"])) : "N/A";
                lloanannotationrate.Text = document.Contains("rfAnnotationFee") ? FormatCurrency(ConvertToDouble(document["rfAnnotationFee"])) : "N/A";
                lloanVAT.Text = document.Contains("rfVat") ? FormatCurrency(ConvertToDouble(document["rfVat"])) : "N/A";
                lloanmisc.Text = document.Contains("rfMisc") ? FormatCurrency(ConvertToDouble(document["rfMisc"])) : "N/A";
                ldocrate.Text = document.Contains("rfDocFee") ? FormatCurrency(ConvertToDouble(document["rfDocFee"])) : "N/A";
                lclientno.Text = document.Contains("cashClnNo") ? document["cashClnNo"].AsString : "N/A";
                lstartpayment.Text = document.Contains("PaymentStartDate") ? document["PaymentStartDate"].AsString : "N/A";

                // Determine Client Name based on payment method
                if (document.Contains("cashName"))
                {
                    lclientname.Text = document["cashName"].AsString;
                    lpaymentmode.Text = "Cash";

                }
                else if (document.Contains("onlineName"))
                {
                    lclientname.Text = document["onlineName"].AsString;
                    lpaymentmode.Text = document["onlinePlatform"].AsString;
                }
                else if (document.Contains("bankName"))
                {
                    lclientname.Text = document["bankName"].AsString;
                    lpaymentmode.Text = document["bankPlatform"].AsString;
                }
                else
                {
                    lclientname.Text = "N/A";
                }

                // Populate DataGridView
                var dataTable = new DataTable();
                dataTable.Columns.Add("Transaction No.");
                dataTable.Columns.Add("Releasing Date");
                dataTable.Columns.Add("Loan Amount (Principal)");
                dataTable.Columns.Add("Processing Fee");
                dataTable.Columns.Add("Disbursed Amount");

                // Check which payment method is used and populate accordingly
                if (document.Contains("cashNo"))
                {
                    // Cash Payment
                    _ = dataTable.Rows.Add(
                        document.Contains("cashNo") ? document["cashNo"].AsString : "N/A",
                        document.Contains("cashDate") ? document["cashDate"].AsString : "N/A",
                        document.Contains("cashAmt") ? FormatCurrency(ConvertToDouble(document["cashAmt"])) : "N/A",
                        document.Contains("cashProFee") ? FormatCurrency(ConvertToDouble(document["cashProFee"])) : "N/A",
                        document.Contains("cashPoAmt") ? FormatCurrency(ConvertToDouble(document["cashPoAmt"])) : "N/A"

                    );
                }
                else if (document.Contains("onlineRefNo"))
                {
                    // Online Payment
                    _ = dataTable.Rows.Add(
                        document.Contains("onlineRefNo") ? document["onlineRefNo"].AsString : "N/A",
                        document.Contains("onlineDate") ? document["onlineDate"].AsString : "N/A",
                        document.Contains("onlineAmt") ? FormatCurrency(ConvertToDouble(document["onlineAmt"])) : "N/A",
                        document.Contains("onlineProFee") ? FormatCurrency(ConvertToDouble(document["onlineProFee"])) : "N/A",
                        document.Contains("onlinePoAmt") ? FormatCurrency(ConvertToDouble(document["onlinePoAmt"])) : "N/A"
                    );
                }
                else if (document.Contains("bankRefNo"))
                {
                    // Bank Payment
                    _ = dataTable.Rows.Add(
                        document.Contains("bankRefNo") ? document["bankRefNo"].AsString : "N/A",
                        document.Contains("bankDate") ? document["bankDate"].AsString : "N/A",
                        document.Contains("bankAmt") ? FormatCurrency(ConvertToDouble(document["bankAmt"])) : "N/A",
                        document.Contains("bankProFee") ? FormatCurrency(ConvertToDouble(document["bankProFee"])) : "N/A",
                        document.Contains("bankPoAmt") ? FormatCurrency(ConvertToDouble(document["bankPoAmt"])) : "N/A"
                    );
                }
                else
                {
                    MessageBox.Show("No payment details found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                dgvdisburse.DataSource = dataTable;
            }
            else
            {
                MessageBox.Show("Data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double ConvertToDouble(BsonValue bsonValue)
        {
            switch (bsonValue.BsonType)
            {
                case BsonType.Double:
                    return bsonValue.AsDouble;
                case BsonType.String:
                    string cleanedString = CleanCurrencyString(bsonValue.AsString);
                    if (double.TryParse(cleanedString, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                    {
                        return result;
                    }
                    break;
                case BsonType.Int32:
                    return bsonValue.AsInt32;
                case BsonType.Int64:
                    return bsonValue.AsInt64;
                default:
                    throw new InvalidCastException($"Unsupported BsonType: {bsonValue.BsonType}");
            }
            throw new FormatException($"Cannot convert BsonValue to double: {bsonValue}");
        }

        private string CleanCurrencyString(string value)
        {
            // Remove non-numeric characters (e.g., currency symbols) except decimal points and commas
            return Regex.Replace(value, @"[^\d.,-]", "");
        }

        private string FormatCurrency(double amount)
        {
            // Format the amount as currency with the Peso sign
            return $"₱{amount:N2}";
        }

        private void LoadCollectorInfo(string collectorName)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_collectors");

            var filter = Builders<BsonDocument>.Filter.Eq("Name", collectorName);
            var collectorDoc = collection.Find(filter).FirstOrDefault();

            if (collectorDoc != null)
            {
                tarearoute.Text = collectorDoc.Contains("AreaRoute") ? collectorDoc["AreaRoute"].AsString : "N/A";
                tidno.Text = collectorDoc.Contains("GeneratedIDNumber") ? collectorDoc["GeneratedIDNumber"].AsString : "N/A";
                tdesignation.Text = collectorDoc.Contains("Role") ? collectorDoc["Role"].AsString : "N/A";
                tcontact.Text = collectorDoc.Contains("ContactNumber") ? collectorDoc["ContactNumber"].AsString : "N/A";
            }
            else
            {
                tarearoute.Text = "N/A";
                tidno.Text = "N/A";
                tdesignation.Text = "N/A";
                tcontact.Text = "N/A";
            }
        }

        private BsonDocument CreateLoanReleasedDocument()
        {
            var document = new BsonDocument
            {
                { "LoanIDNo", loanLNno.Text },
                { "loanAmt", ConvertToDouble(ExtractNumericValue(lloanamt.Text)) },
                { "loanTerm", ExtractNumericValue(lloanterm.Text) },
                { "loanInterest", ExtractNumericValue(lloaninterest.Text) },
                { "days", ExtractNumericValue(lamotperiod.Text) },
                { "LoanType", lloantype.Text },
                { "Mode", lloanmode.Text },
                { "cashProFee", ConvertToDouble(ExtractNumericValue(lloanprocessfee.Text)) },
                { "rfNotarialFee", ConvertToDouble(ExtractNumericValue(lloannotarialrate.Text)) },
                { "rfInsuranceFee", ConvertToDouble(ExtractNumericValue(lloaninsurancerate.Text)) },
                { "rfAnnotationFee", ConvertToDouble(ExtractNumericValue(lloanannotationrate.Text)) },
                { "rfVat", ConvertToDouble(ExtractNumericValue(lloanVAT.Text)) },
                { "rfMisc", ConvertToDouble(ExtractNumericValue(lloanmisc.Text)) },
                { "rfDocFee", ConvertToDouble(ExtractNumericValue(ldocrate.Text)) },
                { "cashClnNo", lclientno.Text },
                { "payoutDate", lstartpayment.Text },
                { "clientName", lclientname.Text },
                { "paymentMode", lpaymentmode.Text },
                { "CollectorName", cbcollector.SelectedItem.ToString() },
                { "AreaRoute", tarearoute.Text },        
                { "IDNo", tidno.Text },                 
                { "Designation", tdesignation.Text },    
                { "Contact", tcontact.Text }
            };

            AddPaymentDetails(document);

            return document;
        }

        private void AddPaymentDetails(BsonDocument document)
        {
            var paymentDetails = new BsonDocument
             {
                 { "Transaction No.", "N/A" },
                 { "Releasing Date", "N/A" },
                 { "Loan Amount (Principal)", "N/A" },
                 { "Processing Fee", "N/A" },
                 { "Disbursed Amount", "N/A" }
             };

            if (dgvdisburse.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdisburse.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        paymentDetails["Transaction No."] = row.Cells["Transaction No."].Value?.ToString() ?? "N/A";
                        paymentDetails["Releasing Date"] = row.Cells["Releasing Date"].Value?.ToString() ?? "N/A";
                        paymentDetails["Loan Amount (Principal)"] = row.Cells["Loan Amount (Principal)"].Value?.ToString() ?? "N/A";
                        paymentDetails["Processing Fee"] = row.Cells["Processing Fee"].Value?.ToString() ?? "N/A";
                        paymentDetails["Disbursed Amount"] = row.Cells["Disbursed Amount"].Value?.ToString() ?? "N/A";
                    }
                }
            }

            document["PaymentDetails"] = paymentDetails;
        }

        private void SaveDocumentToCollection(BsonDocument document)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_released");
            collection.InsertOne(document);
        }

        // Helper method to extract numeric values from formatted strings
        private double ExtractNumericValue(string formattedValue)
        {
            string cleanedValue = CleanCurrencyString(formattedValue);
            return ConvertToDouble(new BsonString(cleanedValue));
        }

        private void DeleteDocumentFromCollection(string cashClnNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("cashClnNo", cashClnNo);
            collection.DeleteOne(filter);
        }

        private void ClearUIFields()
        {
            // Clear the UI fields as needed
            loanLNno.Text = "";
            lloanamt.Text = "";
            lloanterm.Text = "";
            lloaninterest.Text = "";
            lamotperiod.Text = "";
            lloantype.Text = "";
            lloanmode.Text = "";
            lloanprocessfee.Text = "";
            lloannotarialrate.Text = "";
            lloaninsurancerate.Text = "";
            lloanannotationrate.Text = "";
            lloanVAT.Text = "";
            lloanmisc.Text = "";
            ldocrate.Text = "";
            lclientno.Text = "";
            lstartpayment.Text = "";
            lclientname.Text = "";
            lpaymentmode.Text = "";

            dgvdisburse.DataSource = null;  // Clear DataGridView
            lnorecord.Visible = true;
            lnorecord.Visible = true;
        }



        private void frm_home_loan_voucher_Load(object sender, EventArgs e)
        {
            LoadCollectors();
            if (!string.IsNullOrEmpty(_cashClnNo))
            {
                LoadDataToUI(_cashClnNo);
            }
        }

        private void cbcollector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCollector = cbcollector.SelectedItem as string;

            if (selectedCollector == "--select collector--")
            {
                tarearoute.Text = "N/A";
                tidno.Text = "N/A";
                tdesignation.Text = "N/A";
                tcontact.Text = "N/A";
            }
            else
            {
                LoadCollectorInfo(selectedCollector);
            }
        }

        private void brelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to release the disbursement process?", 
                "Release Disbursement Process", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                try
                {
                    load.Show(this);
                    Thread.Sleep(4000);
                    var document = CreateLoanReleasedDocument();
                    SaveDocumentToCollection(document);
                    load.Close();

                    MessageBox.Show(this, "Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bsoa.Enabled = true;
                    bvoucher.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void dgvdisburse_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdisburse.ClearSelection();
        }

        private void bcancel_Click(object sender, EventArgs e)
        {
            // Confirm the abort action with the user
            if (MessageBox.Show(this, "Are you sure you want to abort the releasing process and delete the data?",
                "Abort Releasing Process", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Optionally, you can clear the UI fields here if needed
                    ClearUIFields();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frm_home_loan_voucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Optionally, you can clear the UI fields here if needed
                ClearUIFields();
                this.Hide();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
