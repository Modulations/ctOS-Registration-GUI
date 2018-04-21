using System;
using System.Windows.Forms;
using System.IO;

namespace ctOS_Registration {
    public partial class Form4 : Form {
        public Form4() {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e) {
            string name = textBox1.Text;
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ctOS_Registration\Profiles";

            string safeFileName(string filename) {
                return name
                    .Replace("\\", "")
                    .Replace("/", "")
                    .Replace("\"", "")
                    .Replace("*", "")
                    .Replace(":", "")
                    .Replace("?", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("|", "")
                    .Replace(" ", "_");
            }
            string fullpath = filepath + @"\" + safeFileName(name) + @".json";
            if (File.Exists(fullpath)) {
                ctOSDatabaseAccess.ctOSDisplay(fullpath);
            }else {
                MessageBox.Show("Profile was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) {

        }
    }
}
