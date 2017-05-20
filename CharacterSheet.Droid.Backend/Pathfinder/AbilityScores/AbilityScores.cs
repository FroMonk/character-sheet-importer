using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class AbilityScores
    {
        public Dictionary<AbilityScoreType, AbilityScore> Scores { get; set; } = new Dictionary<AbilityScoreType, AbilityScore>();

        public AbilityScores()
        {            
            Scores.Add(AbilityScoreType.Strength, new AbilityScore("Strength", "STR"));
            Scores.Add(AbilityScoreType.Dexterity, new AbilityScore("Dexterity", "DEX"));
            Scores.Add(AbilityScoreType.Constitution, new AbilityScore("Constitution", "CON"));
            Scores.Add(AbilityScoreType.Intelligence, new AbilityScore("Intelligence", "INT"));
            Scores.Add(AbilityScoreType.Wisdom, new AbilityScore("Wisdom", "WIS"));
            Scores.Add(AbilityScoreType.Charisma, new AbilityScore("Charisma", "CHA"));
        }
    }
}
