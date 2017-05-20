using CharacterSheet.Helpers;
using CharacterSheet.Pathfinder;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet
{
    [Serializable]
    public class Game
    {
        public List<Campaign> Campaigns { get; set; } = new List<Campaign>();
        public CharacterStats SelectedCharacter { get; set; }
    }
}
