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
using CharacterSheet.Pathfinder;

namespace CharacterSheet.Droid.Backend.Pathfinder.Notes
{
    public class Note : IHasDescription
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}