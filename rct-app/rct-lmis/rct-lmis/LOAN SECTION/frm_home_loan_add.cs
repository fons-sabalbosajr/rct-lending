using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_add : Form
    {
        public frm_home_loan_add()
        {
            InitializeComponent();
            bsavetransaction.Enabled = false;
            InitializeDataGridView();

            SetupAutoCompleteProvince();
        }

        private List<string> filePaths = new List<string>();

        private void InitializeDataGridView()
        {
            dgvuploads.AutoGenerateColumns = false;
            dgvuploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                Width = 300
            });
            dgvuploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Size",
                HeaderText = "Size",
                DataPropertyName = "Size",
                Width = 150
            });
            dgvuploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 150
            });
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn()
            {
                Name = "Delete",
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true, // This will show "Delete" on all buttons
                Width = 100
            };
            dgvuploads.Columns.Add(deleteButtonColumn);
        }

        private void SetupAutoCompleteProvince()
        {
            // List of provinces in the Philippines
            string[] provinces = new string[]
            {
                "Abra", "Agusan del Norte", "Agusan del Sur", "Aklan", "Albay",
                "Antique", "Apayao", "Aurora", "Basilan", "Bataan", "Batanes",
                "Batangas", "Benguet", "Biliran", "Bohol", "Bukidnon",
                "Bulacan", "Cagayan", "Camarines Norte", "Camarines Sur",
                "Camiguin", "Capiz", "Catanduanes", "Cavite", "Cebu",
                "Compostela Valley", "Cotabato", "Davao del Norte", "Davao del Sur",
                "Davao Occidental", "Davao Oriental", "Dinagat Islands", "Eastern Samar",
                "Guimaras", "Ifugao", "Ilocos Norte", "Ilocos Sur", "Iloilo",
                "Isabela", "Kalinga", "La Union", "Laguna", "Lanao del Norte",
                "Lanao del Sur", "Leyte", "Maguindanao", "Marinduque", "Masbate",
                "Misamis Occidental", "Misamis Oriental", "Mountain Province",
                "Negros Occidental", "Negros Oriental", "Northern Samar",
                "Nueva Ecija", "Nueva Vizcaya", "Occidental Mindoro", "Oriental Mindoro",
                "Palawan", "Pampanga", "Pangasinan", "Quezon", "Quirino",
                "Rizal", "Romblon", "Samar", "Sarangani", "Siquijor",
                "Sorsogon", "South Cotabato", "Southern Leyte", "Sultan Kudarat",
                "Sulu", "Surigao del Norte", "Surigao del Sur", "Tarlac",
                "Tawi-Tawi", "Zambales", "Zamboanga del Norte", "Zamboanga del Sur",
                "Zamboanga Sibugay"
            };

            // Create an AutoCompleteStringCollection and add the provinces to it
            AutoCompleteStringCollection autoCompleteProvinces = new AutoCompleteStringCollection();
            autoCompleteProvinces.AddRange(provinces);

            // Set up the textbox for autocomplete
            tbrprovince.AutoCompleteCustomSource = autoCompleteProvinces;
            tbrprovince.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbrprovince.AutoCompleteSource = AutoCompleteSource.CustomSource;


            tbrprovpr.AutoCompleteCustomSource = autoCompleteProvinces;
            tbrprovpr.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbrprovpr.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void frm_home_loan_add_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void frm_home_loan_add_Load(object sender, EventArgs e)
        {

        }

        private string FormatFileSize(long fileSize)
        {
            if (fileSize >= 1073741824)
                return string.Format("{0:##.##} GB", fileSize / 1073741824.0);
            else if (fileSize >= 1048576)
                return string.Format("{0:##.##} MB", fileSize / 1048576.0);
            else if (fileSize >= 1024)
                return string.Format("{0:##.##} KB", fileSize / 1024.0);
            else
                return string.Format("{0} Bytes", fileSize);
        }

        private void baddfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    filePaths.Add(fileName);

                    FileInfo fileInfo = new FileInfo(fileName);
                    long fileSize = fileInfo.Length; // File size in bytes

                    string fileSizeString = FormatFileSize(fileSize); // Convert to readable format

                    dgvuploads.Rows.Add(Path.GetFileName(fileName), fileSizeString, "Ready to Upload", "Action");
                    lnofile.Visible = false;
                }
            }
        }

        private void bclear_Click(object sender, EventArgs e)
        {
            dgvuploads.Rows.Clear();
            filePaths.Clear();
        }

        private void tbrprovince_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvuploads_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the delete button column
            if (e.ColumnIndex == dgvuploads.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Ensure there are rows in the DataGridView
                if (dgvuploads.Rows.Count > 0 && e.RowIndex < dgvuploads.Rows.Count)
                {
                    // Show confirmation dialog
                    var confirmResult = MessageBox.Show("Are you sure to delete this file?",
                                                        "Confirm Delete",
                                                        MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // Ensure the index is valid before removing from filePaths
                        if (e.RowIndex >= 0 && e.RowIndex < filePaths.Count)
                        {
                            // Remove the corresponding file path from the list
                            filePaths.RemoveAt(e.RowIndex);
                        }

                        // Remove the row from the DataGridView
                        dgvuploads.Rows.RemoveAt(e.RowIndex);

                        // Optionally, check if there are no more files and update the lnofile label visibility
                        if (filePaths.Count == 0)
                        {
                            lnofile.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
