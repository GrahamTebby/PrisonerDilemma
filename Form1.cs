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
        float[] frameRates = new float[] { 0, 1, 1.5F, 2, 3, 5, 7, 10, 15, 20, 101 };

        public Form1()
        {
            InitializeComponent();
            frameRateTrackBar.Minimum = 0;
            frameRateTrackBar.Maximum = frameRates.Length - 1;
            frameRateTrackBar.TickFrequency = 1;
            frameRateTrackBar.LargeChange = 2;
            frameRateTrackBar.SmallChange = 1;

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
            playRound();
        }

        private void playRound()
        {   // It's a bit minimalist at the moment
            trpsCbox.Checked = !trpsCbox.Checked;
        }

        private void frameRateTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Rates are 0, 1, 1.5, 2, 3, 5, 7, 10, 15, 20 and max
            // This will need to be moved to Control, but this is just a sandpit
            int newPosn = frameRateTrackBar.Value;
            float newFrameRate = frameRates[newPosn];
            frameRateTbox.Text = newFrameRate.ToString();
            // If the frame rate is zero, wait for another update
            if (newFrameRate == 0F) return;

            // If the new frame rate is <100, calculate the frequency
            if (newFrameRate < 100)
            {
                int ms = (int)(1000 / newFrameRate);
                timer1.Stop();
                timer1.Interval = ms;
                timer1.Start();
            }
            else
            {
                // If it's max, assign to another thread
                // Not yet implemented
            }
        }

        private void noiseTrackBar_Scroll(object sender, EventArgs e)
        {
            int newValue = noiseTrackBar.Value;
            noiseTbox.Text = newValue.ToString();
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
