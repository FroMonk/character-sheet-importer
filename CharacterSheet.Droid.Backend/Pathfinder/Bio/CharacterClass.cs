using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class CharacterClass
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public CasterSourceType CasterSource { get; set; } = CasterSourceType.None;
        public int BaseSpellDc { get; set; }
        public string OvercomeSpellResistance { get; set; } = "";
        public string ConcentrationCheck { get; set; } = "";
        public int CasterLevel { get; set; }
        public SpellSourceType SpellSource { get; set; } = SpellSourceType.None;
        public string ArcaneSpellFailure { get; set; } = "";

        public CharacterClass(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }        

        public void SetSpellSource(string spellSource)
        {
            if (!string.IsNullOrEmpty(spellSource))
            {
                SpellSource = (SpellSourceType) Enum.Parse(typeof (SpellSourceType), Prepare.ForStringToEnumConversion(spellSource));
            }
        }

        public void SetCasterLevel(string casterLevel)
        {
            CasterLevel = Helpers.Convert.StatToInt(casterLevel);
        }

        public void SetBaseSpellDc(string baseSpellDc)
        {
            BaseSpellDc = Helpers.Convert.StatToInt(baseSpellDc);
        }

        public void SetCasterSource(string casterSource)
        {
            if (!string.IsNullOrEmpty(casterSource))
            {
                CasterSource = (CasterSourceType) Enum.Parse(typeof (CasterSourceType), Prepare.ForStringToEnumConversion(casterSource));
            }
        }

        public void SetLevel(string level)
        {
            Level = Helpers.Convert.StatToInt(level);
        }
    }
}
