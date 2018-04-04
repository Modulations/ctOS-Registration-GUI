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
    public partial class adminLoginPanel : Form {
        public adminLoginPanel() {
            InitializeComponent();
        }

        private void LoginCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void LoginConfirm_Click(object sender, EventArgs e) {
            const string filename = @"adminpass.json";
            if (!File.Exists(filename)) {
                JObject p = new JObject(
                    new JProperty("AdminPassword", "password"));
                File.WriteAllText(filename, p.ToString());
            }
            JObject password = JObject.Parse(File.ReadAllText(filename));

            string GetJObjectValue(JObject array, string key) {
                foreach(KeyValuePair<string, JToken> keyValuePair in array) {
                    if (key == keyValuePair.Key) {
                        return keyValuePair.Value.ToString();
                    }
                }
                MessageBox.Show("Error, KeyValue pair not found for key: " + key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return String.Empty;
            }

            if(textBox1.Text.ToString() == GetJObjectValue(password, "AdminPassword")) {
                MessageBox.Show("Password \"" + GetJObjectValue(password, "AdminPassword") + "\" is correct.", "Access Granted", MessageBoxButtons.OK, MessageBoxIcon.None);
                Form4 f4 = new Form4();
                Hide();
                f4.ShowDialog();
                Close();
            }
        }
    }
}
