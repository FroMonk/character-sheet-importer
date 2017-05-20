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
    public class SavingThrowsSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _fortTotal;
        private ScaledTextView _fortBase;
        private ScaledTextView _fortAbility;
        private ScaledTextView _fortMagic;
        private ScaledTextView _fortEpic;
        private ScaledTextView _fortMisc;
        private ScaledTextView _fortTemp;
        private ScaledTextView _refTotal;
        private ScaledTextView _refBase;
        private ScaledTextView _refAbility;
        private ScaledTextView _refMagic;
        private ScaledTextView _refEpic;
        private ScaledTextView _refMisc;
        private ScaledTextView _refTemp;
        private ScaledTextView _willTotal;
        private ScaledTextView _willBase;
        private ScaledTextView _willAbility;
        private ScaledTextView _willMagic;
        private ScaledTextView _willEpic;
        private ScaledTextView _willMisc;
        private ScaledTextView _willTemp;

        private ScaledImageButton _refRoll;
        private ScaledImageButton _fortRoll;
        private ScaledImageButton _willRoll;

        public SavingThrowsSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            var savingThrows = _selectedCharacter.SavingThrows;

            var fortitude = savingThrows.Throws[SavingThrowType.Fortitude];
            var reflex = savingThrows.Throws[SavingThrowType.Reflex];
            var will = savingThrows.Throws[SavingThrowType.Will];

            _fortTotal.SetStatValue(fortitude.Total);
            _fortBase.SetStatValue(fortitude.Base);
            _fortAbility.SetStatValue(fortitude.Ability);
            _fortMagic.SetStatValue(fortitude.Magic);
            _fortEpic.SetStatValue(fortitude.Epic);
            _fortMisc.SetStatValue(fortitude.Misc);
            _fortTemp.SetStatValue(fortitude.Adjustment);
            _refTotal.SetStatValue(reflex.Total);
            _refBase.SetStatValue(reflex.Base);
            _refAbility.SetStatValue(reflex.Ability);
            _refMagic.SetStatValue(reflex.Magic);
            _refEpic.SetStatValue(reflex.Epic);
            _refMisc.SetStatValue(reflex.Misc);
            _refTemp.SetStatValue(reflex.Adjustment);
            _willTotal.SetStatValue(will.Total);
            _willBase.SetStatValue(will.Base);
            _willAbility.SetStatValue(will.Ability);
            _willMagic.SetStatValue(will.Magic);
            _willEpic.SetStatValue(will.Epic);
            _willMisc.SetStatValue(will.Misc);
            _willTemp.SetStatValue(will.Adjustment);
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _fortTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeTotal);
            _fortBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeBase);
            _fortAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeAbility);
            _fortMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeMagic);
            _fortEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeEpic);
            _fortMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeMisc);
            _fortTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsFortitudeTemp);
            _fortRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsFortitudeRoll);

            _refTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexTotal);
            _refBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexBase);
            _refAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexAbility);
            _refMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexMagic);
            _refEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexEpic);
            _refMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexMisc);
            _refTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsReflexTemp);
            _refRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsReflexRoll);

            _willTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillTotal);
            _willBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillBase);
            _willAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillAbility);
            _willMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillMagic);
            _willEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillEpic);
            _willMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWillMisc);
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