using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class SavingThrows
    {
        public string ConditionalSaveModifiers { get; set; } = "";
        public Dictionary<SavingThrowType, SavingThrow> Throws { get; set; } = new Dictionary<SavingThrowType, SavingThrow>();
    }
}
