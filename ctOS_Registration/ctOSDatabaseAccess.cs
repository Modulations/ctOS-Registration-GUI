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
                if (key == "Threat Level") {
                    return "No Threat Level Data Found";
                }else {
                    MessageBox.Show("Error, KeyValue pair not found for key: " + key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return String.Empty;
                }
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

                string[] info = { name, gender, age, occupation, race, affiliations, salary, threatLevel }; // 0 = name, 1 = gender, 2 = age, 3 = occupation, 4 = race, 5 = affiliations, 6 = salary, 7 = threat level

                Form5 f5 = new Form5();
                f5.SetBoxes(info);
            }
        }

    }
}
