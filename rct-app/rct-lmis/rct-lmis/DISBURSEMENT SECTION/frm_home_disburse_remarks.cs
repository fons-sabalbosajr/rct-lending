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

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_remarks : Form
    {

        private string _collectionId;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;

        public frm_home_disburse_remarks(string collectionId)
        {
            InitializeComponent();

            _collectionId = collectionId;

            // MongoDB connection initialization
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_collections");

        }

        private void frm_home_disburse_remarks_Load(object sender, EventArgs e)
        {

        }

        private void bsave_Click(object sender, EventArgs e)
        {
            // Retrieve the entered remark
            string remarkText = tremarks.Text.Trim();

            if (string.IsNullOrEmpty(remarkText))
            {
                MessageBox.Show("Please enter a remark.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prepare the update for the remark
            var filter = Builders<BsonDocument>.Filter.Eq("CollectionID", _collectionId);
            var update = Builders<BsonDocument>.Update.Push("Remarks", remarkText); // Assuming "Remarks" is an array

            try
            {
                // Update the document in the MongoDB collection
                _loanDisbursedCollection.UpdateOne(filter, update);

                MessageBox.Show("Remark added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Indicate success
                this.Close(); // Close the form
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the remark: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
