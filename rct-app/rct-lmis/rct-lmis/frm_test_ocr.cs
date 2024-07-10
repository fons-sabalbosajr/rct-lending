using System;
using System.Drawing;
using System.Windows.Forms;
using Tesseract;

namespace rct_lmis
{
    public partial class frm_test_ocr : Form
    {
        public frm_test_ocr()
        {
            InitializeComponent();
        }

        private void frm_test_ocr_Load(object sender, EventArgs e)
        {

        }

        private void bupload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    pbphoto.Image = Image.FromFile(filePath);
                    tpath.Text = filePath;
                }
            }
        }

        private void bgetdata_Click(object sender, EventArgs e)
        {
            string filePath = tpath.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please upload an image first.");
                return;
            }

            string extractedText = PerformOCR(filePath);
            ExtractSpecificInformation(extractedText);
        }

        private string PerformOCR(string imagePath)
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return string.Empty;
            }
        }

        private void ExtractSpecificInformation(string text)
        {
            // Example: Extract Name using a simple regex
            var nameMatch = System.Text.RegularExpressions.Regex.Match(text, @"Name:\s*(.*)");
            if (nameMatch.Success)
            {
                tname.Text = nameMatch.Groups[1].Value.Trim();
            }

            var addressMatch = System.Text.RegularExpressions.Regex.Match(text, @"Address:\s*(.*)");
            if (addressMatch.Success)
            {
                taddress.Text = addressMatch.Groups[1].Value.Trim();
            }

            var ageMatch = System.Text.RegularExpressions.Regex.Match(text, @"Age:\s*(\d+)");
            if (ageMatch.Success)
            {
                tage.Text = ageMatch.Groups[1].Value.Trim();
            }

            var statusMatch = System.Text.RegularExpressions.Regex.Match(text, @"Status:\s*(.*)");
            if (statusMatch.Success)
            {
                cbstatus.Text = statusMatch.Groups[1].Value.Trim();
            }
        }
    }
}
