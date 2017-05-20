using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Skill : IHasDescription, ICheckRoll
    {
        private readonly Dictionary<string, AbilityScoreType> _abilityScoreTypeTextMap = new Dictionary<string, AbilityScoreType>
        {
            ["STR"] = AbilityScoreType.Strength,
            ["DEX"] = AbilityScoreType.Dexterity,
            ["CON"] = AbilityScoreType.Constitution,
            ["INT"] = AbilityScoreType.Intelligence,
            ["WIS"] = AbilityScoreType.Wisdom,
            ["CHA"] = AbilityScoreType.Charisma
        };

        public CharacterStats CharacterStats { get; protected set; }
        
        public string Name { get; set; }
        public string Ability { get; set; }

        public int SkillModifier
        {
            get
            {
                return AbilityModifier + Ranks + MiscModifier;
            }
        }

        public int AbilityModifier
        {
            get
            {
                if (Ability == null)
                {
                    return 0;
                }

                return CharacterStats.AbilityScores.Scores[_abilityScoreTypeTextMap[Ability]].TempModifier;
            }            
        }

        private int _ranks;
        public int Ranks
        {
            get
            {
                return IsProficient ? CharacterStats.Proficiency : _ranks;
            }

            set
            {
                _ranks = value;
            }
        }

        public int MiscModifier { get; set; }
        public string Description { get; set; } = "";
        public bool UseUntrained { get; set; } = false;
        public bool IsProficient { get; set; } = false;

        public Skill(string name, CharacterStats characterStats)
        {
            Check.ForNullArgument(name, nameof(name));
            Check.ForNullArgument(characterStats, nameof(characterStats));

            Name = name;
            CharacterStats = characterStats;
        }

        public void SetUseUntrained(string useUntrained)
        {
            Check.ForNullArgument(useUntrained, "useUntrained");

            if (useUntrained.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                UseUntrained = true;
            }
            else
            {
                UseUntrained = false;
            }
        }

        public void SetRanks(string ranks)
        {
            if (ranks.Contains("."))
            {
                ranks = ranks.Split('.')[0];
            }

            Ranks = Helpers.Convert.StatToInt(ranks);
        }

        public void SetMiscModifier(string modifier)
        {
            MiscModifier = Helpers.Convert.StatToInt(modifier);
        }

        public CheckResult Roll()
        {
            return new CheckResult(SkillModifier);
        }
    }
}
