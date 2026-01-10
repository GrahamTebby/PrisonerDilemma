using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Just to see if I can push

namespace PrisonerDilemma
{
    public partial class Form1 : Form
    {
        Bitmap red, blue, fieldBmp;
        float[] frameRates = new float[] { 0, 1, 1.5F, 2, 3, 5, 7, 10, 15, 20, 101 };
        Graphics g;
        int x = 0, y = 0;

        public Form1()
        {
            InitializeComponent();

            // Initialise the frame rate tracker
            frameRateTrackBar.Minimum = 0;
            frameRateTrackBar.Maximum = frameRates.Length - 1;
            frameRateTrackBar.TickFrequency = 1;
            frameRateTrackBar.LargeChange = 2;
            frameRateTrackBar.SmallChange = 1;

            // Initialise the bitmaps for the trps picture box
            fieldBmp = new Bitmap(fieldPbox.Width, fieldPbox.Height);
            fieldPbox.Image = fieldBmp;
            g = Graphics.FromImage(fieldBmp);
            g.Clear(Color.LightBlue);

            red = new Bitmap(trpsPbox.Width, trpsPbox.Height);
            Graphics gRed = Graphics.FromImage(red);
            gRed.Clear(Color.Red);
            blue = new Bitmap(trpsPbox.Width, trpsPbox.Height);
            Graphics gBlue = Graphics.FromImage(blue);
            gBlue.Clear(Color.Blue);
            
            // Based on https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.backgroundworker?view=net-10.0
            backgroundWorker1.WorkerReportsProgress = false;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void startBg()
        {   // Start the asynchronous operation
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void stopBg()
        {   // Stop the asynchronous operation
            if (backgroundWorker1.WorkerSupportsCancellation)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {   // Do the background work here
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending)
            {
                playRoundAsync();
            }
            e.Cancel = true;
        }

         private void playRoundSync()
        {
            playRound(fieldBmp);
            fieldPbox.Image = fieldBmp;
        }

        private void playRoundAsync()
        {

            playRound(fieldBmp);
            this.Invoke((MethodInvoker)delegate { fieldPbox.Image = fieldBmp; });
        }

        private void playRound(Bitmap PBitmap)
        {   // It's a bit minimalist at the moment
            PBitmap.SetPixel(x, y, Color.Black);
            if (++x >= PBitmap.Width)
            {
                x = 0;
                y+=2;
            }
        }

        private void frameRateTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Rates are 0, 1, 1.5, 2, 3, 5, 7, 10, 15, 20 and max
            // This will need to be moved to Control, but this is just a sandpit
            int newPosn = frameRateTrackBar.Value;
            float newFrameRate = frameRates[newPosn];
            frameRateTbox.Text = newFrameRate.ToString();
            // If the frame rate is zero, wait for another update
            stopBg();
            if (newFrameRate == 0F)
            {
                timer1.Stop();
                return;
            }

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
                startBg();
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
        private void timer1Service(object sender, EventArgs e)
        {
            playRoundSync();
        }


    }
}
