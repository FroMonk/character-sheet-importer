using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Convert = CharacterSheet.Helpers.Convert;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.Droid.Backend.Pathfinder.Dice
{
    public class AttackRoll
    {
        public Dictionary<int, string> ToHitMap { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> DiscardedToHitMap { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> CritConfirmMap { get; } = new Dictionary<int, string>();
        public Dictionary<int, string> DamageMap { get; } = new Dictionary<int, string>();
        public Dictionary<int, List<string>> CritDamageMap { get; } = new Dictionary<int, List<string>>();
        public Dictionary<int, AttackRollOutcome> OutcomeMap { get; } = new Dictionary<int, AttackRollOutcome>();

        private Random _random = new Random();

        private DateTime _rolledOn;
        public string RolledAt
        {
            get
            {
                return string.Format("{0:00}:{1:00}:{2:00}", _rolledOn.Hour, _rolledOn.Minute, _rolledOn.Second);
            }
        }

        public AttackRoll(int[] toHits, bool isFifthEdition, int? numberOfDamageDieRolls = null, DieType? damageDieType = null, int? damageDieModifier = null, Critical crit = null, Advantage advantage = Advantage.None)
        {
            var hasDamage = numberOfDamageDieRolls.HasValue;

            int i = 1;

            foreach (var toHit in toHits)
            {
                int rolled;

                if (advantage == Advantage.None)
                {
                    ToHitMap.Add(i, RollToHit(toHit, out rolled));
                    DiscardedToHitMap.Add(i, null);
                }
                else
                {
                    int roll1;
                    int roll2;

                    var roll1Text = RollToHit(toHit, out roll1);
                    var roll2Text = RollToHit(toHit, out roll2);

                    if (advantage == Advantage.Advantage)
                    {
                        if (roll1 >= roll2)
                        {
                            rolled = roll1;
                            ToHitMap.Add(i, roll1Text);
                            DiscardedToHitMap.Add(i, roll2Text);
                        }
                        else
                        {
                            rolled = roll2;
                            ToHitMap.Add(i, roll2Text);
                            DiscardedToHitMap.Add(i, roll1Text);
                        }
                    }
                    else
                    {
                        if (roll1 >= roll2)
                        {
                            rolled = roll2;
                            ToHitMap.Add(i, roll2Text);
                            DiscardedToHitMap.Add(i, roll1Text);
                        }
                        else
                        {
                            rolled = roll1;
                            ToHitMap.Add(i, roll1Text);
                            DiscardedToHitMap.Add(i, roll2Text);
                        }
                    }
                }

                AttackRollOutcome outcome;

                switch (rolled)
                {
                    case 1:
                        outcome = AttackRollOutcome.Fumble;
                        break;
                    case 20:
                        outcome = AttackRollOutcome.Natural20;
                        break;
                    default:
                        if (crit != null && crit.IsInRange(rolled))
                        {
                            outcome = AttackRollOutcome.Crit;
                        }
                        else
                        {
                            outcome = AttackRollOutcome.Standard;
                        }
                        break;
                }

                OutcomeMap.Add(i, outcome);

                string damage = null;

                if (hasDamage)
                {
                    damage = RollDamage(numberOfDamageDieRolls.Value, damageDieType.Value, damageDieModifier.Value);
                }

                DamageMap.Add(i, damage);

                string critConfirm = null;
                List<string> critDamage = null;

                if (outcome == AttackRollOutcome.Crit || outcome == AttackRollOutcome.Natural20)
                {
                    if (isFifthEdition && hasDamage)
                    {
                        critDamage = new List<string>
                        {
                            RollDamage(numberOfDamageDieRolls.Value * crit.Multiplier, damageDieType.Value, damageDieModifier.Value)
                        };
                    }
                    else
                    { 
                        critConfirm = RollToHit(toHit, out rolled);

                        if (hasDamage)
                        {
                            critDamage = new List<string>();

                            int j = 0;

                            while (j < crit.Multiplier - 1)
                            {
                                critDamage.Add(RollDamage(numberOfDamageDieRolls.Value, damageDieType.Value, damageDieModifier.Value));
                                j++;
                            }
                        }
                    }
                }

                if (!isFifthEdition)
                {
                    CritConfirmMap.Add(i, critConfirm);
                }

                CritDamageMap.Add(i, critDamage);

                i++;
            }

            _rolledOn = DateTime.Now;
        }

        private string RollToHit(int toHit, out int rolled)
        {
            rolled = RollDie(DieType.D20);

            return FormatRollOutput(rolled, rolled, toHit);
        }

        private int RollDie(DieType die)
        {
            return _random.Next((int)die) + 1;
        }

        private string RollDamage(int numberOfRolls, DieType die, int modifier)
        {
            int total = 0;
            List<int> rolled = new List<int>();

            var rollOutput = string.Empty;

            for (int i = 0; i < numberOfRolls; i++)
            {
                rolled.Add(RollDie(die));
            }

            for (int i = 0; i < numberOfRolls; i++)
            {
                total += rolled[i];
                rollOutput += rolled[i];

                if (i != numberOfRolls - 1)
                {
                    rollOutput += ",";
                }
            }

            return FormatRollOutput(total, rollOutput, modifier);
        }

        private string FormatRollOutput(int rollTotal, object rolls, int modifier)
        {
            return string.Format("{0}\t({1}){2}", rollTotal + modifier, rolls, Convert.StatToString(modifier));
        }
    }
}