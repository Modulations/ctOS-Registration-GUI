using System.Windows.Forms;

namespace ctOS_Registration {
    public partial class Form5 : Form {
        public Form5() {
            InitializeComponent();
        }
        public void SetBoxes(string[] profile) {
            // 0 = name, 1 = gender, 2 = age, 3 = occupation, 4 = race, 5 = affiliations, 6 = salary, 7 = threat level

            nameBox.AppendText(profile[0]);
            genBox.AppendText(profile[1]);
            ageBox.AppendText(profile[2]);
            occBox.AppendText(profile[3]);
            raceBox.AppendText(profile[4]);
            affBox.AppendText(profile[5]);
            salBox.AppendText(profile[6]);
            threatBox.AppendText(profile[7]);

            ShowDialog();
        }
    }
}
