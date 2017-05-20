using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Health : IAdjustable
    {
        public CharacterStats CharacterStats { get; set; }

        public int MaxHpBase { get; set; }
        public int MaxHp
        {
            get
            {
                return MaxHpBase + GetConAdjustment();
            }
        }

        public int CurrentHpBase { get; set; }
        public int CurrentHp
        {
            get
            {
                return CurrentHpBase + Adjustment + GetConAdjustment();
            }
        }

        public string DamageReduction { get; set; } = "";

        public string AdjustmentName { get; } = "Wounds/Current HP";

        private int _adjustment;
        public int Adjustment
        {
            get
            {
                return _adjustment;
            }
            set
            {
                _adjustment = Math.Max(Math.Min(value, 0), (MaxHp + CharacterStats.AbilityScores.Scores[AbilityScoreType.Constitution].Score) * -1);
            }
        }

        public Health(CharacterStats characterStats)
        {
            Check.ForNullArgument(characterStats, "characterStats");

            CharacterStats = characterStats;
        }

        private int GetConAdjustment()
        {
            var con = CharacterStats.AbilityScores.Scores[AbilityScoreType.Constitution];

            return CharacterStats.Bio.Level * (con.TempModifier - con.Modifier);
        }

        public void SetMaxHp(string maxHp)
        {
            MaxHpBase = Helpers.Convert.StatToInt(maxHp);
        }

        public void SetCurrentHp(string currentHp)
        {
            CurrentHpBase = Helpers.Convert.StatToInt(currentHp);
        }

        public void SetDamageReduction(string damageReduction)
        {
            DamageReduction = damageReduction;
        }
    }
}
