using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using ExcelDataReader;
using Guna.UI2.WinForms;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_loanrates : Form
    {
        private readonly IMongoCollection<BsonDocument> loanRateCollection;

        public frm_home_ADMIN_loanrates()
        {
            InitializeComponent();

            // Initialize MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");
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
            MessageBox.Show("Data uploaded successfully!");
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
                    SaveDataToMongoDB(dataTable);
                    LoadDataToDataGridView();
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

        private void frm_home_ADMIN_loanrates_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();

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
