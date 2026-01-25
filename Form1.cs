using System;
using System.Drawing;
using System.Windows.Forms;
#nullable enable

namespace PrisonerDilemma
{
    public partial class Form1 : Form
    {   // Compile time parameters
        const int agentPx = 10;   // The number of pixels on an agent's side

        // Fields for initialisation
        readonly Slider minSlider, maxSlider, shapeSlider;   // Sliders for init
        readonly Init init;

        // Fields for payoff
        readonly Payoff payoff;
        readonly Slider temptSlider, rewardSlider, punishSlider, suckerSlider;

        // Fields for game play
        readonly Games games;
        readonly private Game game;
        readonly Slider noiseSlider;

        // Fields for control
        readonly Control control;
        readonly SliderFR frameRateSlider;                   // Slider for control
        public const int MaxFR = 1000; // Special value to indicate maximum frame rate (which involes background processing)
        readonly float[] frameRates = new float[] { 0, 1, 1.5F, 2, 3, 5, 10, 20, MaxFR };   // Possible frame rates
        
        readonly Agent[,] agents;
        readonly View view;
         
        public Form1()
        {
            InitializeComponent();

            // Calculate the field size & adjust FieldPbox size for an exact fit
            Size fieldSize = calculateFieldSize();
            adjustFieldPboxSize(fieldSize);
            
            agents = initAgentArray(fieldSize); // Cnstruct the agents array, needed before view and control
            view = new View(fieldPbox, agents, fCountTbox);

            // Initialisation of Payoff
            SliderConstruction temptConstruction = new SliderConstruction
            {
                Name = "Temptation",
                TrackBar = temptTrackBar,
                TextBox = temptTbox,
                Label = temptLabel,
                MinValue = 0F,
                InitialValue = 7F,
                MaxValue = 10F,
                NPosns = 101,
                TextFormat = "F1",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            temptSlider = new Slider(temptConstruction);

            SliderConstruction rewardConstruction = new SliderConstruction
            {
                Name = "Reward",
                TrackBar = rewardTrackBar,
                TextBox = rewardTbox,
                Label = rewardLabel,
                MinValue = 0F,
                InitialValue = 5F,
                MaxValue = 10F,
                NPosns = 101,
                TextFormat = "F1",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            rewardSlider = new Slider(rewardConstruction);

            SliderConstruction punishConstruction = new SliderConstruction
            {
                Name = "Punishment",
                TrackBar = punishTrackBar,
                TextBox = punishTbox,
                Label = punishLabel,
                MinValue = 0F,
                InitialValue = 3F,
                MaxValue = 10F,
                NPosns = 101,
                TextFormat = "F1",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            punishSlider = new Slider(punishConstruction);

            SliderConstruction suckerConstruction = new SliderConstruction
            {
                Name = "Sucker",
                TrackBar = suckerTrackBar,
                TextBox = suckerTbox,
                Label = suckerLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = 10F,
                NPosns = 101,
                TextFormat = "F1",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            suckerSlider = new Slider(suckerConstruction);

            PayoffConstruction payoffConstruction = new PayoffConstruction
            {
                TemptSlider = temptSlider,
                RewardSlider = rewardSlider,
                PunishSlider = punishSlider,
                SuckerSlider = suckerSlider
            };
            payoff = new Payoff(payoffConstruction);

            game = new Game(payoff);
            games = new Games(agents, torroidalFieldCBox, game);  // Control needs to hook games.PlayRound to button event
            games.RoundPlayed += view.OnDraw; // Hook up the event to play a round when drawing

            SliderConstruction frameRateConstruction = new SliderConstruction
            {
                Name = "Frame rate",
                TrackBar = frameRateTrackBar,
                TextBox = frameRateTbox,
                Label = frameRateLabel,
                MinValue = 0F,
                InitialValue = 0F,
                MaxValue = frameRates.Length - 1,
                NPosns = frameRates.Length,
                TextFormat = "",
                InitLabelText = null,
                PermitValueChange = null,
                ValueChanged = null
            };
            frameRateSlider = new SliderFR(frameRateConstruction, frameRates);

            control = new Control(frameRateSlider, goCBox, oneRoundBtn, games);
            control.Play1Round += games.PlayRound; // Control needs to hook games.PlayRound to button event

            // Initialisation of Init
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
            init = new Init(minSlider, maxSlider, shapeSlider, agents, view);

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

        private Agent[,] initAgentArray(Size PFieldSize)
        {
            Size agent1Size = new Size(agentPx, agentPx);
            // Initialise the agents array
            Agent[,] agents1 = new Agent[PFieldSize.Width, PFieldSize.Height];
            for (int x = 0; x < PFieldSize.Width; x++)
            {
                Point p = new Point(x * agentPx, 0);
                for (int y = 0; y < PFieldSize.Height; y++)
                {
                    p.Y = y * agentPx;
                    Rectangle agentRect = new Rectangle(p, agent1Size);
                    agents1[x, y] = new Agent(agentRect);
                }
            }
            return agents1;
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
        {   // Calculates the field size (the number of agents in each direction)
            //  based on the size of fieldPbox and agentPx
            int numAgentsX = fieldPbox.Width / agentPx;
            int numAgentsY = fieldPbox.Height / agentPx;
            return new Size(numAgentsX, numAgentsY);
        }

        private void adjustFieldPboxSize(Size PFieldSize)
        {   // Adjusts the size of fieldPbox to fit an exact number of agents
            fieldPbox.Width = PFieldSize.Width * agentPx;
            fieldPbox.Height = PFieldSize.Height * agentPx;
        }
    }
}
