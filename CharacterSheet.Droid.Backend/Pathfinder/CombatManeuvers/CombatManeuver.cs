using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class CombatManeuver : ICheckRoll
    {
        public CharacterStats CharacterStats { get; set; }

        public int CmbBase { get; set; }
        public int Cmb
        {
            get
            {
                return CharacterStats.AttackBonuses.Bonuses[AttackBonusType.Cmb].ToHit[0] + CmbDifference;
            }
        }

        public int CmdBase { get; set; }
        public int Cmd
        {
            get
            {
                return  CalculateCmd() + CmdDifference;
            }
        }

        public int CmbDifference { get; set; }
        public int CmdDifference { get; set; }

        public string Name { get; set; }

        public CombatManeuver(CharacterStats characterStats, string name)
        {
            Check.ForNullArgument(characterStats, "characterStats");
            Check.ForNullArgument(name, "name");

            CharacterStats = characterStats;
            Name = name;
        }

        public void SetCmb(string cmb)
        {
            CmbBase = Helpers.Convert.StatToInt(cmb);
        }

        public void SetCmd(string cmd)
        {
            CmdBase = Helpers.Convert.StatToInt(cmd);
        }

        public void DetermineDifferences()
        {
            CmbDifference = CmbBase - CharacterStats.AttackBonuses.Bonuses[AttackBonusType.Cmb].ToHit[0];
            CmdDifference = CmdBase - CalculateCmd();
        }

        private int CalculateCmd()
        {
            return 10 + CharacterStats.AttackBonuses.Bonuses[AttackBonusType.Bab].ToHit[0] + CharacterStats.AbilityScores.Scores[AbilityScoreType.Strength].TempModifier + CharacterStats.AbilityScores.Scores[AbilityScoreType.Dexterity].TempModifier + (int)CharacterStats.Bio.Size;
        }

        public CheckResult Roll()
        {
            return new CheckResult(Cmb);
        }
    }
}
