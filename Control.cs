using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrisonerDilemma
{
    internal class Control
    {
        readonly Agent[,] agents;       // The array of agents
        //!!!ControlEventHandler controlEventHandler;
        private SliderFR frameRateSlider;
        private CheckBox goCBox;
        private Button oneRoundBtn;
        private float[] frameRates;
        private Games games;
        private Timer frameTimer1;
        private BackgroundWorker backgroundWorker1;

        public Control(Agent[,] PAgents, SliderFR PFrameRateSlider, CheckBox PGoCBox, Button POneRoundBtn, float[] PFrameRatesGames, Games PGames)
        {
            agents = PAgents;
            frameRateSlider = PFrameRateSlider;
            goCBox = PGoCBox;
            oneRoundBtn = POneRoundBtn;
            frameRates = PFrameRatesGames;
            games = PGames;
            oneRoundBtn.Click += games.PlayRound;
            PFrameRateSlider.ValueChanged += frameRateTrackBar_ValueChanged;
            frameTimer1 = new Timer();
            frameTimer1.Tick += frameTimer1_Tick;
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void frameRateTrackBar_ValueChanged(float PNewFrameRate)
        {
            frameTimer1.Stop();
            stopBg();

            // If the frame rate is zero, wait for another update
            if (PNewFrameRate == 0F)
            {
                return;
            }

            // If the new frame rate is <100, calculate the frequency
            if (PNewFrameRate == Form1.MaxFR)
            {   
                startBg();
            }
            else
            {
                int ms = (int)(1000 / PNewFrameRate);
                frameTimer1.Interval = ms;
                frameTimer1.Start();
            }
        }

        private void frameTimer1_Tick(object sender, EventArgs e)
        {
            games.PlayRound(this, EventArgs.Empty);
            if (!goCBox.Checked)
            {
                // !!!frameTimer1.Stop();
            }
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
                games.PlayRound(this, EventArgs.Empty);
            }
            PE.Cancel = true;        
        }
    }
}