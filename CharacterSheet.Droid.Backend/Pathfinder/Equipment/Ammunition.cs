using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Ammunition : IHasDescription
    {
        public int Remaining { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        
        public Ammunition(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }

        public void SetRemaining(string total)
        {
            if (!string.IsNullOrEmpty(total))
            {
                Remaining = Helpers.Convert.StatToInt(total);
            }
        }
    }
}
