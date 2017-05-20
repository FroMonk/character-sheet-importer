using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class TrackedResource
    {
        public string Name { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public int Used { get; set; }
        public int Left { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
    
        public TrackedResource(string name, string max, string min)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(max, "max");
            Check.ForNullArgument(min, "min");

            Name = name;
            Max = Helpers.Convert.StatToInt(max);
            Min = Helpers.Convert.StatToInt(min);
        }

        public void SetUsed(string used)
        {
            Used = Helpers.Convert.StatToInt(used);
        }

        public void SetLeft(string left)
        {
            Left = Helpers.Convert.StatToInt(left);
        }
    }
}
