using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ctOS_Registration
{
    public partial class ctOS_Welcome : Form
    {
        public ctOS_Welcome()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("User error detected. Continuing survey.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            MessageBox.Show("Thank you for cooperating, citizen.", "Thank you.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void adminPanelButton_Click(object sender, EventArgs e) {
            adminLoginPanel login = new adminLoginPanel();
            Hide();
            login.ShowDialog();
            Close();
        }
    }
}
