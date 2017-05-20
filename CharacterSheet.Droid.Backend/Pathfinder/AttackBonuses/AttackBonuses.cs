using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class AttackBonuses
    {
        public Dictionary<AttackBonusType, AttackBonus> Bonuses { get; set; } = new Dictionary<AttackBonusType, AttackBonus>();
        public List<string> ConditionalModifiers { get; set; } = new List<string>();

        public AttackBonuses(CharacterStats characterStats)
        {
            Bonuses.Add(AttackBonusType.Bab, new AttackBonus("BASE ATTACK BONUS", AttackBonusType.Bab, characterStats));
            Bonuses.Add(AttackBonusType.Melee, new AttackBonus("MELEE", AttackBonusType.Melee, characterStats));
            Bonuses.Add(AttackBonusType.Ranged, new AttackBonus("RANGED", AttackBonusType.Ranged, characterStats));
            Bonuses.Add(AttackBonusType.Cmb, new AttackBonus("CMB", AttackBonusType.Cmb, characterStats));
        }
    }
}
