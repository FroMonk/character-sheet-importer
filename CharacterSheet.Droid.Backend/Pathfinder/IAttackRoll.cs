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
using CharacterSheet.Droid.Backend.Pathfinder.Dice;

namespace CharacterSheet.Droid.Backend.Pathfinder
{
    public interface IAttackRoll
    {
        string Name { get; }
        int[] ToHit { get; }
        AttackRoll Roll(bool isFifthEdition);
    }
}