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
using CharacterSheet.UI.Views;
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.UI.Dialogs;
using CharacterSheet.Helpers;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public abstract class BaseAttacksSegment
    {
        protected MainActivity _activity;

        public BaseAttacksSegment(Activity activity)
        {
            Check.ForNullArgument(activity, nameof(activity));

            _activity = (MainActivity)activity;
        }

        protected void AssignAttackRollEvent(ScaledTextView button, IAttackRoll attackToRoll, bool isFifthEdition = false)
        {
            if (isFifthEdition)
            {
                button.Click += delegate
                {
                    new FifthEditionAttackDialog(_activity, attackToRoll);
                };
            }
            else
            {
                button.Click += delegate
                {
                    new AttackDialog(_activity, attackToRoll);
                };
            }
        }
    }
}