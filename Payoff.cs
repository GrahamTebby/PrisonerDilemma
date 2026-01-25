using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrisonerDilemma
{
    internal class Payoff
    {
        readonly private Slider temptSlider;
        readonly private Slider rewardSlider;
        readonly private Slider punishSlider;
        readonly private Slider suckerSlider;

        public float Tempt { get { return temptSlider.Value; } set { } }
        public float Reward { get { return rewardSlider.Value; } set { } }
        public float Punish { get { return punishSlider.Value; } set { } }
        public float Sucker { get { return suckerSlider.Value; } set { } }

        public Payoff(PayoffConstruction P)
        {
            temptSlider = P.TemptSlider;
            rewardSlider = P.RewardSlider;
            punishSlider = P.PunishSlider;
            suckerSlider = P.SuckerSlider;
        }
    }

    public struct PayoffConstruction
    {
        public Slider TemptSlider;
        public Slider RewardSlider;
        public Slider PunishSlider;
        public Slider SuckerSlider;
    }
}
