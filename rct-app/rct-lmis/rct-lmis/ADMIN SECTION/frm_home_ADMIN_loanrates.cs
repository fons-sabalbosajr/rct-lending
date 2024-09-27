using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using ExcelDataReader;
using Guna.UI2.WinForms;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_loanrates : Form
    {
        private readonly IMongoCollection<BsonDocument> loanRateCollection;

        public frm_home_ADMIN_loanrates()
        {
            InitializeComponent();
            disable();

            // Initialize MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");


            cbmpayment.Items.AddRange(new object[] { "ALL", "DAILY", "WEEKLY", "SEMI-MONTHLY", "MONTHLY" });
            cbmpayment.SelectedIndex = 0;
        }

        LoadingFunction load = new LoadingFunction();

        private void disable()
        {
            cbltype.Enabled = false;
            cblterms.Enabled = false;
            cbmode.Enabled = false;
            tloanamt.Enabled = false;
            tprofee.Enabled = false;
            tinterest.Enabled = false;
            tnotfee.Enabled = false;
            tannfee.Enabled = false;
            tinsfee.Enabled = false;
            tvatfee.Enabled = false;
            tpenaltyfee.Enabled = false;
            tmiscfee.Enabled = false;
            tdocfee.Enabled = false;
        }

        private void enable()
        {
            cbltype.Enabled = true;
            cblterms.Enabled = true;
            cbmode.Enabled = true;
            tloanamt.Enabled = true;
            tprofee.Enabled = true;
            tinterest.Enabled = true;
            tnotfee.Enabled = true;
            tannfee.Enabled = true;
            tinsfee.Enabled = true;
            tvatfee.Enabled = true;
            tpenaltyfee.Enabled = true;
            tmiscfee.Enabled = true;
            tdocfee.Enabled = true;

        }

        private void ApplyFilters()
        {
            string searchText = tsearch.Text.ToLower();
            string paymentMode = cbmpayment.SelectedItem?.ToString();

            // Build MongoDB filter
            var filters = new List<FilterDefinition<BsonDocument>>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var textFilter = Builders<BsonDocument>.Filter.Or(
                    Builders<BsonDocument>.Filter.Regex("Type", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Principal", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Term", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Mode", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Processing Fee", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Interest Rate/Month", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Notarial Rate", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Annotation Rate", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Insurance Rate", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Vat Rate", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Penalty Rate", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Doc Rate", new BsonRegularExpression(searchText, "i")),
                    Builders<BsonDocument>.Filter.Regex("Misc. Rate", new BsonRegularExpression(searchText, "i"))
                );
                filters.Add(textFilter);
            }

            if (!string.IsNullOrWhiteSpace(paymentMode) && paymentMode != "ALL")
            {
                var modeFilter = Builders<BsonDocument>.Filter.Eq("Mode", paymentMode);
                filters.Add(modeFilter);
            }

            var combinedFilter = filters.Count > 0 ? Builders<BsonDocument>.Filter.And(filters) : Builders<BsonDocument>.Filter.Empty;

            // Retrieve data from MongoDB
            var documents = loanRateCollection.Find(combinedFilter).ToList();

            // Bind data to DataGridView
            DataTable dataTable = new DataTable();

            if (documents.Count > 0)
            {
                // Create columns based on the first document's elements
                foreach (var element in documents[0].Elements)
                {
                    dataTable.Columns.Add(element.Name);
                }

                // Add rows to the DataTable
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var element in doc.Elements)
                    {
                        if (element.Value.IsNumeric())
                        {
                            // Round numeric values to the nearest ones
                            row[element.Name] = Math.Round(element.Value.ToDouble(), 0);
                        }
                        else
                        {
                            row[element.Name] = element.Value.ToString();
                        }
                    }
                    dataTable.Rows.Add(row);
                }
            }

            dgvdata.DataSource = dataTable; // Bind the DataTable to the DataGridView

            if (dgvdata.Columns.Count > 0)
            {
                dgvdata.Columns[0].Visible = false; // Hide the first column
                lnorecord.Visible = false;
            }
        }

        private DataTable ReadExcelFile(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Use the first row as column headers
                        }
                    });
                    return result.Tables[0]; // Assuming the data is in the first sheet
                }
            }
        }

        private void SaveDataToMongoDB(DataTable dataTable)
        {
            pbloading.Minimum = 0;
            pbloading.Maximum = dataTable.Rows.Count;
            pbloading.Value = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                pbloading.Visible = true;
                var document = new BsonDocument();
                foreach (DataColumn column in dataTable.Columns)
                {
                    document[column.ColumnName] = BsonValue.Create(row[column.ColumnName]);
                }
                loanRateCollection.InsertOne(document);
                pbloading.Value += 1; // Update progress bar
            }
            pbloading.Visible = false;
        }

        private void buploadcsv_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    DataTable dataTable = ReadExcelFile(filePath);

                    load.Show(this);
                    Thread.Sleep(1000);
                    SaveDataToMongoDB(dataTable);
                    LoadDataToDataGridView();
                    load.Close();
                    MessageBox.Show("Data uploaded successfully!");
                }
            }
        }

        private void LoadDataToDataGridView()
        {
            var filter = Builders<BsonDocument>.Filter.Empty; // Retrieve all documents
            var documents = loanRateCollection.Find(filter).ToList();
            DataTable dataTable = new DataTable();

            if (documents.Count > 0)
            {
                // Create columns based on the first document's elements
                foreach (var element in documents[0].Elements)
                {
                    dataTable.Columns.Add(element.Name);
                }

                // Add rows to the DataTable
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var element in doc.Elements)
                    {
                        if (element.Value.IsNumeric())
                        {
                            if (element.Name == "Interest Rate/Month")
                            {
                                // Format Interest Rate/Month with percentage
                                row[element.Name] = Math.Round(element.Value.ToDouble(), 2) + "%";
                            }
                            else if (element.Name == "Penalty Rate")
                            {
                                // Format Interest Rate/Month with percentage
                                row[element.Name] = Math.Round(element.Value.ToDouble(), 2) + "%";
                            }

                            else
                            {
                                // Round numeric values to the nearest ones
                                row[element.Name] = Math.Round(element.Value.ToDouble(), 0);
                            }
                        }
                        else
                        {
                            if (element.Name == "Term")
                            {
                                // Convert the term value to an integer and append "month" or "months"
                                int termValue = int.Parse(element.Value.ToString());
                                row[element.Name] = termValue + (termValue == 1 ? " month" : " months");
                            }
                            else
                            {
                                row[element.Name] = element.Value.ToString();
                            }
                        }
                    }
                    dataTable.Rows.Add(row);
                }
            }

            dgvdata.DataSource = dataTable; // Bind the DataTable to the DataGridView

            if (dgvdata.Columns.Count > 0)
            {
                dgvdata.Columns[0].Visible = false; // Hide the first column
                lnorecord.Visible = false;
            }
        }


        private void frm_home_ADMIN_loanrates_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();

        }

        private void beditrate_Click(object sender, EventArgs e)
        {
            enable();
        }

        private void dgvdata_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvdata.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvdata.SelectedRows[0];

                // Map the DataGridView columns to the controls
                cbltype.SelectedItem = selectedRow.Cells["Type"].Value?.ToString();
                tloanamt.Text = selectedRow.Cells["Principal"].Value?.ToString();
                cblterms.SelectedItem = selectedRow.Cells["Term"].Value?.ToString();
                cbmode.SelectedItem = selectedRow.Cells["Mode"].Value?.ToString();
                tprofee.Text = selectedRow.Cells["Processing Fee"].Value?.ToString();
                tinterest.Text = selectedRow.Cells["Interest Rate/Month"].Value?.ToString();
                tnotfee.Text = selectedRow.Cells["Notarial Rate"].Value?.ToString();
                tannfee.Text = selectedRow.Cells["Annotation Rate"].Value?.ToString();
                tinsfee.Text = selectedRow.Cells["Insurance Rate"].Value?.ToString();
                tvatfee.Text = selectedRow.Cells["Vat Rate"].Value?.ToString();
                tpenaltyfee.Text = selectedRow.Cells["Penalty Rate"].Value?.ToString();
                tdocfee.Text = selectedRow.Cells["Doc Rate"].Value?.ToString();
                tmiscfee.Text = selectedRow.Cells["Misc. Rate"].Value?.ToString();
            }
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cbmpayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }
    }

    public static class BsonValueExtensions
    {
        public static bool IsNumeric(this BsonValue value)
        {
            return value.IsInt32 || value.IsInt64 || value.IsDouble || value.IsDecimal128;
        }

        public static double ToDouble(this BsonValue value)
        {
            return value.IsInt32 ? value.AsInt32 :
                   value.IsInt64 ? value.AsInt64 :
                   value.IsDouble ? value.AsDouble :
                   value.IsDecimal128 ? (double)value.AsDecimal128 :
                   0.0;
        }
    }
}
