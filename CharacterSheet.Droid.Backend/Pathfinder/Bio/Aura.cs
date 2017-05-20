using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Aura : IHasDescription
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string SourceText { get; set; } = "";
        public string ShortName { get; set; }
        public string Description { get; set; } = "";

        public Aura(string name, string shortName, string type)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(shortName, "shortName");
            Check.ForNullArgument(type, "type");

            Name = name;
            Type = type;
            ShortName = shortName;
        }
    }
}
