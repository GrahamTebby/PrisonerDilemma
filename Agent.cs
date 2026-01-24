using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PrisonerDilemma
{

    internal class Agent
    {
        public float P;                 // Probability of cooperating: 0 to 1
        private Rectangle rectangle;    // Where it can draw itself
        // A list of the results of this round, cleared with InitRound, incrementsed with AddResult, used in FinaliseRound
        private readonly List <Result1> results = new List<Result1>();
        private static readonly Brush[] brushes = new Brush[256]; // The colours for drawing based on P

        static Agent()
        {   // Initialise the brushes array
            // Cannot have a public modifier.
            // Is executed before any instance is created.
            for (int rg = 0; rg < 256; rg++)
            {
                brushes[rg] = new SolidBrush(Color.FromArgb(rg, rg, 255));
            }
        }

        public Agent(Rectangle PRect)
        {
            P = 0.5F;
            rectangle = PRect;
        }

        public void InitRound()
        {   // Called at the start of each round to clear previous results
            results.Clear();
        }

        public void AddResult(Result1 PResult1)
        {   // Called after each match to add the result
            results.Add(PResult1);
        }

        public void FinaliseRound()
        {   // !!! For now, average P out
            P = results.Sum(PR => PR.P) / results.Count;
        }

        public void Draw(Graphics PG)
        {   // Draw the agent as a rectangle with color based on P between blue (defect) and white (cooperate)
            int rg = (int)(P * 256);
            if (rg > 255) rg = 255;
            PG.FillRectangle(brushes[rg], rectangle);
        }
    }
}
