using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class ConditionResult
    {
        public float Adjustment { get; set; }
        public bool DoesStack { get; set; } = true;
    }
}
