using System;
using System.Collections.Generic;
using System.Text;
using CharacterSheet.Helpers;
using CharacterSheet.Droid.Backend.Pathfinder;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class AbilityScore : ICheckRoll, IAdjustable
    {
        public string Name { get; protected set; }
        public string ShortName { get; protected set; }
        public int Score { get; set; }
        public int Modifier { get; set; }

        public int TempScoreBase { get; set; }
        public int TempScore
        {
            get
            {
                return TempScoreBase + Adjustment;
            }
        }

        public int TempModifierBase
        {
            get
            {
                return CalculateModifier(TempScoreBase);
            }
        }

        public int TempModifier
        {
            get
            {
                return CalculateModifier(TempScore);
            }
        }
        
        public string AdjustmentName
        {
            get
            {
                return string.Format("{0} Temp Score", Name);
            }
        }

        public int Adjustment { get; set; }
        public bool IsProficient { get; set; } = false;

        public AbilityScore(string name, string shortName)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(shortName, "shortName");

            Name = name;
            ShortName = shortName;
        }

        private int CalculateModifier(int score)
        {
            return (int)Math.Floor(((double)score - 10d) / 2d);
        }

        public void SetScore(string score)
        {
            Score = Helpers.Convert.StatToInt(score);
        }

        public void SetModifier(string modifier)
        {
            Modifier = Helpers.Convert.StatToInt(modifier);
        }

        public void SetTempScore(string tempScore)
        {
            TempScoreBase = Helpers.Convert.StatToInt(tempScore);
        }

        public CheckResult Roll()
        {
            return new CheckResult(TempModifier);
        }
    }
}
