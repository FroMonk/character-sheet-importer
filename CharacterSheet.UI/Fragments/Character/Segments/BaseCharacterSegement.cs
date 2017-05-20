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
using CharacterSheet.Helpers;
using CharacterSheet.UI.Views;
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.UI.Dialogs;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public abstract class BaseCharacterSegement
    {
        protected Activity _activity;
        protected CharacterStats _selectedCharacter;
        protected CharacterFragment _characterFragment;

        public BaseCharacterSegement(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment)
        {
            Check.ForNullArgument(activity, "activity");
            Check.ForNullArgument(root, "root");
            Check.ForNullArgument(selectedCharacter, "selectedCharacter");
            Check.ForNullArgument(characterFragment, nameof(characterFragment));

            _activity = activity;
            _selectedCharacter = selectedCharacter;
            _characterFragment = characterFragment;

            AssociateViews(root);
            AssignValues();
            AssignEvents();
        }

        protected abstract void AssociateViews(ViewGroup root);
        public abstract void AssignValues();

        protected virtual void AssignEvents()
        {
        }

        protected void AssignCheckRollEvent(ScaledImageButton button, ICheckRoll checkToRoll)
        {
            button.Click += delegate
            {
                new CheckDialog(_activity, checkToRoll);
            };
        }

        protected void AssignAttackRollEvent(ScaledImageButton button, IAttackRoll attackToRoll)
        {
            button.Click += delegate
            {
                new AttackDialog(_activity, attackToRoll);
            };
        }

        protected void AssignAdjustmentEvent(View view, IAdjustable thingToAdjust, Action action)
        {
            view.Click += delegate
            {
                new AdjustmentDialog(_activity, thingToAdjust, action);
            };
        }
    }
}