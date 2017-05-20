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

namespace CharacterSheet.Droid.Backend.Pathfinder.Equipment
{
    public enum AttackType
    {
        Melee1HP,
        Melee1HO,
        Melee2H,
        Melee2WPOH,
        Melee2WPOL,
        Melee2WOH,
        Ranged
    }
}