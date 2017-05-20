using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Campaign
    {
        public string Name { get; set; }
        public List<CharacterStats> Characters { get; set; } = new List<CharacterStats>();
        public List<string> Notes { get; set; } = new List<string>();

        public Campaign(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }
    }
}
