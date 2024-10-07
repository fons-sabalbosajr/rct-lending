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

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_routes : Form
    {

        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> loanCollectorsCollection;
        private IMongoCollection<BsonDocument> idCounterCollection;


        public frm_home_ADMIN_routes()
        {
            InitializeComponent();
            database = MongoDBConnection.Instance.Database; // Get the MongoDB database instance
            loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_area_routes");
            idCounterCollection = database.GetCollection<BsonDocument>("id_counter");
        }

        private void SaveDataToDatabase()
        {
            int newIdNo = GetNextIdNo(); // Get the next IDNo

            var document = new BsonDocument
            {
                { "IDNo", newIdNo }, // Use the new IDNo
                { "Area", tarea.Text },
                { "Remarks", tremarks.Text },
            };

            try
            {
                loanCollectorsCollection.InsertOne(document); // Save the document to MongoDB
                //MessageBox.Show("Data saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int GetNextIdNo()
        {
            var idCounterDocument = idCounterCollection.Find(new BsonDocument()).FirstOrDefault();

            if (idCounterDocument == null)
            {
                // If no document exists, initialize the counter at 1
                idCounterDocument = new BsonDocument { { "LastId", 1 } };
                idCounterCollection.InsertOne(idCounterDocument);
                return 1;
            }
            else
            {
                // Increment the last used IDNo
                int lastId = idCounterDocument["LastId"].AsInt32;
                int newId = lastId + 1;

                // Update the counter in the collection
                var update = Builders<BsonDocument>.Update.Set("LastId", newId);
                idCounterCollection.UpdateOne(new BsonDocument(), update); // Update the document

                return newId; // Return the new IDNo
            }
        }

        private void UpdateDataInDatabase()
        {
            var idToUpdate = tidno.Text; // Get the IDNo from the text box
            var filter = Builders<BsonDocument>.Filter.Eq("IDNo", idToUpdate); // Create a filter for the document to update

            var update = Builders<BsonDocument>.Update
                .Set("Area", tarea.Text)
                .Set("Remarks", tremarks.Text);

            try
            {
                var result = loanCollectorsCollection.UpdateOne(filter, update); // Update the document in MongoDB

                if (result.ModifiedCount > 0)
                {
                    MessageBox.Show("Data updated successfully!");
                }
                else
                {
                    MessageBox.Show("No document found with the specified IDNo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                var documents = loanCollectorsCollection.Find(new BsonDocument()).ToList(); // Retrieve all documents

                dgvarea.DataSource = documents.Select(d => new
                {
                    Area = d["Area"].AsString,
                    Remarks = d["Remarks"].AsString,
                    IDNo = d["IDNo"].AsInt32 // Ensure IDNo is an integer
                }).ToList(); // Map documents to an anonymous object

                dgvarea.Refresh(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteDataFromDatabase()
        {
            var idToDelete = tidno.Text; // Get the IDNo from the text box
            var filter = Builders<BsonDocument>.Filter.Eq("IDNo", idToDelete); // Create a filter for the document to delete

            try
            {
                var result = loanCollectorsCollection.DeleteOne(filter); // Delete the document in MongoDB

                if (result.DeletedCount > 0)
                {
                    MessageBox.Show("Data deleted successfully!");
                }
                else
                {
                    MessageBox.Show("No document found with the specified IDNo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ResetIDNo()
        {
            // Step 1: Reset the ID counter to 1
            var idCounterDocument = idCounterCollection.Find(new BsonDocument()).FirstOrDefault();

            if (idCounterDocument != null)
            {
                var update = Builders<BsonDocument>.Update.Set("LastId", 1);
                idCounterCollection.UpdateOne(new BsonDocument(), update);
            }
            else
            {
                // If the counter does not exist, create it with LastId = 1
                var newCounterDocument = new BsonDocument { { "LastId", 1 } };
                idCounterCollection.InsertOne(newCounterDocument);
            }

            // Step 2: Update the existing documents in loan_area_routes
            var documents = loanCollectorsCollection.Find(new BsonDocument()).ToList();
            int newId = 1;

            foreach (var document in documents)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]); // Use the document's original _id for the update

                var update = Builders<BsonDocument>.Update.Set("IDNo", newId);
                loanCollectorsCollection.UpdateOne(filter, update);

                newId++; // Increment for the next document
            }

            MessageBox.Show("IDNo reset successfully to start from 1!");
        }


        private void bedit_Click(object sender, EventArgs e)
        {
            tarea.Enabled = true;
            tremarks.Enabled = true;
            badd.Enabled = true;
        }

        private void frm_home_ADMIN_routes_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void badd_Click(object sender, EventArgs e)
        {
            SaveDataToDatabase();
            LoadDataIntoDataGridView();
            ResetIDNo();
        }

        private void bupdate_Click(object sender, EventArgs e)
        {
            UpdateDataInDatabase();
            LoadDataIntoDataGridView();
        }

        private void bdel_Click(object sender, EventArgs e)
        {
            DeleteDataFromDatabase();
            LoadDataIntoDataGridView();
        }

        private void breset_Click(object sender, EventArgs e)
        {
            ResetIDNo();
        }
    }
}
