using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrisonerDilemma
{

    internal class Agent
    {
        public float P;
        private readonly List <Result1> results = new List<Result1>();
        private Rectangle rectangle;

        public Agent(Rectangle PRect)
        {
            P = 0.5F; // Probability of cooperating: 0 to 1
            rectangle = PRect;
        }

        public void InitRound()
        {
            results.Clear();
        }

        public void AddResult(Result1 PResult1)
        {
            results.Add(PResult1);
        }

        public void FinaliseRound()
        {   // !!! No change in P for now
#if false
            // Adjust P based on results
            float avgWinnings = results.Average(r => r.Winnings);
            if (avgWinnings < 3) // Arbitrary threshold for adjustment
            {
                P = Math.Max(0, P - 1); // Decrease probability of cooperating
            }
            else
            {
                P = Math.Min(100, P + 1); // Increase probability of cooperating
            }
#endif
        }

        public void Draw(Graphics PG)
        {
            // Draw the agent as a rectangle with color based on P between black and yellow
            int rg = (int)(P * 255);
            using (Brush brush = new SolidBrush(Color.FromArgb(rg, rg, 255)))
            {
                PG.FillRectangle(brush, rectangle);
            }
        }
    }
}
