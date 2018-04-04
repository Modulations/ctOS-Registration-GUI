using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.IO;

namespace ctOS_Registration {
    class ctOSDatabaseAccess {
        public static void ctOSDisplay(string filepath) {
            bool error = false;
            string GetJObjectValue(JObject array, string key)
            {
                foreach (KeyValuePair<string, JToken> keyValuePair in array) {
                    if (key == keyValuePair.Key) {
                        return keyValuePair.Value.ToString();
                    }
                }
                MessageBox.Show("Error, KeyValue pair not found for key: " + key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return String.Empty;
            }
            JObject profile;
            try {
                profile = JObject.Parse(File.ReadAllText(filepath));
            }catch(Exception e) {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
                profile = new JObject();
            }

            if(!error) {
                string name = GetJObjectValue(profile, "Name");
                string gender = GetJObjectValue(profile, "Gender");
                string age = GetJObjectValue(profile, "Age");
                string occupation = GetJObjectValue(profile, "Occupation");
                string race = GetJObjectValue(profile, "Race");
                string affiliations = GetJObjectValue(profile, "Affiliations");
                string salary = GetJObjectValue(profile, "Salary");
                string threatLevel = GetJObjectValue(profile, "Threat Level");

                MessageBox.Show(name + "\n" + gender + "\n" + age + "\n" + occupation + "\n" + race + "\n" + affiliations + "\n" + salary + "\n" + threatLevel, "Profile Of " + name, MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

    }
}
