using System;
using System.Windows.Forms;
using System.IO;

namespace ctOS_Registration {
    public partial class ctOS_Welcome : Form
    {
        public ctOS_Welcome()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form2 f2 = new Form2();
            f2.ctOS_Registration();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("User error detected. Continuing survey.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Hide();
            Form2 f2 = new Form2();
            f2.ctOS_Registration();
            Close();
        }

        private void adminPanelButton_Click(object sender, EventArgs e) {
            string profilesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";
            string passDir = profilesDir + @"\Password";
            string filename = passDir + @"\password.json";
            if (File.Exists(filename)) {
                adminLoginPanel login = new adminLoginPanel();
                Hide();
                login.ShowDialog();
                Close();
            }else {
                if (!Directory.Exists(profilesDir)) {
                    try {
                        Directory.CreateDirectory(profilesDir);
                    }catch(Exception ex) {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (!Directory.Exists(passDir)) {
                    try {
                        Directory.CreateDirectory(passDir);
                    } catch (Exception ex) {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (!File.Exists(filename)) {
                    Form5 f5 = new Form5();
                    f5.ShowDialog();
                }
            }
        }
    }
}
