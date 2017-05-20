using CharacterSheet.Droid.Backend.Pathfinder.Dice;
using CharacterSheet.Droid.Backend.Pathfinder.Equipment;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class RangedAttack : Attack
    {
        public RangedAttack(string title, string range, string damage, string toHit, CharacterStats characterStats, Weapon weapon) : base(AttackType.Ranged, title, damage, toHit, characterStats, weapon)
        {
            Check.ForNullArgument(range, nameof(range));

            Range = range;
        }

        public string Range { get; set; }
    }
}
