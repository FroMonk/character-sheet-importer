using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class SavingThrow : ICheckRoll, IAdjustable
    {
        public CharacterStats CharacterStats { get; set; }

        public string Name { get; set; }
        public string StatName { get; set; }

        public int Total
        {
            get
            {
                return Base + Ability + Magic + Epic + Misc + Adjustment;
            }
        }

        private int _base;
        public int Base
        {
            get
            {
                return IsProficient ? CharacterStats.Proficiency : _base;
            }

            set
            {
                _base = value;
            }
        }

        public int Ability
        {
            get
            {
                return CharacterStats.AbilityScores.Scores[_abilityBasedOn].TempModifier;
            }
        }

        public int Magic { get; set; }
        public int Epic { get; set; }
        public int Misc { get; set; }

        public string AdjustmentName
        {
            get
            {
                return CharacterStats.GameSystem == GameSystem.D20FifthEdition ? string.Format("{0} Saving Throw Temp", Name) : string.Format("{0} Temp", Name);
            }
        }

        public int Adjustment { get; set; }
        private AbilityScoreType _abilityBasedOn;

        public bool IsProficient { get; set; }

        public SavingThrow(string name, string statName, CharacterStats characterStats, AbilityScoreType abilityBasedOn)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(statName, "statName");
            Check.ForNullArgument(characterStats, "characterStats");
            Check.ForNullArgument(abilityBasedOn, "abilityBasedOn");

            Name = name;
            StatName = statName;
            CharacterStats = characterStats;
            _abilityBasedOn = abilityBasedOn;
        }

        public void SetBase(string baseValue)
        {
            Base = Helpers.Convert.StatToInt(baseValue);
        }

        public void SetMagic(string magic)
        {
            Magic = Helpers.Convert.StatToInt(magic);
        }

        public void SetEpic(string epic)
        {
            Epic = Helpers.Convert.StatToInt(epic);
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
