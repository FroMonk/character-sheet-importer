using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Spells
    {
        public Dictionary<string, SpellClass> SpellClasses { get; set; } = new Dictionary<string, SpellClass>();
        public List<SpellBook> SpellBooks { get; set; } = new List<SpellBook>();

        public void Reset()
        {
            foreach (var spellClass in SpellClasses)
            {
                foreach (var level in spellClass.Value.Levels)
                {
                    level.Value.Reset();
                }
            }
        }
    }
}
