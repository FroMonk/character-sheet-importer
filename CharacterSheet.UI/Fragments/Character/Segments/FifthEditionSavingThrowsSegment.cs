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
    public class FifthEditionSavingThrowsSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _strProficient;
        private ScaledTextView _strTotal;
        private ScaledTextView _strBase;
        private ScaledTextView _strAbility;
        private ScaledTextView _strMagic;
        private ScaledTextView _strEpic;
        private ScaledTextView _strMisc;
        private ScaledTextView _strTemp;

        private ScaledTextView _dexProficient;
        private ScaledTextView _dexTotal;
        private ScaledTextView _dexBase;
        private ScaledTextView _dexAbility;
        private ScaledTextView _dexMagic;
        private ScaledTextView _dexEpic;
        private ScaledTextView _dexMisc;
        private ScaledTextView _dexTemp;

        private ScaledTextView _conProficient;
        private ScaledTextView _conTotal;
        private ScaledTextView _conBase;
        private ScaledTextView _contAbility;
        private ScaledTextView _conMagic;
        private ScaledTextView _conEpic;
        private ScaledTextView _conMisc;
        private ScaledTextView _conTemp;

        private ScaledTextView _intProficient;
        private ScaledTextView _intTotal;
        private ScaledTextView _intBase;
        private ScaledTextView _intAbility;
        private ScaledTextView _intMagic;
        private ScaledTextView _intEpic;
        private ScaledTextView _intMisc;
        private ScaledTextView _intTemp;

        private ScaledTextView _wisProficient;
        private ScaledTextView _wisTotal;
        private ScaledTextView _wisBase;
        private ScaledTextView _wisAbility;
        private ScaledTextView _wisMagic;
        private ScaledTextView _wisEpic;
        private ScaledTextView _wisMisc;
        private ScaledTextView _wisTemp;

        private ScaledTextView _chaProficient;
        private ScaledTextView _chaTotal;
        private ScaledTextView _chaBase;
        private ScaledTextView _chatAbility;
        private ScaledTextView _chaMagic;
        private ScaledTextView _chaEpic;
        private ScaledTextView _chaMisc;
        private ScaledTextView _chaTemp;

        private ScaledImageButton _strRoll;
        private ScaledImageButton _dexRoll;
        private ScaledImageButton _conRoll;
        private ScaledImageButton _intRoll;
        private ScaledImageButton _wisRoll;
        private ScaledImageButton _chaRoll;

        public FifthEditionSavingThrowsSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
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
            _strBase.SetStatValue(strength.Base);
            _strAbility.SetStatValue(strength.Ability);
            _strMagic.SetStatValue(strength.Magic);
            _strEpic.SetStatValue(strength.Epic);
            _strMisc.SetStatValue(strength.Misc);
            _strTemp.SetStatValue(strength.Adjustment);

            _dexTotal.SetStatValue(dexterity.Total);
            _dexBase.SetStatValue(dexterity.Base);
            _dexAbility.SetStatValue(dexterity.Ability);
            _dexMagic.SetStatValue(dexterity.Magic);
            _dexEpic.SetStatValue(dexterity.Epic);
            _dexMisc.SetStatValue(dexterity.Misc);
            _dexTemp.SetStatValue(dexterity.Adjustment);

            _conTotal.SetStatValue(constitution.Total);
            _conBase.SetStatValue(constitution.Base);
            _contAbility.SetStatValue(constitution.Ability);
            _conMagic.SetStatValue(constitution.Magic);
            _conEpic.SetStatValue(constitution.Epic);
            _conMisc.SetStatValue(constitution.Misc);
            _conTemp.SetStatValue(constitution.Adjustment);

            _intTotal.SetStatValue(intelligence.Total);
            _intBase.SetStatValue(intelligence.Base);
            _intAbility.SetStatValue(intelligence.Ability);
            _intMagic.SetStatValue(intelligence.Magic);
            _intEpic.SetStatValue(intelligence.Epic);
            _intMisc.SetStatValue(intelligence.Misc);
            _intTemp.SetStatValue(intelligence.Adjustment);

            _wisTotal.SetStatValue(wisdom.Total);
            _wisBase.SetStatValue(wisdom.Base);
            _wisAbility.SetStatValue(wisdom.Ability);
            _wisMagic.SetStatValue(wisdom.Magic);
            _wisEpic.SetStatValue(wisdom.Epic);
            _wisMisc.SetStatValue(wisdom.Misc);
            _wisTemp.SetStatValue(wisdom.Adjustment);

            _chaTotal.SetStatValue(charisma.Total);
            _chaBase.SetStatValue(charisma.Base);
            _chatAbility.SetStatValue(charisma.Ability);
            _chaMagic.SetStatValue(charisma.Magic);
            _chaEpic.SetStatValue(charisma.Epic);
            _chaMisc.SetStatValue(charisma.Misc);
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
            _strBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthBase);
            _strAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthAbility);
            _strMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthMagic);
            _strEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthEpic);
            _strMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthMisc);
            _strTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsStrengthTemp);
            _strRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsStrengthRoll);

            _dexProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityProficient);
            _dexTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityTotal);
            _dexBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityBase);
            _dexAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityAbility);
            _dexMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityMagic);
            _dexEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityEpic);
            _dexMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityMisc);
            _dexTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsDexterityTemp);
            _dexRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsDexterityRoll);

            _conProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionProficient);
            _conTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionTotal);
            _conBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionBase);
            _contAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionAbility);
            _conMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionMagic);
            _conEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionEpic);
            _conMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionMisc);
            _conTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsConstitutionTemp);
            _conRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsConstitutionRoll);

            _intProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceProficient);
            _intTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceTotal);
            _intBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceBase);
            _intAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceAbility);
            _intMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceMagic);
            _intEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceEpic);
            _intMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceMisc);
            _intTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsIntelligenceTemp);
            _intRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsIntelligenceRoll);

            _wisProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomProficient);
            _wisTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomTotal);
            _wisBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomBase);
            _wisAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomAbility);
            _wisMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomMagic);
            _wisEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomEpic);
            _wisMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomMisc);
            _wisTemp = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsWisdomTemp);
            _wisRoll = (ScaledImageButton)root.FindViewById(Resource.Id.savingThrowsWisdomRoll);

            _chaProficient = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaProficient);
            _chaTotal = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaTotal);
            _chaBase = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaBase);
            _chatAbility = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaAbility);
            _chaMagic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaMagic);
            _chaEpic = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaEpic);
            _chaMisc = (ScaledTextView)root.FindViewById(Resource.Id.savingThrowsCharismaMisc);
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