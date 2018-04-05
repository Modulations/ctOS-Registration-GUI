using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ctOS_Registration {
    public partial class adminLoginPanel : Form {
        public adminLoginPanel() {
            InitializeComponent();
        }

        private void LoginCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void LoginConfirm_Click(object sender, EventArgs e) {
            bool error = false;

            string passDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles\Password";
            string filename = passDir + @"\password.json";
            if (!File.Exists(filename)) {
                JObject p = new JObject(
                    new JProperty("AdminPassword", "password"));
                try {
                    File.WriteAllText(filename, p.ToString());
                }catch(Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    error = true;
                }
            }
            JObject password;
            if (!error) {
                password = JObject.Parse(File.ReadAllText(filename));
            } else password = new JObject();

            string GetJObjectValue(JObject array, string key) {
                foreach (KeyValuePair<string, JToken> keyValuePair in array) {
                    if (key == keyValuePair.Key) {
                        return keyValuePair.Value.ToString();
                    }
                }
                if (!error) MessageBox.Show("Error, KeyValue pair not found for key: " + key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return String.Empty;
            }

            if(textBox1.Text.ToString() == GetJObjectValue(password, "AdminPassword") && !error) {
                MessageBox.Show("Password \"" + GetJObjectValue(password, "AdminPassword") + "\" is correct.", "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.None);
                Form4 f4 = new Form4();
                Hide();
                f4.ShowDialog();
                Close();
            }
        }
    }
}
