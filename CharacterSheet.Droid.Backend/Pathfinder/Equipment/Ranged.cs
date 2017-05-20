using CharacterSheet.Droid.Backend.Pathfinder.Equipment;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using CharacterSheet.Droid.Backend.Pathfinder;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Ranged
    {
        private const string SLING = "sling";

        public List<RangedAttack> RangedAttacks { get; set; } = new List<RangedAttack>();
        public List<Ammunition> AmmunitionList { get; set; } = new List<Ammunition>();
        public bool HasPointBlankShot { get; set; }
        public bool HasFarShot { get; set; }
        public int RangeIncrementValue { get; set; }
        public string BaseRangedAttack { get; set; }
        public bool IsSling { get; set; }
        
        /// <summary>
        /// Constructor only used by PCGen parser
        /// </summary>
        public Ranged()
        {

        }

        /// <summary>
        /// Constructor only used by Hero Lab parser
        /// </summary>
        public Ranged(bool hasPointBlankShot, bool hasFarShot, string rangeIncrementValue, string baseRangedAttack, string damage, CharacterStats characterStats, Weapon weapon)
        {
            Check.ForNullArgument(rangeIncrementValue, nameof(rangeIncrementValue));
            Check.ForNullArgument(baseRangedAttack, nameof(baseRangedAttack));
            Check.ForNullArgument(damage, nameof(damage));
            Check.ForNullArgument(weapon, nameof(weapon));

            BaseRangedAttack = baseRangedAttack;

            rangeIncrementValue = rangeIncrementValue.Replace("'", "");
            RangeIncrementValue = Helpers.Convert.StatToInt(rangeIncrementValue);
            HasPointBlankShot = hasPointBlankShot;
            HasFarShot = hasFarShot;

            SetupAmmunition(weapon);

            for (int i = 1; i <= 5; i++)
            {
                var distance = GetRangeValue(i);

                var attack = new RangedAttack(string.Format("{0} | {1}", weapon.Name, distance), GetRangeValue(i), damage, AdjustToHit(i), characterStats, weapon);

                RangedAttacks.Add(attack);
            }

            IsSling = weapon.Name.ToLower().Contains(SLING);

        }

        /// <summary>
        /// Constructor only used by Hero Lab Fifth Edition parser
        /// </summary>
        public Ranged(string rangesText, string baseRangedAttack, string damage, CharacterStats characterStats, Weapon weapon)
        {
            Check.ForNullArgument(rangesText, nameof(rangesText));
            Check.ForNullArgument(baseRangedAttack, nameof(baseRangedAttack));
            Check.ForNullArgument(damage, nameof(damage));
            Check.ForNullArgument(weapon, nameof(weapon));

            BaseRangedAttack = baseRangedAttack;
            
            var ranges = rangesText.Split('/').ToList();

            SetupAmmunition(weapon);

            ranges.ForEach(x => RangedAttacks.Add(new RangedAttack(string.Format("{0} | {1}", weapon.Name, x), x, damage, BaseRangedAttack, characterStats, weapon)));

            if (RangedAttacks.Count() > 1)
            {
                RangedAttacks.LastOrDefault().Advantage = Advantage.Disadvantage;
            }
            
            IsSling = weapon.Name.ToLower().Contains(SLING);
        }

        private void SetupAmmunition(Weapon weapon)
        {
            var ammoSegmentRegex = new Regex(@"(?<ammoSegment> \([0-9]+ @ .+\))");
            var ammoSegmentMatch = ammoSegmentRegex.Match(weapon.Name);

            if (ammoSegmentMatch.Success)
            {
                var ammoMatch = Regex.Match(ammoSegmentMatch.Groups["ammoSegment"].Value, @"^.*\((?<ammoCount>[0-9]+) @ .*\).*$");

                if (ammoMatch.Success)
                {
                    int ammoCount = 0;

                    if (int.TryParse(ammoMatch.Groups["ammoCount"].Value, out ammoCount))
                    {
                        AmmunitionList.Add(new Ammunition("Unknown Ammunition")
                        {
                            Remaining = ammoCount
                        });
                    }
                }

                weapon.Name = ammoSegmentRegex.Replace(weapon.Name, string.Empty);
            }
        }

        private string GetRangeValue(int multiplier)
        {
            int range;

            if (HasPointBlankShot)
            {
                if (multiplier == 1)
                {
                    range = 30;
                }
                else
                {
                    range = RangeIncrementValue * (multiplier - 1);
                }
            }
            else
            {
                range = RangeIncrementValue * multiplier;
            }

            return String.Format("{0} ft.", range);
        }

        private string AdjustToHit(int attackNumber)
        {
            int[] toHit = Helpers.Convert.AttackToIntArray(BaseRangedAttack);

            for (int i = 0; i < toHit.Length; i++)
            {
                if (HasPointBlankShot)
                {
                    if (attackNumber == 1)
                    {
                        toHit[i] += 1;
                    }
                    else if (HasFarShot)
                    {
                        toHit[i] -= attackNumber - 2;
                    }
                    else
                    {
                        toHit[i] -= 2 * (attackNumber - 2);
                    }
                }
                else
                {
                    toHit[i] -= 2 * (attackNumber - 1);
                }
            }

            return Helpers.Convert.AttackToString(toHit);
        }
    }
}
