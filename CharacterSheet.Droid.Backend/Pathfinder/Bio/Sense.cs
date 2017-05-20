using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Sense : IHasDescription
    {
        public string Name { get; set; }
        public string Description { get; set; } = "";

        public Sense(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }
    }
}
