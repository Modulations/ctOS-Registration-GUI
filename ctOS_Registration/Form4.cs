using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ctOS_Registration {
    public partial class Form4 : Form {
        public Form4() {
            InitializeComponent();
        }
        string safeFileName(string name) {
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
        private void SearchButton_Click(object sender, EventArgs e) {
            string name = textBox1.Text;
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";

            string fullpath = filepath + @"\" + safeFileName(name) + @".json";
            if (File.Exists(fullpath)) {
                string[] profile = ctOSDatabaseAccess.GetCTOSProfile(fullpath);
                Form2 f2 = new Form2();
                f2.SetBoxes(profile);
            }else {
                MessageBox.Show("Profile was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void importProfile_Click(object sender, EventArgs e) {
            string fileToImport = String.Empty;


            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JSON Files|*.json";
            fd.Title = "Please choose a .JSON profile to import.";
            bool fileChoosen = false;
            if (fd.ShowDialog() == DialogResult.OK) {
                fileToImport = fd.FileName;
                fileChoosen = true;
            }

            if (fileChoosen) {
                OpenFileDialog fd2 = new OpenFileDialog();
                fd2.Filter = "PNG Files|*.png";
                fd2.Title = "Please choose a PNG File for the imported profile's picture.";
                if (fd2.ShowDialog() == DialogResult.OK) {
                    Bitmap image = new Bitmap(fd2.FileName);
                    image.Save(@"temp.png", ImageFormat.Png);
                }

                bool fileError = false;
                string[] profileArray = ctOSDatabaseAccess.GetCTOSProfile(fileToImport);
                string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";
                string filename = filepath + @"\" + safeFileName(profileArray[0]) + @".json";
                string pictureDir = filepath + @"\Pictures";
                string pictureFilename = pictureDir + @"\" + safeFileName(profileArray[0]) + ".png";
                string pictureFileLocation = @"temp.png";

                try {
                    if (!Directory.Exists(filepath)) {
                        Directory.CreateDirectory(filepath);
                    }
                    if (!Directory.Exists(pictureDir)) {
                        Directory.CreateDirectory(pictureDir);
                    }
                    if (File.Exists(filename)) {
                        File.Delete(filename);
                    }
                    if (File.Exists(pictureFilename)) {
                        File.Delete(pictureFilename);
                    }
                    if (!File.Exists(pictureFileLocation)) {
                        Bitmap image = Properties.Resources.download;
                        image.Save(pictureFileLocation, ImageFormat.Png);
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fileError = true;
                }

                // 0 = name, 1 = gender, 2 = age, 3 = occupation, 4 = race, 5 = affiliations, 6 = salary, 7 = place of birth, 8 = threat level, 9 = Image Location
                JObject profileObj = new JObject(
                    new JProperty("Name", profileArray[0]),
                    new JProperty("Gender", profileArray[1]),
                    new JProperty("Place Of Birth", profileArray[7]),
                    new JProperty("Age", profileArray[2]),
                    new JProperty("Occupation", profileArray[3]),
                    new JProperty("Race", profileArray[4]),
                    new JProperty("Affiliations", profileArray[5]),
                    new JProperty("Salary", profileArray[6]),
                    new JProperty("Threat Level", profileArray[8]));
                string profileString = profileObj.ToString();

                try {
                    if (!fileError) {
                        File.WriteAllText(filename, profileString);
                        File.Copy(pictureFileLocation, pictureFilename);
                        File.Delete(pictureFileLocation);
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fileError = true;
                }

                try {
                    if (!fileError) {
                        string[] profile = ctOSDatabaseAccess.GetCTOSProfile(filename);
                        Form2 f2 = new Form2();
                        f2.SetBoxes(profile);
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    try {
                        if (File.Exists("temp.png")) File.Delete("temp.png");
                    } catch {
                        MessageBox.Show("Could not delete temp.png, please delete manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
