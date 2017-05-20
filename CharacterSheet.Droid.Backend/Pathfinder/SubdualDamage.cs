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
using CharacterSheet.Pathfinder;
using CharacterSheet.Helpers;

namespace CharacterSheet.Droid.Backend.Pathfinder
{
    [Serializable]
    public class SubdualDamage : IAdjustable
    {
        public CharacterStats CharacterStats { get; set; }

        public string AdjustmentName { get; } = "Subdual Damage";

        private int _adjustment;
        public int Adjustment
        {
            get
            {
                return _adjustment;
            }

            set
            {
                _adjustment = Math.Max(Math.Min(value, CharacterStats.Health.MaxHp), 0);
            }
        }

        public SubdualDamage(CharacterStats characterStats)
        {
            Check.ForNullArgument(characterStats, nameof(characterStats));

            CharacterStats = characterStats;
        }

        public void SetSubdualDamage(string subdualDamage)
        {
            Adjustment = Math.Max(Helpers.Convert.StatToInt(subdualDamage), 0);
        }
    }
}