using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PrisonerDilemma
{
    internal class View
    {
        public readonly Size FieldSize;
        public readonly Size Agent1Size;
        private PictureBox fieldPbox;
        private Bitmap fieldBmp;
        private Graphics gField;
        private Agent[,] agents;

        public View(PictureBox PFieldPbox, int PAgentPx, Agent[,] PAgents)
        {
            fieldPbox = PFieldPbox;
            int numAgentsX = fieldPbox.Width / PAgentPx;
            int numAgentsY = fieldPbox.Height / PAgentPx;
            Agent1Size = new Size(PAgentPx, PAgentPx);
            fieldPbox.Width = numAgentsX * PAgentPx;
            fieldPbox.Height = numAgentsY * PAgentPx;
            FieldSize = new Size(numAgentsX, numAgentsY);
            agents = PAgents;

            // Initialise the bitmap and graphics object
            fieldBmp = new Bitmap(fieldPbox.Width, fieldPbox.Height);
            gField = Graphics.FromImage(fieldBmp);
            gField.Clear(Color.LightYellow);

            fieldPbox.Image = fieldBmp;
        }

        public void OnDraw(Agent[,] PAgents)
        {
            for (int x = 0; x < FieldSize.Width; x++)
            {
                for (int y = 0; y < FieldSize.Height; y++)
                {
                    PAgents[x, y].Draw(gField);
                }
            }
            fieldPbox.Image = fieldBmp;
            fieldPbox.Invalidate();
        }
    }
}
