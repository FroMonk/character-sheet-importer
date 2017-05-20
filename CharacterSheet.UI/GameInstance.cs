using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI
{
    public class GameInstance : Java.Lang.Object, ISerializable
    {
        public List<Campaign> Campaigns { get; set; }
        public CharacterStats SelectedCharacter { get; set; }
    }
}