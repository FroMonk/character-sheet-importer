using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Speed
    {
        public string MovementType { get; set; } = "";
        public int Value { get; set; }
        public int Base { get; set; }
        public string Unit { get; set; } = "";

        public void SetMovementType(string movementType)
        {
            MovementType = movementType;
        }

        public void SetValue(string value)
        {
            Value = Helpers.Convert.StatToInt(value);
        }

        public void SetBaseValue(string baseValue)
        {
            Base = Helpers.Convert.StatToInt(baseValue);
        }

        public void SetUnit(string unit)
        {
            Unit = unit;
        }

        public string GetSpeed()
        {
            return String.Format("{0} {1} {2}", MovementType, Base, Unit);
        }
    }
}
