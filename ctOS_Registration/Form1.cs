using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ctOS_Registration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.ctOS_RegistrationPage();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("User error detected. Continuing survey.");
            this.Hide();
            Form2 f2 = new Form2();
            f2.ctOS_RegistrationPage();
            Show();
        }

        private void adminPanelButton_Click(object sender, EventArgs e) {
            string profilesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";
            string passDir = profilesDir + @"\Password";
            string filename = passDir + @"\password.json";
            if (File.Exists(filename)) {
                adminLoginPanel login = new adminLoginPanel();
                Hide();
                login.ShowDialog();
                Show();
            }else {
                if (!Directory.Exists(profilesDir)) {
                    try {
                        Directory.CreateDirectory(profilesDir);
                        Directory.CreateDirectory(passDir);
                    } catch(Exception ex) {
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
