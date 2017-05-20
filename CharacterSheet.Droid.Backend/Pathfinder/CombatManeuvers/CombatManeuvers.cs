using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class CombatManeuvers
    {
        public Dictionary<string, CombatManeuver> Maneuvers { get; set; } = new Dictionary<string, CombatManeuver>();
        public List<string> Modifiers { get; set; } = new List<string>();
    }
}
