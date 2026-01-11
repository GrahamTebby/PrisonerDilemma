using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrisonerDilemma
{
    internal class Control
    {
        readonly int fieldPx = 50;      // The size of the agent in pixels
        readonly PictureBox fieldPbox;  // A copy of the PictureBox from the form
        readonly Agent[,] agents;       // The array of agents
        readonly Graphics g;            // Graphics object for drawing

        public Control(PictureBox PFieldPbox)
        {
            fieldPbox = PFieldPbox;
            Random random = new Random();

            // Determine number of agents in each dimension
            int numAgentsX = fieldPbox.Width / fieldPx;
            int numAgentsY = fieldPbox.Height / fieldPx;
            fieldPbox.Width = numAgentsX * fieldPx;
            fieldPbox.Height = numAgentsY * fieldPx;

            // Initialise the bitmap and graphics object
            Bitmap bmp = new Bitmap(fieldPbox.Width, fieldPbox.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.LightYellow);


            // Initialise the agents array
            agents = new Agent[numAgentsX, numAgentsY];
            for (int x = 0; x < numAgentsX; x++)
            {
                for (int y = 0; y < numAgentsY; y++)
                {
                    Rectangle agentRect = new Rectangle(x * fieldPx, y * fieldPx, fieldPx, fieldPx);
                    agents[x, y] = new Agent(agentRect, random);
                }
            }

            // ** Temporary code to test agent drawing **
            for (int x = 0; x < numAgentsX; x++)
            {
                for (int y = 0; y < numAgentsY; y++)
                {
                    agents[x, y].Draw(g);
                }
            }
            fieldPbox.Image = bmp;
            fieldPbox.Invalidate();
        }
    }
}
