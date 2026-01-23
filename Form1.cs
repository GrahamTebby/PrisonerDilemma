using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#nullable enable

namespace PrisonerDilemma
{
    public partial class Form1 : Form
    {   // Compile time parameters
        const int agentPx = 50;   // The number of pixels on an agent's side
        readonly float[] frameRates = new float[] { 0, 1, 1.5F, 2, 3, 5, 7, 10, 15, 20, -1 };

        readonly Agent[,] agents;
        readonly View view;
        readonly private Game game;
        readonly Games games;
        readonly Control control;
        readonly Init init;
        Slider minSlider, maxSlider, shapeSlider;   // Sliders for init
        SliderFR frameRateSlider;     // Slider for control
        Slider noiseSlider;

        public Form1()
        {
            InitializeComponent();

            // Calculate the field size & adjust FieldPbox size for an exact fit
            Size fieldSize = calculateFieldSize();
            adjustFieldPboxSize(fieldSize);
            
            agents = initAgentArray(fieldSize); // Cnstruct the agents array
            // Now that we have the agents array, construct the view & control
            view = new View(fieldPbox, agents);
            game = new Game();
            games = new Games(agents, torroidalFieldCBox, game);  // We need this before control so that control can hook up the event
            games.RoundPlayed += view.OnDraw; // Hook up the event to play a round when drawing
            control = new Control(agents, oneRoundBtn, games);
            //control = new Control(agents, oneRoundBtn, frameRateSlider, goCBox, frameRates);

            #region Initialise the init sliders
            SliderConstruction minConstruction = new SliderConstruction
            {
                Name = "Min",
                TrackBar = minTrackBar,
                TextBox = minTbox,
                Label = minLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = 1F,
                NPosns = 101,
                TextFormat = "F2",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            minSlider = new Slider(minConstruction);

            SliderConstruction maxConstruction = new SliderConstruction
            {
                Name = "Max",
                TrackBar = maxTrackBar,
                TextBox = maxTbox,
                Label = maxLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = 1F,
                NPosns = 101,
                TextFormat = "F2",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            maxSlider = new Slider(maxConstruction);

            SliderConstruction shapeConstruction = new SliderConstruction
            {
                Name = "Shape",
                TrackBar = shapeTrackBar,
                TextBox = shapeTbox,
                Label = shapeLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = 1F,
                NPosns = 101,
                TextFormat = "F2",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            shapeSlider = new Slider(shapeConstruction);
            #endregion

            init = new Init(minSlider, maxSlider, shapeSlider, agents, view);

            #region Initialise the control slider (Framerate)
            SliderConstruction frameRateConstruction = new SliderConstruction
            {
                Name = "Frame rate",
                TrackBar = frameRateTrackBar,
                TextBox = frameRateTbox,
                Label = frameRateLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = 1F,
                NPosns = frameRates.Length - 1,
                TextFormat = "F1",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            frameRateSlider = new SliderFR(frameRateConstruction);
            #endregion

            SliderConstruction noiseConstruction = new SliderConstruction
            {
                Name = "Noise",
                TrackBar = noiseTrackBar,
                TextBox = noiseTbox,
                Label = noiseLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = 1F,
                NPosns = 101,
                TextFormat = "F2",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            noiseSlider = new Slider(noiseConstruction);
        }

        private Agent[,] initAgentArray(Size fieldSize)
        {
            Size agent1Size = new Size(agentPx, agentPx);
            // Initialise the agents array
            Agent[,] agents1 = new Agent[fieldSize.Width, fieldSize.Height];
            for (int x = 0; x < fieldSize.Width; x++)
            {
                Point p = new Point(x * agentPx, 0);
                for (int y = 0; y < fieldSize.Height; y++)
                {
                    p.Y = y * agentPx;
                    Rectangle agentRect = new Rectangle(p, agent1Size);
                    agents1[x, y] = new Agent(agentRect);
                }
            }
            return agents1;
        }

        private void frameRateTrackBar_ValueChanged(object PSender, EventArgs PE)
        {
            return; // Disabled for the moment
            // Rates are 0, 1, 1.5, 2, 3, 5, 7, 10, 15, 20 and max
            // This will need to be moved to Control, but this is just a sandpit
            int newPosn = frameRateTrackBar.Value;
            float newFrameRate = frameRates[newPosn];
            frameRateTbox.Text = newFrameRate.ToString();
            timer1.Stop();
            // !!! stopBg();
            // If the frame rate is zero, wait for another update
            if (newFrameRate == 0F)
            {
                return;
            }

            // If the new frame rate is <100, calculate the frequency
            if (newFrameRate < 100)
            {
                int ms = (int)(1000 / newFrameRate);
                timer1.Interval = ms;
                timer1.Start();
            }
            else
            {
                // If it's max, assign to another thread
                // !!! startBg();
            }
        }

        private void noiseTrackBar_Scroll(object PSender, EventArgs PE)
        {
            int newValue = noiseTrackBar.Value;
            noiseTbox.Text = newValue.ToString();
        }

        private void trpsCbox_CheckedChanged(object PSender, EventArgs PE)
        {

        }
        private void timer1Service(object PSender, EventArgs PE)
        {
            // !!!playRoundSync();
        }

        private Size calculateFieldSize()
        {
            int numAgentsX = fieldPbox.Width / agentPx;
            int numAgentsY = fieldPbox.Height / agentPx;
            return new Size(numAgentsX, numAgentsY);
        }

        private void adjustFieldPboxSize(Size PFieldSize)
        {
            fieldPbox.Width = PFieldSize.Width * agentPx;
            fieldPbox.Height = PFieldSize.Height * agentPx;
        }
    }
}
