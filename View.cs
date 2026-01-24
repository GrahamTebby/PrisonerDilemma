using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
//using System.Runtime.Remoting.Channels;

namespace PrisonerDilemma
{
    internal class View
    {
        // Local references to external objects
        readonly private PictureBox fieldPbox;
        readonly private Agent[,] agents;
        readonly private Bitmap fieldBmp;
        readonly private Graphics gField;

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
            // Multiple events subscribe to this, possibly from a background thread
            if (fieldPbox.InvokeRequired)   // Is it from the HMI thread?
            {   // No it was not; invoke on the HMI thread (which re-enters)
                fieldPbox.Invoke((MethodInvoker)delegate { OnDraw(PSender, PE); });
                return;
            }
            // From here we are on the HMI thread
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