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
        }

        private void frameRateTrackBar_ValueChanged(float PNewFrameRate)
        {
            if (PNewFrameRate == Form1.MaxFR)
            {   // !!! Background not yet implemented
                return;
            }
            frameTimer1.Stop();
            // !!! stopBg();
            // If the frame rate is zero, wait for another update
            if (PNewFrameRate == 0F)
            {
                return;
            }

            // If the new frame rate is <100, calculate the frequency
            if (PNewFrameRate < 100)
            {
                int ms = (int)(1000 / PNewFrameRate);
                frameTimer1.Interval = ms;
                frameTimer1.Start();
            }
            else
            {
                // If it's max, assign to another thread
                // !!! startBg();
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

#if false
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

        void backgroundWorker1_DoWork(object PSender, DoWorkEventArgs PE)
        {   // Do the background work here
            //BackgroundWorker worker = PSender as BackgroundWorker;

            //while (!worker.CancellationPending)
            {
                //    playRoundAsync();
            }
            //PE.Cancel = true;
        
        }

        private void playRoundAsync()
        {
            playRound(fieldBmp);
            this.Invoke((MethodInvoker)delegate { fieldPbox.Image = fieldBmp; });
        }
#endif
    }
}