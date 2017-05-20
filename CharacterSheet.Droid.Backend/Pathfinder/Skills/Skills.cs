using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Skills
    {
        public List<Skill> SkillsList { get; set; } = new List<Skill>();
        public string ConditionalModifiers { get; set; } = "";        
    }
}
