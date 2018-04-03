using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ctOS_Registration {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            bool fileError = false;

            string sterilizeTextBoxText(TextBox boxText)
            {
                string text = boxText.ToString();

                return text.Replace("System.Windows.Forms.TextBox, Text: ", "");
            }
            string safeFileName(TextBox boxText)
            {
                Name = sterilizeTextBoxText(boxText);
                return Name.Replace(" ", "_");
            }

            string profilesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";
            string filename = profilesDir + @"\" + safeFileName(nameBox) + @".json";

            try {
                if (!Directory.Exists(profilesDir)) {
                    Directory.CreateDirectory(profilesDir);
                }
                if (File.Exists(filename)) {
                    File.Delete(filename);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileError = true;
            } finally { }

            string name = sterilizeTextBoxText(nameBox);
            string placeOfBirth = sterilizeTextBoxText(birthBox);
            string age = sterilizeTextBoxText(ageBox);
            string occupation = sterilizeTextBoxText(occBox);
            string race = sterilizeTextBoxText(raceBox);
            string affiliations = sterilizeTextBoxText(affBox);
            string salary = sterilizeTextBoxText(salBox);
            string aliases = sterilizeTextBoxText(aliBox);
            string specs = sterilizeTextBoxText(specBox);
            string gender = genderBox.SelectedItem.ToString();

            JObject profile = new JObject(
                new JProperty("Name", name),
                new JProperty("Gender", gender),
                new JProperty("Place Of Birth", placeOfBirth),
                new JProperty("Age", age),
                new JProperty("Occupation", occupation),
                new JProperty("Race", race),
                new JProperty("Affiliations", affiliations),
                new JProperty("Salary", salary),
                new JProperty("Aliases", aliases),
                new JProperty("Specializations", specs));

            string profileString = profile.ToString();

            /*string HashedSHA256(string text) {
                try {
                    byte[] bytes = Encoding.UTF8.GetBytes(text);
                    SHA256Managed hashstring = new SHA256Managed();
                    byte[] hash = hashstring.ComputeHash(bytes);
                    string hashString = string.Empty;
                    foreach (byte x in hash) {
                        hashString += String.Format("{0:x2}", x);
                    }
                    return hashString;
                }catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "";
                }finally {}
            }*/

            try {
                File.WriteAllText(filename, profileString); // File.WriteAllText(filename, HashedSHA256(profileString)) for Hashed or File.WriteAllText(filename, profileString)
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileError = true;
            } finally { }

            try {
                if (!fileError) {
                    System.Diagnostics.Process.Start(@"c:\Windows\notepad.exe", filename);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}