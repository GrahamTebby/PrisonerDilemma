using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrisonerDilemma
{
    public partial class Form1 : Form
    {
        Bitmap red, blue;

        public Form1()
        {
            InitializeComponent();
            red = new Bitmap(trpsPbox.Width, trpsPbox.Height);
            blue = new Bitmap(trpsPbox.Width, trpsPbox.Height);
            Graphics gRed = Graphics.FromImage(red);
            gRed.Clear(Color.Red);
            Graphics gBlue = Graphics.FromImage(blue);
            gBlue.Clear(Color.Blue);
            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += timer1Service;
            timer1.Start();
        }

        private void timer1Service(object sender, EventArgs e)
        {
            trpsCbox.Checked = !trpsCbox.Checked;
        }

        private void trpsCbox_CheckedChanged(object sender, EventArgs e)
        {
            if (trpsCbox.Checked)
            {
                trpsPbox.Image = red;
            }
            else
            {
                trpsPbox.Image = blue;
            }
        }

    }
}
