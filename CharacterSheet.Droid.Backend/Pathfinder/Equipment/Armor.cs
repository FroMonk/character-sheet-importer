using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Armor : IHasDescription
    {
        public string Name { get; set; }
        public int ArmorBonus { get; set; }
        public int MaxDexBonus { get; set; }
        public int CheckPenalty { get; set; }
        public int SpellFailure { get; set; }
        public string Description { get; set; } = "";

        public Armor(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }
    
        public void SetArmorBonus(string armorBonus)
        {
            ArmorBonus = Helpers.Convert.StatToInt(armorBonus);
        }

        public void SetMaxDexBonus(string maxDexBonus)
        {
            MaxDexBonus = Helpers.Convert.StatToInt(maxDexBonus);
        }

        public void SetCheckPenalty(string checkPenalty)
        {
            CheckPenalty = Helpers.Convert.StatToInt(checkPenalty);
        }

        public void SetSpellFailure(string spellFailure)
        {
            SpellFailure = Helpers.Convert.StatToInt(spellFailure);
        }
    }
}
