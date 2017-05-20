using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Gear
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public string EncumbranceLevel { get; set; } = "";

        public double Carried
        {
            get
            {
                return Items.Sum(x => x.Weight);
            }
        }
    }
}
