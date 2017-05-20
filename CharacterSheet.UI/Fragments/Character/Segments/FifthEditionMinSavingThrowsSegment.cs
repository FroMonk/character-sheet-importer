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
using Android.Graphics;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public class FifthEditionMinSavingThrowsSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _strProficient;
        private ScaledTextView _strTotal;
        private ScaledTextView _strTemp;

        private ScaledTextView _dexProficient;
        private ScaledTextView _dexTotal;
        private ScaledTextView _dexTemp;

        private ScaledTextView _conProficient;
        private ScaledTextView _conTotal;
        private ScaledTextView _conTemp;

        private ScaledTextView _intProficient;
        private ScaledTextView _intTotal;
        private ScaledTextView _intTemp;

        private ScaledTextView _wisProficient;
        private ScaledTextView _wisTotal;
        private ScaledTextView _wisTemp;

        private ScaledTextView _chaProficient;
        private ScaledTextView _chaTotal;
        private ScaledTextView _chaTemp;

        private ScaledImageButton _strRoll;
        private ScaledImageButton _dexRoll;
        private ScaledImageButton _conRoll;
        private ScaledImageButton _intRoll;
        private ScaledImageButton _wisRoll;
        private ScaledImageButton _chaRoll;

        public FifthEditionMinSavingThrowsSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            var savingThrows = _selectedCharacter.SavingThrows;

            var strength = savingThrows.Throws[SavingThrowType.Strength];
            var dexterity = savingThrows.Throws[SavingThrowType.Dexterity];
            var constitution = savingThrows.Throws[SavingThrowType.Constitution];
            var intelligence = savingThrows.Throws[SavingThrowType.Intelligence];
            var wisdom = savingThrows.Throws[SavingThrowType.Wisdom];
            var charisma = savingThrows.Throws[SavingThrowType.Charisma];

            _strTotal.SetStatValue(strength.Total);
            _strTemp.SetStatValue(strength.Adjustment);
            _dexTotal.SetStatValue(dexterity.Total);
            _dexTemp.SetStatValue(dexterity.Adjustment);
            _conTotal.SetStatValue(constitution.Total);
            _conTemp.SetStatValue(constitution.Adjustment);
            _intTotal.SetStatValue(intelligence.Total);
            _intTemp.SetStatValue(intelligence.Adjustment);
            _wisTotal.SetStatValue(wisdom.Total);
            _wisTemp.SetStatValue(wisdom.Adjustment);
            _chaTotal.SetStatValue(charisma.Total);
            _chaTemp.SetStatValue(charisma.Adjustment);

            if (!strength.IsProficient)
            {
                _strProficient.SetTextColor(Color.Black);
            }

            if (!dexterity.IsProficient)
            {
                _dexProficient.SetTextColor(Color.Black);
            }

            if (!constitution.IsProficient)
            {
                _conProficient.SetTextColor(Color.Black);
            }

            if (!intelligence.IsProficient)
            {
                _intProficient.SetTextColor(Color.Black);
            }

            if (!wisdom.IsProficient)
            {
                _wisProficient.SetTextColor(Color.Black);
            }

            if (!charisma.IsProficient)
            {
                _chaProficient.SetTextColor(Color.Black);
            }
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _strProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthProficient);
            _strTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthTotal);
            _strTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthTemp);
            _strRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsStrengthRoll);

            _dexProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityProficient);
            _dexTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityTotal);
            _dexTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityTemp);
            _dexRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsDexterityRoll);

            _conProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionProficient);
            _conTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionTotal);
            _conTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionTemp);
            _conRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsConstitutionRoll);

            _intProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceProficient);
            _intTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceTotal);
            _intTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceTemp);
            _intRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsIntelligenceRoll);

            _wisProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomProficient);
            _wisTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomTotal);
            _wisTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomTemp);
            _wisRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsWisdomRoll);

            _chaProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaProficient);
            _chaTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaTotal);
            _chaTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaTemp);
            _chaRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsCharismaRoll);
        }

        protected override void AssignEvents()
        {
            var savingThrows = _selectedCharacter.SavingThrows;

            var strength = savingThrows.Throws[SavingThrowType.Strength];
            var dexterity = savingThrows.Throws[SavingThrowType.Dexterity];
            var constitution = savingThrows.Throws[SavingThrowType.Constitution];
            var intelligence = savingThrows.Throws[SavingThrowType.Intelligence];
            var wisdom = savingThrows.Throws[SavingThrowType.Wisdom];
            var charisma = savingThrows.Throws[SavingThrowType.Charisma];

            AssignCheckRollEvent(_strRoll, strength);
            AssignCheckRollEvent(_dexRoll, dexterity);
            AssignCheckRollEvent(_conRoll, constitution);
            AssignCheckRollEvent(_intRoll, intelligence);
            AssignCheckRollEvent(_wisRoll, wisdom);
            AssignCheckRollEvent(_chaRoll, charisma);

            Action action = () => AssignValues();

            AssignAdjustmentEvent(_strTemp, strength, action);
            AssignAdjustmentEvent(_dexTemp, dexterity, action);
            AssignAdjustmentEvent(_conTemp, constitution, action);
            AssignAdjustmentEvent(_intTemp, intelligence, action);
            AssignAdjustmentEvent(_wisTemp, wisdom, action);
            AssignAdjustmentEvent(_chaTemp, charisma, action);
        }
    }
}