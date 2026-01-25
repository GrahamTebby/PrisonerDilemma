using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrisonerDilemma
{
    internal class Game
    {
        private Payoff payoff;
        Random random = new Random();
        private Result1 resultA, resultB;

        public Game(Payoff PPayoff)
        {
            payoff = PPayoff;
        }

        public void Play(Agent PAgentA, Agent PAgentB)
        {   // Play one agent against another and give them the results
            bool aCooperates = PAgentA.P > (float)random.NextDouble();
            bool bCooperates = PAgentB.P > (float)random.NextDouble();
            float WinningsA = 0;
            float WinningsB = 0;

            resultA.HisP = PAgentB.P;
            resultB.HisP = PAgentA.P;
            if (aCooperates)
            {
                if (bCooperates)
                {   // Both cooperate
                    WinningsA = payoff.Reward;
                    WinningsB = payoff.Reward;
                }
                else
                {   // A cooperates, B defects
                    WinningsA = payoff.Sucker;
                    WinningsB = payoff.Tempt;
                }
            }
            else
            {
                if (bCooperates)
                {   // A defects, B cooperates
                    WinningsA = payoff.Tempt;
                    WinningsB = payoff.Sucker;
                }
                else
                {   // Both defect
                    WinningsA = payoff.Punish;
                    WinningsB = payoff.Punish;
                }
            }
            resultA.MyWinnings = WinningsA;
            resultA.HisWinnings = WinningsB;
            resultB.MyWinnings = WinningsB;
            resultB.HisWinnings = WinningsA;

            PAgentA.AddResult(resultA);
            PAgentB.AddResult(resultB);
        }
    }
}