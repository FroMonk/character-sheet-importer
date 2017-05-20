using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Melee
    {
        public Attack OneWeaponPrimaryHand { get; set; }
        public Attack OneWeaponOffhand { get; set; }
        public Attack TwoHands { get; set; }
        public Attack TwoWeaponsPrimaryHandOtherHeavy { get; set; }
        public Attack TwoWeaponsPrimaryHandOtherLight { get; set; }
        public Attack TwoWeaponsOffhand { get; set; }
    }
}
