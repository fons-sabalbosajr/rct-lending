using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Windows.Forms;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_collections : Form
    {
        private string _loanId;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private DataTable _loanCollectionTable;

        public frm_home_disburse_collections(string loanId)
        {
            InitializeComponent();
            _loanId = loanId;

            // MongoDB connection initialization
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_collections");

            dtdate.Value = DateTime.Now;
        }

        private void LoadLoanCollections()
        {
            // Query to get the loan collections based on LoanID
            var filter = Builders<BsonDocument>.Filter.Eq("LoanID", _loanId);
            var loanCollections = _loanDisbursedCollection.Find(filter).ToList();

            // Convert MongoDB data to a DataTable for display
            DataTable dt = new DataTable();
            dt.Columns.Add("CollectionID");
            dt.Columns.Add("DateCollected");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Collector");

            foreach (var collection in loanCollections)
            {
                dt.Rows.Add(
                    collection["CollectionID"].AsString,
                    collection["DateCollected"].ToUniversalTime().ToString("yyyy-MM-dd"),
                    collection["Amount"].AsDouble.ToString("F2"),
                    collection["Collector"].AsString
                );
            }

            // Bind data to DataGridView
            dgvdata.DataSource = dt;

            if (dgvdata.Rows.Count == 0)
            {
                // Show lnorecord label if no data
                lnorecord.Visible = true;
            }
            else
            {
                // Hide lnorecord label and show DataGridView if data exists
                lnorecord.Visible = false;
                AddRowButtons();
            }
        }

        private void AddRowButtons()
        {
            // Clear any existing button columns
            dgvdata.Columns.Clear();
            LoadLoanCollections();

            // Add "View" button column
            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            btnView.HeaderText = "View";
            btnView.Text = "View";
            btnView.UseColumnTextForButtonValue = true;
            dgvdata.Columns.Add(btnView);

            // Add "Print" button column
            DataGridViewButtonColumn btnPrint = new DataGridViewButtonColumn();
            btnPrint.HeaderText = "Print";
            btnPrint.Text = "Print";
            btnPrint.UseColumnTextForButtonValue = true;
            dgvdata.Columns.Add(btnPrint);

            // Add "Delete" button column
            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "Delete";
            btnDelete.Text = "Delete";
            btnDelete.UseColumnTextForButtonValue = true;
            dgvdata.Columns.Add(btnDelete);
        }

      
        private void ViewCollectionDetails(string collectionId)
        {
            // Display details for the selected collection
            MessageBox.Show($"Viewing details for Collection ID: {collectionId}");
        }

        private void PrintCollectionDetails(string collectionId)
        {
            // Print logic for the selected collection
            MessageBox.Show($"Printing details for Collection ID: {collectionId}");
        }

        private void DeleteCollection(string collectionId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("CollectionID", collectionId);
            var result = _loanDisbursedCollection.DeleteOne(filter);

            if (result.DeletedCount > 0)
            {
                MessageBox.Show($"Collection ID: {collectionId} has been deleted.");
                // Reload the data after deletion
                LoadLoanCollections();
            }
            else
            {
                MessageBox.Show($"Failed to delete Collection ID: {collectionId}");
            }
        }

        private void SearchInDataGrid(string keyword)
        {
            DataView dv = _loanCollectionTable.DefaultView;
            string filter = string.Empty;

            // Loop through all columns to search in any column
            foreach (DataColumn col in _loanCollectionTable.Columns)
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " OR "; // Add OR between column filters
                }

                // Using LIKE to perform partial matching in each column
                filter += $"{col.ColumnName} LIKE '%{keyword}%'";
            }

            // Apply filter to the DataView
            dv.RowFilter = filter;

            // Update DataGridView with filtered results
            dgvdata.DataSource = dv;

            // Check if any rows are displayed after filtering
            if (dgvdata.Rows.Count == 0)
            {
                lnorecord.Visible = true;
                
            }
            else
            {
                lnorecord.Visible = false;
               
            }
        }

        private void frm_home_disburse_collections_Load(object sender, EventArgs e)
        {
            laccountid.Text = _loanId;
            LoadLoanCollections();
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string collectionId = dgvdata.Rows[e.RowIndex].Cells["CollectionID"].Value.ToString();

                // Check which button was clicked based on the column index
                if (dgvdata.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.ColumnIndex == dgvdata.Columns["View"].Index)
                {
                    ViewCollectionDetails(collectionId);
                }
                else if (dgvdata.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.ColumnIndex == dgvdata.Columns["Print"].Index)
                {
                    PrintCollectionDetails(collectionId);
                }
                else if (dgvdata.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.ColumnIndex == dgvdata.Columns["Delete"].Index)
                {
                    DeleteCollection(collectionId);
                }
            }
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            SearchInDataGrid(tsearch.Text);
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void bnew_Click(object sender, EventArgs e)
        {
            frm_home_disburse_collections_add add = new frm_home_disburse_collections_add();
            add.ShowDialog(this);
        }
    }
}
