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
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public class MinAcSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _total;
        private ScaledTextView _touch;
        private ScaledTextView _flat;
        private ScaledTextView _temp;

        public MinAcSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _total = (ScaledTextView)root.FindViewById(Resource.Id.acTotal);
            _touch = (ScaledTextView)root.FindViewById(Resource.Id.acTouch);
            _flat = (ScaledTextView)root.FindViewById(Resource.Id.acFlat);
            _temp = (ScaledTextView)root.FindViewById(Resource.Id.acTemp);
        }

        public override void AssignValues()
        {
            var ac = _selectedCharacter.AC;

            _total.SetStatValue(ac.Total);
            _touch.SetStatValue(ac.Touch);
            _flat.SetStatValue(ac.Flat);
            _temp.SetStatValue(ac.Adjustment);
        }

        protected override void AssignEvents()
        { 
            AssignAdjustmentEvent(_temp, _selectedCharacter.AC, () => AssignValues());
        }
    }
}