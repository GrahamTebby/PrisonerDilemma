using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrisonerDilemma
{
    internal class Payoff
    {
        public float Tempt = 7;      // Temptation to defect
        public float Reward = 5;     // Reward for mutual cooperation
        public float Punish = 1;     // Punishment for mutual defection
        public float Sucker = 0;     // Sucker's payoff
    }
}
