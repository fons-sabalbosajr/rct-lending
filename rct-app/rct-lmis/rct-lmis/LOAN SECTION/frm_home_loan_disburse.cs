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
    }
}
