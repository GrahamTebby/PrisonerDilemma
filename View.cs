using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Remoting.Channels;

namespace PrisonerDilemma
{
    internal class View
    {
        // Local references to external objects
        private PictureBox fieldPbox;
        private Agent[,] agents;
        // 
        private Bitmap fieldBmp;
        private Graphics gField;

        public View(PictureBox PFieldPbox, Agent[,] PAgents)
        {
            fieldPbox = PFieldPbox;
            agents = PAgents;
            // Initialise the bitmap and graphics object
            fieldBmp = new Bitmap(fieldPbox.Width, fieldPbox.Height);
            gField = Graphics.FromImage(fieldBmp);
        }

        public void OnDraw(object PSender, EventArgs PE)
        {   // Tell each agent to draw itself
            // This might be called from a background thread
            if (fieldPbox.InvokeRequired)
            {   // Yes it was. Invoke on the HMI thread
                fieldPbox.Invoke((MethodInvoker)delegate { OnDraw(PSender, PE); });
                return;
            }
            int nx = agents.GetLength(0);
            int ny = agents.GetLength(1);
            for (int x = 0; x < nx; x++)
            {
                for (int y = 0; y < ny; y++)
                {
                    agents[x, y].Draw(gField);
                }
            }
            fieldPbox.Image = fieldBmp;
            fieldPbox.Invalidate();
        }
    }
}
