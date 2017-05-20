using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Initiative : ICheckRoll
    {
        public CharacterStats CharacterStats { get; set; }

        public int Total
        {
            get
            {
                return Dex + Misc;
            }
        }

        public int Dex
        {
            get
            {
                return CharacterStats.AbilityScores.Scores[AbilityScoreType.Dexterity].TempModifier;
            }
        }

        public int Misc { get; set; }

        public string Name { get; } = "Initiative";

        public Initiative(CharacterStats characterStats)
        {
            Check.ForNullArgument(characterStats, "characterStats");

            CharacterStats = characterStats;
        }

        public void SetMisc(string misc)
        {
            Misc = Helpers.Convert.StatToInt(misc);
        }

        public CheckResult Roll()
        {
            return new CheckResult(Total);
        }
    }
}
