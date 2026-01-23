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
        private Button oneRoundBtn;
        private Games games;
        private Slider frameRateSlider;
        private CheckBox goCBox;
        private float[] frameRates;

        public Control(Agent[,] PAgents, Button POneRoundBtn, Games PGames)
        {
            agents = PAgents;
            oneRoundBtn = POneRoundBtn;
            games = PGames;
            oneRoundBtn.Click += games.PlayRound;
            // !!! Need to generate calls to Games and get the slider to display frame rates, not 
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
#endif

        public Control(Agent[,] PAgents, Button POneRoundBtn, SliderFR PFrameRateSlider, CheckBox PGoCBox, float[] PFrameRates)
        {
            agents = PAgents;
            oneRoundBtn = POneRoundBtn;
            frameRateSlider = PFrameRateSlider;
            goCBox = PGoCBox;
            frameRates = PFrameRates;
        }
    }
}