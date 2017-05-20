using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Droid.Backend.Pathfinder.Dice;
using CharacterSheet.Droid.Backend.Pathfinder.Equipment;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Convert = CharacterSheet.Helpers.Convert;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Attack : IAttackRoll
    {
        public CharacterStats CharacterStats { get; protected set; }
        public Weapon Weapon { get; protected set; }

        public string Name { get; protected set; }
        public AttackType AttackType { get; set; }

        public int[] BaseToHit { get; set; }

        public int[] ToHit
        {
            get
            {
                var attackBonuses = CharacterStats.AttackBonuses.Bonuses;

                if (AttackType == AttackType.Ranged)
                {
                    return BaseToHit.Select(x => x + GetTempModifierDifference(AbilityScoreType.Dexterity)  + attackBonuses[AttackBonusType.Ranged].Adjustment).ToArray();
                }
                
                return BaseToHit.Select(x => x + GetTempModifierDifference(AbilityScoreType.Strength) + attackBonuses[AttackBonusType.Melee].Adjustment).ToArray();
            }
        }

        public int NumberOfDice { get; set; }
        public DieType Die { get; set; }
        public int BaseModifier { get; set; }

        public int Modifier
        {
            get
            {
                var standardModifier = BaseModifier + GetTempModifierDifference(AbilityScoreType.Strength);

                if (AttackType == AttackType.Ranged)
                {
                    if (Weapon.Melee != null || Weapon.Ranged.IsSling)
                    {
                        return standardModifier;
                    }

                    return BaseModifier;
                }

                var str = CharacterStats.AbilityScores.Scores[AbilityScoreType.Strength];

                int noStrValue = 0;

                switch (AttackType)
                {
                    case AttackType.Melee1HO:
                    case AttackType.Melee2WOH:
                        noStrValue = BaseModifier - (int)Math.Floor((double)str.TempModifierBase * 0.5);

                        return noStrValue + (int)Math.Floor((double)str.TempModifier * 0.5);
                    case AttackType.Melee2H:
                        if (Weapon.IsLightWeapon)
                        {
                            return standardModifier;
                        }

                        noStrValue = BaseModifier - (int)Math.Floor((double)str.TempModifierBase * 1.5);

                        return noStrValue + (int)Math.Floor((double)str.TempModifier * 1.5);
                    default:
                        return standardModifier;
                };
            }
        }

        public string AdditionalDamageText { get; set; } = "";
        public Critical Critical { get; set; }

        public Advantage Advantage { get; set; } = Advantage.None;
        
        public string DamageText
        {
            get
            {
                return string.Format("{0}{1}{2}", NumberOfDice, Die.ToString().ToLower(), Convert.StatToString(Modifier));
            }
        }

        public Attack(AttackType attackType, string title, string damage, string toHit, CharacterStats characterStats, Weapon weapon)
        {
            Check.ForNullArgument(attackType, nameof(attackType));
            Check.ForNullArgument(title, nameof(title));
            Check.ForNullArgument(damage, nameof(damage));
            Check.ForNullArgument(toHit, nameof(toHit));
            Check.ForNullArgument(characterStats, nameof(characterStats));
            Check.ForNullArgument(weapon, nameof(weapon));

            AttackType = attackType;
            Name = title;
            SetDamage(damage);
            SetToHit(toHit);
            CharacterStats = characterStats;
            Weapon = weapon;
        }

        private int GetTempModifierDifference(AbilityScoreType abilityScoreType)
        {
            var abilityScore = CharacterStats.AbilityScores.Scores[abilityScoreType];

            return abilityScore.TempModifier - abilityScore.TempModifierBase;
        }

        private void SetToHit(string toHit)
        {
            string toHitLower = toHit.ToLower();

            if (toHitLower.Contains("x"))
            {
                string multipliedToHit = "";

                String[] toHitSplit = toHitLower.Split('x');

                int multiplier = int.Parse(toHitSplit[1]);

                for (int i = 1; i <= multiplier; i++)
                {
                    multipliedToHit += toHitSplit[0].Trim();

                    if (i != multiplier)
                    {
                        multipliedToHit += "/";
                    }
                }

                BaseToHit = Helpers.Convert.AttackToIntArray(multipliedToHit);
            }
            else
            {
                BaseToHit = Helpers.Convert.AttackToIntArray(toHit);
            }
        }

        public string GetDamage(int adjustment)
        {
            string damage = String.Format("{0}{1}", NumberOfDice, Die.ToString());

            if (BaseModifier != 0)
            {
                damage = damage + Helpers.Convert.StatToString(BaseModifier + adjustment);
            }

            return damage + AdditionalDamageText;
        }

        private void SetDamage(string damage)
        {
            if (damage.Contains(" "))
            {
                int indexOfSpace = damage.IndexOf(" ");
                AdditionalDamageText = damage.Substring(indexOfSpace);
                damage = damage.Substring(0, indexOfSpace);
            }

            String[] damageStage1 = damage.ToLower().Split('d');

            if (damageStage1.Length > 1)
            {
                NumberOfDice = int.Parse(damageStage1[0]);
                SetDiceAndModifier(damageStage1[1]);
            }
            else
            {
                NumberOfDice = 1;
                SetDiceAndModifier(damageStage1[0]);
            }
        }

        private void SetDiceAndModifier(string diceAndMod)
        {
            Check.ForNullArgument(diceAndMod, "diceAndMod");

            String[] damageStage2;
            bool isNegativeModifier = false;

            if (diceAndMod.Contains("+"))
            {
                damageStage2 = diceAndMod.Split('+');
            }
            else if (diceAndMod.Contains("-"))
            {
                damageStage2 = diceAndMod.Split('-');
                isNegativeModifier = true;
            }
            else
            {
                damageStage2 = new String[] { diceAndMod };
            }

            Die = (DieType) Enum.Parse(typeof(DieType), string.Format("D{0}", damageStage2[0]));

            if (damageStage2.Length > 1)
            {
                int modifier = int.Parse(damageStage2[1]);

                if (isNegativeModifier)
                {
                    modifier *= -1;
                }

                BaseModifier = modifier;
            }
        }
        
        public AttackRoll Roll(bool isFifthEdition)
        {
            return new AttackRoll(BaseToHit, isFifthEdition, NumberOfDice, Die, BaseModifier, Critical, Advantage);
        }
    }
}
