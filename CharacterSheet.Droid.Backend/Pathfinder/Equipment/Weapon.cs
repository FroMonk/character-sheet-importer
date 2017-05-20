using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Weapon : IHasDescription
    {
        public CharacterStats CharacterStats { get; protected set; }

        public string Name { get; set; }
        public Critical Critical { get; set; }
        public Attack Attack { get; set; }
        public string Reach { get; set; } = "";
        public string Description { get; set; } = "";
        public string Hand { get; set; } = "";
        public string Size { get; set; } = "";
        public string Type { get; set; } = "";
        public bool IsLightWeapon { get; set; } = false;
        public Melee Melee { get; set; }
        public Ranged Ranged { get; set; }
        public bool IsUnarmed { get; set; } = false;
        public bool IsNonStandardMelee { get; set; } = false;
        public bool IsProficient { get; set; } = false;

        public Weapon(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }
    }
}
