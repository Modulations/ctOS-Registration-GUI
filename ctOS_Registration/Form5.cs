using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ctOS_Registration {
    public partial class Form5 : Form {
        public Form5() {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void ConfirmButton_Click(object sender, EventArgs e) {
            if (passBox.Text.ToString() == passBox2.Text.ToString()) {
                bool error = false;

                string passFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles\Password\password.json";
                string password = passBox.Text.ToString();

                JObject passwordObj = new JObject(
                    new JProperty("AdminPassword", password));
                try {
                    File.AppendAllText(passFile, passwordObj.ToString());
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    error = true;
                }
                if (!error) {
                    MessageBox.Show("Thank you. Please try to enter the Admin Panel again to use your new access code.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }else {
                MessageBox.Show("Passwords are not the same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
