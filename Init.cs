using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable

namespace PrisonerDilemma
{
    internal class Init
    {
        Slider minSlider, maxSlider, shapeSlider;
        Agent[,] agents;
        public bool Enable { get { return _enable; } set { setEnable(value); } }
        public event EventHandler? InitChanged;

        private bool _enable = true;

        public Init(Slider PMinSlider, Slider PMaxSlider, Slider PShapeSlider, Agent[,] PAgents, View PView)
        {
            minSlider = PMinSlider;
            maxSlider = PMaxSlider;
            shapeSlider = PShapeSlider;
            PMinSlider.ValueChanged = onSliderChanged;
            PMaxSlider.ValueChanged = onSliderChanged;  
            PShapeSlider.ValueChanged = onSliderChanged;
            // Don't have the delegates for PermitValueChange for now
            agents = PAgents;
            InitChanged += PView.OnDraw;
        }

        private void onSliderChanged(float PNewValue)
        {
            reinitialise();
        }

        private void setEnable(bool PEnableValue)
        {
            minSlider.Enable = PEnableValue;
            maxSlider.Enable = PEnableValue;
            shapeSlider.Enable = PEnableValue;
        }

        private void reinitialise()
        {
            int nx = agents.GetLength(0);
            int ny = agents.GetLength(1);
            float minP = minSlider.Value;
            float maxP = maxSlider.Value;
            // !!! Don't care about shapeSlider for now
            Random random = new Random();
            for (int x = 0; x < nx; x++)
            {
                for (int y = 0; y < ny; y++)
                {
                    agents[x, y].P = (float)(minP + (maxP - minP) * random.NextDouble());
                }
            }

            InitChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
