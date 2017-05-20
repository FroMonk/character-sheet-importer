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
using CharacterSheet.UI.Views;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public class InitiativeSegment : BaseCharacterSegement
    {
        private ScaledTextView _total;
        private ScaledTextView _dex;
        private ScaledTextView _misc;

        private ScaledImageButton Roll;

        public InitiativeSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            var initiative = _selectedCharacter.Initiative;

            _total.SetStatValue(initiative.Total);
            _dex.SetStatValue(initiative.Dex);
            _misc.SetStatValue(initiative.Misc);
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _total = (ScaledTextView)root.FindViewById(Resource.Id.initiativeTotal);
            _dex = (ScaledTextView)root.FindViewById(Resource.Id.initiativeDex);
            _misc = (ScaledTextView)root.FindViewById(Resource.Id.initiativeMisc);
            Roll = (ScaledImageButton)root.FindViewById(Resource.Id.initiativeRoll);
        }

        protected override void AssignEvents()
        {
            AssignCheckRollEvent(Roll, _selectedCharacter.Initiative);
        }
    }
}