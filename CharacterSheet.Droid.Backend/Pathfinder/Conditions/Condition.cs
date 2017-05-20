using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Condition
    {
        public string Name { get; set; }
        public ConditionType ConditionType { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; } = "";

        public Dictionary<string, ConditionResult> ConditionResults = new Dictionary<string, ConditionResult>
        {
                { "speed", new ConditionResult() },
                { "ac", new ConditionResult() },
                { "toHit", new ConditionResult() },
                { "reflex", new ConditionResult() },
                { "melee", new ConditionResult() },
                { "acDex", new ConditionResult() },
                { "strSkill", new ConditionResult() },
                { "dexSkill", new ConditionResult() },
                { "opposedPerceptionSkill", new ConditionResult() },
                { "attackRolls", new ConditionResult() },
                { "sightPerceptionSkill", new ConditionResult() },
                { "initiative", new ConditionResult() },
                { "dex", new ConditionResult() },
                { "reactionPerceptionSkill", new ConditionResult() },
                { "savingThrows", new ConditionResult() },
                { "skillChecks", new ConditionResult() },
                { "abilityChecks", new ConditionResult() },
                { "cmbCmdExceptGrapple", new ConditionResult() },
                { "attackRollsAgainstSighted", new ConditionResult() },
                { "stealthStationary", new ConditionResult() },
                { "stealthMoving", new ConditionResult() },
                { "str", new ConditionResult() },
                { "canCast", new ConditionResult() },
                { "meleeAttackRolls", new ConditionResult() },
                { "rangedAttackRollsExceptCrossbow", new ConditionResult() },
                { "acAgainstMelee", new ConditionResult() },
                { "acAgainstRanged", new ConditionResult() },
                { "weaponDamageRolls", new ConditionResult() },
                { "savesAgainstFear", new ConditionResult() }
        };

        public Condition(string name, ConditionType conditionType)
        {
            Check.ForNullArgument(name, "name");
            Check.ForNullArgument(conditionType, "conditionType");

            Name = name;
            ConditionType = conditionType;
        }
    }
}
