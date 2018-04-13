﻿using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

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

        public void ctOS_RegistrationPage() {
            Text = "ctOS User Registration";
            label8.Hide();
            threatBox.Hide();
            pictureBox2.Hide();
            Bitmap image = ResizeImage(Properties.Resources.download, 250, 250);
            pictureBox3.Image = image;
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Region = new Region(path);
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
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Region = new Region(path);

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
            string pictureDir = profilesDir + @"\Pictures";
            string pictureFilename = pictureDir + @"\" + safeFileName(nameBox) + ".png";
            string pictureFileLocation = @"temp.png";

            try {
                if (!Directory.Exists(profilesDir)) {
                    Directory.CreateDirectory(profilesDir);
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
                if (!File.Exists("temp.png")) {
                    Bitmap image = Properties.Resources.download;
                    image.Save("temp.png", ImageFormat.Png);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileError = true;
            }

            string name = sterilizeTextBoxText(nameBox);
            string placeOfBirth = sterilizeTextBoxText(birthBox);
            string age = sterilizeTextBoxText(ageBox);
            string occupation = sterilizeTextBoxText(occBox);
            string race = sterilizeTextBoxText(raceBox);
            string affiliations = sterilizeTextBoxText(affBox);
            string salary = sterilizeTextBoxText(salBox);
            double threatLevel = GetRandomNumber(0.00, 100.00);
            string gender;
            try {
                if (genderBox.SelectedItem.ToString() == String.Empty) {
                    MessageBox.Show("Please Select a Valid Option.", "Gender", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gender = "Error";
                    fileError = true;
                }else {
                    gender = genderBox.SelectedItem.ToString();
                }
            } catch {
                MessageBox.Show("Please Select a Valid Option.", "Gender", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                gender = "Error";
                fileError = true;
            }

            JObject profile = new JObject(
                new JProperty("Name", name),
                new JProperty("Gender", gender),
                new JProperty("Place Of Birth", placeOfBirth),
                new JProperty("Age", age),
                new JProperty("Occupation", occupation),
                new JProperty("Race", race),
                new JProperty("Affiliations", affiliations),
                new JProperty("Salary", salary),
                new JProperty("Threat Level", threatLevel.ToString()));

            string profileString = profile.ToString();
            
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
                }catch (Exception ex) {
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
            pictureBox3.Image = ResizeImage(image, 250, 250);
        }
    }
}
