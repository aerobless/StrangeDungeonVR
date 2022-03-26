using System;
using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using static System.Environment;

namespace SixtyMeters.logic.item.edibles
{
    public class HealthPotion : EdibleItem
    {
        // Settings
        public int hpRestoredPercentage;
        public List<string> randomDescription;

        // Internals
        private int _hpRestoredPerUse;

        public override void InitImplementation()
        {
            _hpRestoredPerUse =
                CalculateRestorationPower(GameManager.variabilityManager.player.baseHealth, hpRestoredPercentage);
        }

        public override string GetFullName()
        {
            return "Minor Health Potion";
        }

        public override string GetEmptyName()
        {
            return "<color=grey>[Empty]</color> Minor Health Potion";
        }

        public override string GetDescription()
        {
            return $@"{Helper.GETRandomFromList(randomDescription)}{NewLine}
<color=green>EFFECT: Heals you for {_hpRestoredPerUse} HP</color>{NewLine}
Pull towards your mouth to drink.";
        }

        protected override void ApplyEffectImplementation()
        {
            GameManager.player.Heal(_hpRestoredPerUse);
            GameManager.statisticsManager.potionsUsed++;
        }

        private int CalculateRestorationPower(int baseHealth, int restorationPercentage)
        {
            double exactHp = baseHealth / 100 * restorationPercentage;
            var roundedHp = (int) Math.Floor(exactHp / 10) * 10;
            return roundedHp;
        }
    }
}