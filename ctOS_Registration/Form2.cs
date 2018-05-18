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

        Bitmap ResizeImage(Image imageToChange, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(imageToChange.HorizontalResolution, imageToChange.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(imageToChange, destRect, 0, 0, imageToChange.Width, imageToChange.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        void CircularizeImage(PictureBox picture) {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, picture.Width, picture.Height);
            picture.Region = new Region(path);
        }

        public void ctOS_RegistrationPage() {
            Text = "ctOS User Registration";
            label8.Hide();
            threatBox.Hide();
            pictureBox2.Hide();
            Bitmap image = ResizeImage(Properties.Resources.download, 250, 250);
            pictureBox3.Image = image;
            CircularizeImage(pictureBox3);
            ShowDialog();
        }

        public void SetBoxes(string[] profile)
        {
            // 0 = name, 1 = gender, 2 = age, 3 = occupation, 4 = race, 5 = affiliations, 6 = salary, 7 = place of birth, 8 = threat level, 9 = Image Location

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
            if (profile[8].Contains(".")) {
                int index = profile[8].IndexOf('.');
                profile[8] = profile[8].Substring(0, index);
            }
            profile[8] += @"%";
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

            Bitmap image = new Bitmap(profile[9]);
            pictureBox3.Image = ResizeImage(image, 250, 250);
            CircularizeImage(pictureBox3);

            button1.Enabled = false;

            ShowDialog();
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
            
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Profiles";
            string filename = filepath + @"\" + safeFileName(nameBox) + @".json";
            string pictureDir = filepath + @"\Pictures";
            string pictureFilename = pictureDir + @"\" + safeFileName(nameBox) + ".png";
            string pictureFileLocation = @"temp.png";

            try {
                if (!Directory.Exists(filepath)) {
                    Directory.CreateDirectory(filepath);
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
            } finally { }

            string name = sterilizeTextBoxText(nameBox);
            string placeOfBirth = sterilizeTextBoxText(birthBox);
            string age = sterilizeTextBoxText(ageBox);
            string dateOfBirth = sterilizeTextBoxText(dobBox);
            string occupation = sterilizeTextBoxText(occBox);
            string race = sterilizeTextBoxText(raceBox);
            string affiliations = sterilizeTextBoxText(affBox);
            string salary = sterilizeTextBoxText(salBox);
            string aliases = sterilizeTextBoxText(aliBox);
            string specs = sterilizeTextBoxText(specBox);
            string gender;
            try {
                gender = genderBox.SelectedItem.ToString();
            } catch (Exception ex) {
                string exc = ex.ToString();
                MessageBox.Show("Please Select a Valid Option.", "Gender", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gender = "Error";
                fileError = true;
            }

            JObject profile = new JObject(
                new JProperty("Name", name),
                new JProperty("Gender", gender),
                new JProperty("Place Of Birth", placeOfBirth),
                new JProperty("Date Of Birth", dateOfBirth),
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
                if(!fileError) {
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
                    string[] profileArray = ctOSDatabaseAccess.GetCTOSProfile(filename);
                    Form2 f2 = new Form2();
                    f2.SetBoxes(profileArray);
                    int p = (int) Environment.OSVersion.Platform;
                    if((p == 4) || (p == 6) || (p == 128)) { // Running on Unix
                        System.Diagnostics.Process.Start("xdg-open", filename);
                    } else { // Running on not Unix
                        System.Diagnostics.Process.Start(@"c:\Windows\notepad.exe", filename);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }finally {
                try {
                    if (File.Exists("temp.png")) File.Delete("temp.png");
                }catch {
                    MessageBox.Show("Could not delete temp.png, please delete manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (!fileError) {
                    MessageBox.Show("Thank you for cooperating, citizen.", "Thank you.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e) {
            bool choosenPicture = false;
            string pictureLocation = String.Empty;

            MessageBox.Show("Please choose a PNG file for your profile picture.\n(Make sure that the image is square, otherwise stretching will occur)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "PNG Files|*.png";
            fd.Title = "Please choose a PNG for your profile picture.";
            if (fd.ShowDialog() == DialogResult.OK) {
                pictureLocation = fd.FileName;
                choosenPicture = true;
            } else {
                choosenPicture = false;
            }

            Bitmap image = choosenPicture ? new Bitmap(pictureLocation) : Properties.Resources.download;
            
            image.Save("temp.png", ImageFormat.Png);
            pictureBox3.Hide();
            pictureBox3.Image = ResizeImage(image, 250, 250);
            CircularizeImage(pictureBox3);
            pictureBox3.Show();
        }
    }
}
