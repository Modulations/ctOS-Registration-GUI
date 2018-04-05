using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ctOS_Registration {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
        }

        public void ctOS_Registration() {
            Text = "ctOS User Registration";
            label8.Hide();
            threatBox.Hide();
            pictureBox2.Hide();
            ShowDialog();
        }

        public void SetBoxes(string[] profile)
        {
            // 0 = name, 1 = gender, 2 = age, 3 = occupation, 4 = race, 5 = affiliations, 6 = salary, 7 = place of birth, 8 = threat level

            Text = "Profile of User: " + profile[0];

            nameBox.AppendText(profile[0]);
            genderBox.Text = profile[1];
            ageBox.AppendText(profile[2]);
            occBox.AppendText(profile[3]);
            raceBox.AppendText(profile[4]);
            affBox.AppendText(profile[5]);
            salBox.AppendText(profile[6]);
            birthBox.AppendText(profile[7]);
            threatBox.Enabled = true;
            threatBox.AppendText(profile[8]);

            nameBox.Enabled = false;
            genderBox.Enabled = false;
            ageBox.Enabled = false;
            occBox.Enabled = false;
            raceBox.Enabled = false;
            affBox.Enabled = false;
            salBox.Enabled = false;
            birthBox.Enabled = false;
            threatBox.Enabled = false;

            button1.Enabled = false;

            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) {
            bool fileError = false;
            double GetRandomNumber(double minimum, double maximum)
            {
                Random random = new Random();
                return random.NextDouble() * (maximum - minimum) + minimum;
            }
            string sterilizeTextBoxText(TextBox boxText)
            {
                string text = boxText.ToString();

                return text.Replace("System.Windows.Forms.TextBox, Text: ", "");
            }
            string safeFileName(TextBox boxText)
            {
                Name = sterilizeTextBoxText(boxText);
                return Name
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
            //string dateOfBirth = sterilizeTextBoxText(dobBox);
            string occupation = sterilizeTextBoxText(occBox);
            string race = sterilizeTextBoxText(raceBox);
            string affiliations = sterilizeTextBoxText(affBox);
            string salary = sterilizeTextBoxText(salBox);
            double threatLevel = GetRandomNumber(0.00, 100.00);
            //string aliases = sterilizeTextBoxText(aliBox);
            //string specs = sterilizeTextBoxText(specBox);
            string gender;
            try {
                if (genderBox.SelectedItem.ToString() == String.Empty) {
                    MessageBox.Show("Please Select a Valid Option.", "Gender", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gender = "Error";
                    fileError = true;
                }else {
                    gender = genderBox.SelectedItem.ToString();
                }
            } catch (Exception ex) {
                MessageBox.Show("Please Select a Valid Option.", "Gender", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gender = "Error";
                fileError = true;
            }

            JObject profile = new JObject(
                new JProperty("Name", name),
                new JProperty("Gender", gender),
                new JProperty("Place Of Birth", placeOfBirth),
                //new JProperty("Date Of Birth", dateOfBirth),
                new JProperty("Age", age),
                new JProperty("Occupation", occupation),
                new JProperty("Race", race),
                new JProperty("Affiliations", affiliations),
                new JProperty("Salary", salary),
                new JProperty("Threat Level", threatLevel.ToString()));
                //new JProperty("Aliases", aliases),
                //new JProperty("Specializations", specs));

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
                if(!fileError) File.WriteAllText(filename, profileString); // File.WriteAllText(filename, HashedSHA256(profileString)) for Hashed or File.WriteAllText(filename, profileString)
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
            }finally {
                if(!fileError) {
                    MessageBox.Show("Thank you for cooperating, citizen.", "Thank you.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
        }
    }
}