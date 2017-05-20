using System;
using System.Collections.Generic;
using System.Text;
using CharacterSheet.Helpers;
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Droid.Backend.Pathfinder.Dice;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class AttackBonus : IAttackRoll, IAdjustable
    {
        public CharacterStats CharacterStats { get; set; }

        public string Name { get; set; }
        public string Subtitle { get; set; } = "ATTACK BONUS";
        public AttackBonusType AttackBonusType { get; set; }
        
        public int[] ToHit
        {
            get
            {
                var adjustedTotal = new int[Base.Length];
                var adjustment = Stat + Size + Epic + Misc + Adjustment;
                for (int i = 0; i < Base.Length; i++)
                {
                    adjustedTotal[i] = Base[i] + adjustment;
                }

                return adjustedTotal;
            }
        }
        
        public int[] _base { get; set; } = new int[0];
        public int[] Base
        {
            get
            {
                return AttackBonusType == AttackBonusType.Bab ? _base : CharacterStats.AttackBonuses.Bonuses[AttackBonusType.Bab].ToHit;
            }
        }

        public int Stat
        {
            get
            {
                switch (AttackBonusType)
                {
                    case AttackBonusType.Melee:
                        return CharacterStats.AbilityScores.Scores[AbilityScoreType.Strength].TempModifier;
                    case AttackBonusType.Ranged:
                        return CharacterStats.AbilityScores.Scores[AbilityScoreType.Dexterity].TempModifier;
                    case AttackBonusType.Cmb:
                        return (int)CharacterStats.Bio.Size <= ((int) Pathfinder.Size.Tiny) * -1 ? CharacterStats.AbilityScores.Scores[AbilityScoreType.Dexterity].TempModifier
                                                                                : CharacterStats.AbilityScores.Scores[AbilityScoreType.Strength].TempModifier;
                    default:
                        return 0;
                }                
            }
        }
        
        public int Size
        {
            get
            {
                return (int)CharacterStats.Bio.Size;
            }
        }

        public int Epic { get; set; }
        public int Misc { get; set; }

        public string AdjustmentName
        {
            get
            {
                return string.Format("{0} Temp", Name);
            }
        }

        public int Adjustment { get; set; }

        public AttackBonus(string name, AttackBonusType attackBonusType, CharacterStats characterStats)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(attackBonusType, "attackBonusType");
            Check.ForNullArgument(characterStats, "characterStats");

            Name = name;
            AttackBonusType = attackBonusType;
            CharacterStats = characterStats;
        }

        public void SetBase(string baseValue)
        {
            _base = Helpers.Convert.AttackToIntArray(baseValue);
        }

        public void SetEpic(string epic)
        {
            Epic = Helpers.Convert.StatToInt(epic);
        }

        public void SetMisc(string misc)
        {
            Misc = Helpers.Convert.StatToInt(misc);
        }

        public AttackRoll Roll(bool isFifthEdition)
        {
            return new AttackRoll(ToHit, isFifthEdition: isFifthEdition);
        }
    }
}
