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
    public class MinSavingThrowsSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _fortTotal;
        private ScaledTextView _fortTemp;
        private ScaledTextView _refTotal;
        private ScaledTextView _refTemp;
        private ScaledTextView _willTotal;
        private ScaledTextView _willTemp;

        private ScaledImageButton _refRoll;
        private ScaledImageButton _fortRoll;
        private ScaledImageButton _willRoll;

        public MinSavingThrowsSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            var savingThrows = _selectedCharacter.SavingThrows;

            var fortitude = savingThrows.Throws[SavingThrowType.Fortitude];
            var reflex = savingThrows.Throws[SavingThrowType.Reflex];
            var will = savingThrows.Throws[SavingThrowType.Will];

            _fortTotal.SetStatValue(fortitude.Total);
            _fortTemp.SetStatValue(fortitude.Adjustment);
            _refTotal.SetStatValue(reflex.Total);
            _refTemp.SetStatValue(reflex.Adjustment);
            _willTotal.SetStatValue(will.Total);
            _willTemp.SetStatValue(will.Adjustment);
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _fortTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeTotal);
            _fortTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeTemp);
            _fortRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsFortitudeRoll);

            _refTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexTotal);
            _refTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexTemp);
            _refRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsReflexRoll);

            _willTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillTotal);
            _willTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillTemp);
            _willRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsWillRoll);
        }

        protected override void AssignEvents()
        {
            var savingThrows = _selectedCharacter.SavingThrows;

            var fortitude = savingThrows.Throws[SavingThrowType.Fortitude];
            var reflex = savingThrows.Throws[SavingThrowType.Reflex];
            var will = savingThrows.Throws[SavingThrowType.Will];

            AssignCheckRollEvent(_fortRoll, fortitude);
            AssignCheckRollEvent(_refRoll, reflex);
            AssignCheckRollEvent(_willRoll, will);

            Action action = () => AssignValues();

            AssignAdjustmentEvent(_fortTemp, fortitude, action);
            AssignAdjustmentEvent(_refTemp, reflex, action);
            AssignAdjustmentEvent(_willTemp, will, action);
        }
    }
}