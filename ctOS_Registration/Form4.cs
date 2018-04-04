using System;
using System.Windows.Forms;
using System.IO;

namespace ctOS_Registration {
    public partial class Form4 : Form {
        public Form4() {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e) {
            string name = textBox1.Text.ToString();
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";

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
                MessageBox.Show("Path Exists", "here", MessageBoxButtons.OK, MessageBoxIcon.None);
                ctOSDatabaseAccess.ctOSDisplay(fullpath);
            }else {
                MessageBox.Show("Path Doesn't Exist", "here", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
    }
}
