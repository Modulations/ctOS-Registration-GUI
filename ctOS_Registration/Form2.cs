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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            var timer = new Timer();
            //change the background image every second  
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            //add image in list from resource file.  
            List<Bitmap> lisimage = new List<Bitmap>();
            lisimage.Add(Properties.Resources.ctOS_Background);
            lisimage.Add(Properties.Resources.ctOS_Background);
            var indexbackimage = DateTime.Now.Second % lisimage.Count;
            this.BackgroundImage = lisimage[indexbackimage];
        }
    }
}
