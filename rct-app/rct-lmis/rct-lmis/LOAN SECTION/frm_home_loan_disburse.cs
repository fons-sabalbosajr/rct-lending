using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.ADMIN_SECTION;
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
    public partial class frm_home_loan_disburse : Form
    {
        public string AccountID { get; set; }

        private IMongoCollection<BsonDocument> collection;
        private readonly IMongoCollection<BsonDocument> loanRateCollection;
        private DataTable dataTable;

        public frm_home_loan_disburse()
        {
            InitializeComponent();

            var database = MongoDBConnection.Instance.Database;
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");

        }

        private void frm_home_loan_disburse_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
        }

        private void LoadDataToDataGridView()
        {
            var filter = Builders<BsonDocument>.Filter.Empty; // Retrieve all documents
            var documents = loanRateCollection.Find(filter).ToList();
            DataTable dataTable = new DataTable();

            // Define the columns to display in the DataGridView
            string[] displayColumns = { "Term", "Principal", "Type", "Mode", "Interest Rate/Month", "Processing" };

            // Create the columns in the DataTable
            foreach (string column in displayColumns)
            {
                dataTable.Columns.Add(column);
            }
            dataTable.Columns.Add("FullDocument", typeof(BsonDocument)); // Hidden column to store the entire document

            if (documents.Count > 0)
            {
                // Add rows to the DataTable
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (string column in displayColumns)
                    {
                        if (doc.Contains(column))
                        {
                            var element = doc[column];
                            if (element.IsNumeric())
                            {
                                if (column == "Principal")
                                {
                                    // Format Principal with ₱ text
                                    row[column] = "₱ " + Math.Round(element.ToDouble(), 0).ToString();
                                }
                                else
                                {
                                    // Round numeric values to the nearest ones
                                    row[column] = Math.Round(element.ToDouble(), 0);
                                }
                            }
                            else
                            {
                                row[column] = element.ToString();
                            }
                        }
                    }
                    row["FullDocument"] = doc; // Store the entire document in the hidden column
                    dataTable.Rows.Add(row);
                }
            }

            dgvloandata.DataSource = dataTable;

            if (dgvloandata.Columns.Count > 0)
            {
                dgvloandata.Columns["FullDocument"].Visible = false; // Hide the full document column
            }

            // Center align all columns and set specific numeric formatting
            foreach (DataGridViewColumn column in dgvloandata.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Format numeric columns to show two decimal places except "Term"
                if (column.Name != "Term" && (column.Name == "Principal" || column.Name == "Interest Rate/Month" || column.Name == "Processing"))
                {
                    column.DefaultCellStyle.Format = "N2";
                }
            }

            // Debug output to verify the DataTable structure and data
            //Console.WriteLine("DataTable Columns:");
            foreach (DataColumn col in dataTable.Columns)
            {
                Console.WriteLine($"Column: {col.ColumnName}");
            }

            //Console.WriteLine("DataTable Rows:");
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write($"{item} ");
                }
                Console.WriteLine();
            }
        }

        private void tsearchamt_TextChanged(object sender, EventArgs e)
        {
            string searchText = tsearchamt.Text.Trim();
            string filterExpression = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    // Create the filter expression
                    filterExpression = string.Format(
                        "Term LIKE '%{0}%' OR Principal LIKE '%{0}%' OR [Interest Rate/Month] LIKE '%{0}%' OR Processing LIKE '%{0}%' OR Type LIKE '%{0}%' OR Mode LIKE '%{0}%'",
                        searchText);

                    // Apply the filter to the DataTable
                    DataView dv = new DataView(dataTable);
                    dv.RowFilter = filterExpression;
                    dgvloandata.DataSource = dv;
                }
                else
                {
                    // Reset the DataView if the search text is empty
                    // Make sure `dataTable` is properly assigned with the full dataset initially
                    dgvloandata.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying search filter: " + ex.Message);
            }
        }


        private void dgvloandata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row index is selected
            {
                DataGridViewRow selectedRow = dgvloandata.Rows[e.RowIndex];
                if (selectedRow != null && selectedRow.Cells["FullDocument"].Value != null)
                {
                    BsonDocument fullDocument = selectedRow.Cells["FullDocument"].Value as BsonDocument;

                    if (fullDocument != null)
                    {
                        // Debug output
                        //Console.WriteLine("FullDocument: " + fullDocument.ToJson());

                        try
                        {
                            tloanamt.Text = fullDocument.Contains("Principal") ? "₱ " + fullDocument["Principal"].ToString() + ".00" : string.Empty;
                            tloanterm.Text = fullDocument.Contains("Term") ? fullDocument["Term"].ToString() + " month/s" : string.Empty;
                            tloaninterest.Text = fullDocument.Contains("Interest Rate/Month") ? fullDocument["Interest Rate/Month"].ToString() + ".00" : string.Empty;
                            trfservicefee.Text = fullDocument.Contains("Processing Fee") ? fullDocument["Processing Fee"].ToString() + ".00" : string.Empty;


                            trfnotarialfee.Text = fullDocument.Contains("Notarial Rate") ? fullDocument["Notarial Rate"].ToString() + ".00" : string.Empty;
                            trfnotarialamt.Text = trfannotationfee.Text;

                            trfinsurancefee.Text = fullDocument.Contains("Insurance Rate") ? fullDocument["Insurance Rate"].ToString() + ".00" : string.Empty;
                            trfinsuranceamt.Text = trfinsurancefee.Text;

                            trfannotationfee.Text = fullDocument.Contains("Annotation Rate") ? fullDocument["Annotation Rate"].ToString() + ".00" : string.Empty;
                            trfannotationmt.Text = trfannotationfee.Text;

                            trfvat.Text = fullDocument.Contains("Vat Rate") ? fullDocument["Vat Rate"].ToString() + ".00" : string.Empty;
                            trfvatamt.Text = trfvat.Text;

                            trfmisc.Text = fullDocument.Contains("Misc. Rate") ? fullDocument["Misc. Rate"].ToString() + ".00" : string.Empty;
                            trfmiscamt.Text = trfmisc.Text;

                            trfdocfee.Text = fullDocument.Contains("Doc Rate") ? fullDocument["Doc Rate"].ToString() + ".00" : string.Empty;
                            trfdocamt.Text = trfdocfee.Text;

                            // Assuming you have logic to compute these amounts, otherwise set them as empty or with computed values.
                            tamortizedamt.Text = string.Empty;
                            tpenaltymo.Text = fullDocument.Contains("Penalty Rate") ? fullDocument["Penalty Rate"].ToString() + ".00" : string.Empty;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing fullDocument: " + ex.Message);
                        }
                    }
                    else
                    {
                        // Debug output
                        Console.WriteLine("FullDocument is null");
                    }
                }
                else
                {
                    // Debug output
                    Console.WriteLine("Selected row or FullDocument cell is null");
                }
            }
        }
    }
}
