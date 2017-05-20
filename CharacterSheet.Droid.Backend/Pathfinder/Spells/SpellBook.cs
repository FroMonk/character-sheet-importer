using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class SpellBook
    {
        public string Name { get; set; }
        public List<Spell> Spells { get; set; } = new List<Spell>();

        public SpellBook(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }
    }
}
