using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrisonerDilemma
{
    internal class Game
    {
        public void Play(Agent PAgentA, Agent PAgentB)
        {   // Play one agent against another and give them the results
            // !!! To begin, we return no winnings and the oppoent's P
            Result1 resultA = new Result1()
            {
                Winnings = 0,
                P = PAgentB.P
            };
            Result1 resultB = new Result1()
            {
                Winnings = 0,
                P = PAgentA.P
            };
            PAgentA.AddResult(resultA);
            PAgentB.AddResult(resultB);
        }
    }
}