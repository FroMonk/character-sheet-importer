using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Item : IHasDescription
    {
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";
        public int Qty { get; set; }

        public double UnitWeight { get; set; }

        public double Weight
        {
            get
            {
                return Qty * UnitWeight;
            }
        }

        public double Cost { get; set; }

        public Item(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;
        }

        public void SetQty(string qty)
        {
            Qty = Helpers.Convert.StatToInt(qty);
        }

        public void SetWeight(string weight)
        {
            var totalWeight = Math.Max(double.Parse(weight), 0d);

            UnitWeight = totalWeight > 0 && Qty > 0 ? totalWeight / Qty : 0;
        }
        
        public void SetCost(string cost)
        {
            Cost = double.Parse(cost);
        }
    }
}
