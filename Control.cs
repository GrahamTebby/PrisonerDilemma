using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#nullable enable

namespace PrisonerDilemma
{
    internal class Control
    {
        // HMI elements serviced here
        readonly private SliderFR frameRateSlider;  // Frame rate
        readonly private CheckBox goCBox;           // Go continuously checkbox
        readonly private Button oneRoundBtn;        // Run a single round
        // Other objects
        readonly private Timer frameTimer;
        readonly private BackgroundWorker backgroundWorker1;
        private float currentFrameRate;
        public event EventHandler? Play1Round;      // Event to play one round

        public Control(SliderFR PFrameRateSlider, CheckBox PGoCBox, Button POneRoundBtn, Games PGames)
        {
            frameRateSlider = PFrameRateSlider;
            goCBox = PGoCBox;
            oneRoundBtn = POneRoundBtn;
            PFrameRateSlider.ValueChanged += frameRateTrackBar_ValueChanged;
            frameTimer = new Timer();
            frameTimer.Tick += frameTimer1_Tick;
            goCBox.CheckedChanged += goCboxChanged;
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.WorkerSupportsCancellation = true;
            currentFrameRate = 0F;
            // We want to be able to disable one round button when running continuously
            // We prefer to keep all control logic in this class, therefore need to hook up the event here
            // This means that we need to have access to PGames here
            oneRoundBtn.Click += PGames.PlayRound;
        }

        private void frameRateTrackBar_ValueChanged(float PNewFrameRate)
        {
            currentFrameRate = PNewFrameRate;
            hmiChanged();
        }

        private void goCboxChanged(object sender, EventArgs e)
        {
            hmiChanged();
        }

        private void frameTimer1_Tick(object PSender, EventArgs PE)
        {
            Play1Round?.Invoke(this, EventArgs.Empty);
        }

        private void hmiChanged()
        {   // Stop everything first
            frameTimer.Stop();
            stopBg();

            // If the frame rate is zero or the go checkbox is not checked, wait for another update
            if (currentFrameRate == 0F || !goCBox.Checked)
            {
                return;
            }

            if (currentFrameRate == Form1.MaxFR)
            {   // Maximum frame rate, use background worker
                startBg();
            }
            else
            {   // If the new frame rate is not Form1.MaxFR, calculate the frequency
                int ms = (int)(1000 / currentFrameRate);
                frameTimer.Interval = ms;
                frameTimer.Start();
            }
        }

        private void startBg()
        {   // Start the asynchronous operation
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                oneRoundBtn.Enabled = false;
            }
        }

        private void stopBg()
        {   // Stop the asynchronous operation
            if (backgroundWorker1.WorkerSupportsCancellation)
            {
                backgroundWorker1.CancelAsync();
                while (backgroundWorker1.IsBusy)
                {
                    Application.DoEvents();   // Keep the HMI responsive
                }
                oneRoundBtn.Enabled = true;
                // !!! This returns immediately
                // Need to service
                // void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
                //{
                //    if (e.Cancelled)
                //    {
                //        // Now it's truly cancelled
                //    }
                //}
            }
        }

        void backgroundWorker1_DoWork(object PSender, DoWorkEventArgs PE)
        {   // Do the background work here            
            while (!backgroundWorker1.CancellationPending)
            {
                Play1Round?.Invoke(this, EventArgs.Empty);   // Invoke the event to play one round
            }
            PE.Cancel = true;        
        }
    }
}