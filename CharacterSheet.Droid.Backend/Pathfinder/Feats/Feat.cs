using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Feat : IHasDescription
    {
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";

        public Feat(string name)
        {
            Check.ForNullArgument(name, "name");
            
            Name = name;
        }
    }
}
