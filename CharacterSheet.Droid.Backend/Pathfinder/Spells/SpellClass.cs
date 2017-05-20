using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class SpellClass
    {
        public string Name { get; set; }
        public string CasterClass { get; set; }
        public SpellSourceType SpellSource { get; set; }
        public int MaxSpellLevel { get; set; }
        public Dictionary<int, SpellClassLevel> Levels { get; set; } = new Dictionary<int, SpellClassLevel>();

        public Dictionary<int, SpellClassLevel> PopulatedLevels
        {
            get
            {
                return Levels.Where(x => x.Value.Spells.Any()).ToDictionary(x => x.Key, x=> x.Value);
            }
        }

        public SpellClass(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
            CasterClass = name;
            SpellSource = SpellSourceType.Unknown;
        }

        public SpellClass(string name, string spellSource, string maxSpellLevel)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(spellSource, "spellSource");
            Check.ForNullArgument(maxSpellLevel, "maxSpellLevel");

            Name = name;
            CasterClass = name;
            SpellSource = (SpellSourceType)Enum.Parse(typeof(SpellSourceType), Prepare.ForStringToEnumConversion(spellSource));
            MaxSpellLevel = Helpers.Convert.StatToInt(maxSpellLevel);
        }        
    }
}
