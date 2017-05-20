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
    public class AbilityScoresSegment : BaseCharacterSegement
    {
        private ScaledTextView _strScore;
        private ScaledTextView _strMod;
        private ScaledTextView _strTempScore;
        private ScaledTextView _strTempMod;
        private ScaledTextView _dexScore;
        private ScaledTextView _dexMod;
        private ScaledTextView _dexTempScore;
        private ScaledTextView _dexTempMod;
        private ScaledTextView _conScore;
        private ScaledTextView _conMod;
        private ScaledTextView _conTempScore;
        private ScaledTextView _conTempMod;
        private ScaledTextView _intScore;
        private ScaledTextView _intMod;
        private ScaledTextView _intTempScore;
        private ScaledTextView _intTempMod;
        private ScaledTextView _wisScore;
        private ScaledTextView _wisMod;
        private ScaledTextView _wisTempScore;
        private ScaledTextView _wisTempMod;
        private ScaledTextView _chaScore;
        private ScaledTextView _chaMod;
        private ScaledTextView _chaTempScore;
        private ScaledTextView _chaTempMod;

        private ScaledImageButton _strRoll;
        private ScaledImageButton _dexRoll;
        private ScaledImageButton _conRoll;
        private ScaledImageButton _intRoll;
        private ScaledImageButton _wisRoll;
        private ScaledImageButton _chaRoll;

        public AbilityScoresSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _strScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityStrScore);
            _strMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityStrModifier);
            _strTempScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityStrTempScore);
            _strTempMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityStrTempModifier);
            _strRoll = (ScaledImageButton)root.FindViewById(Resource.Id.abilityStrRoll);

            _dexScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityDexScore);
            _dexMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityDexModifier);
            _dexTempScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityDexTempScore);
            _dexTempMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityDexTempModifier);
            _dexRoll = (ScaledImageButton)root.FindViewById(Resource.Id.abilityDexRoll);

            _conScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityConScore);
            _conMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityConModifier);
            _conTempScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityConTempScore);
            _conTempMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityConTempModifier);
            _conRoll = (ScaledImageButton)root.FindViewById(Resource.Id.abilityConRoll);

            _intScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityIntScore);
            _intMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityIntModifier);
            _intTempScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityIntTempScore);
            _intTempMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityIntTempModifier);
            _intRoll = (ScaledImageButton)root.FindViewById(Resource.Id.abilityIntRoll);

            _wisScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityWisScore);
            _wisMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityWisModifier);
            _wisTempScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityWisTempScore);
            _wisTempMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityWisTempModifier);
            _wisRoll = (ScaledImageButton)root.FindViewById(Resource.Id.abilityWisRoll);

            _chaScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityChaScore);
            _chaMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityChaModifier);
            _chaTempScore = (ScaledTextView)root.FindViewById(Resource.Id.abilityChaTempScore);
            _chaTempMod = (ScaledTextView)root.FindViewById(Resource.Id.abilityChaTempModifier);
            _chaRoll = (ScaledImageButton)root.FindViewById(Resource.Id.abilityChaRoll);
        }

        public override void AssignValues()
        {
            var abilityScores = _selectedCharacter.AbilityScores;

            var strength = abilityScores.Scores[AbilityScoreType.Strength];
            var dexterity = abilityScores.Scores[AbilityScoreType.Dexterity];
            var constitution = abilityScores.Scores[AbilityScoreType.Constitution];
            var intelligence = abilityScores.Scores[AbilityScoreType.Intelligence];
            var wisdom = abilityScores.Scores[AbilityScoreType.Wisdom];
            var charisma = abilityScores.Scores[AbilityScoreType.Charisma];

            _strScore.SetStatValue(strength.Score);
            _strMod.SetStatValue(strength.Modifier);
            _strTempScore.SetStatValue(strength.TempScore);
            _strTempMod.SetStatValue(strength.TempModifier);
            _dexScore.SetStatValue(dexterity.Score);
            _dexMod.SetStatValue(dexterity.Modifier);
            _dexTempScore.SetStatValue(dexterity.TempScore);
            _dexTempMod.SetStatValue(dexterity.TempModifier);
            _conScore.SetStatValue(constitution.Score);
            _conMod.SetStatValue(constitution.Modifier);
            _conTempScore.SetStatValue(constitution.TempScore);
            _conTempMod.SetStatValue(constitution.TempModifier);
            _intScore.SetStatValue(intelligence.Score);
            _intMod.SetStatValue(intelligence.Modifier);
            _intTempScore.SetStatValue(intelligence.TempScore);
            _intTempMod.SetStatValue(intelligence.TempModifier);
            _wisScore.SetStatValue(wisdom.Score);
            _wisMod.SetStatValue(wisdom.Modifier);
            _wisTempScore.SetStatValue(wisdom.TempScore);
            _wisTempMod.SetStatValue(wisdom.TempModifier);
            _chaScore.SetStatValue(charisma.Score);
            _chaMod.SetStatValue(charisma.Modifier);
            _chaTempScore.SetStatValue(charisma.TempScore);
            _chaTempMod.SetStatValue(charisma.TempModifier);
        }

        protected override void AssignEvents()
        {
            var abilityScores = _selectedCharacter.AbilityScores;

            var strength = abilityScores.Scores[AbilityScoreType.Strength];
            var dexterity = abilityScores.Scores[AbilityScoreType.Dexterity];
            var constitution = abilityScores.Scores[AbilityScoreType.Constitution];
            var intelligence = abilityScores.Scores[AbilityScoreType.Intelligence];
            var wisdom = abilityScores.Scores[AbilityScoreType.Wisdom];
            var charisma = abilityScores.Scores[AbilityScoreType.Charisma];

            AssignCheckRollEvent(_strRoll, strength);
            AssignCheckRollEvent(_dexRoll, dexterity);
            AssignCheckRollEvent(_conRoll, constitution);
            AssignCheckRollEvent(_intRoll, intelligence);
            AssignCheckRollEvent(_wisRoll, wisdom);
            AssignCheckRollEvent(_chaRoll, charisma);
                        
            AssignAdjustmentEvent(_strTempScore, strength, () =>
            {
                AssignValues();
                _characterFragment.Attacks.AssignValues();
                _characterFragment.CombatManeuvers.UpdateValues();
            });

            AssignAdjustmentEvent(_dexTempScore, dexterity, () =>
            {
                AssignValues();
                _characterFragment.Attacks.AssignValues();
                _characterFragment.Initiative.AssignValues();
                _characterFragment.Ac.AssignValues();
                _characterFragment.SavingThrows.AssignValues();
                _characterFragment.CombatManeuvers.UpdateValues();
            });

            AssignAdjustmentEvent(_conTempScore, constitution, () =>
            {
                AssignValues();
                _characterFragment.Hp.AssignValues();
                _characterFragment.SavingThrows.AssignValues();
            });

            AssignAdjustmentEvent(_intTempScore, intelligence, () => AssignValues());

            AssignAdjustmentEvent(_wisTempScore, wisdom, () =>
            {
                AssignValues();
                _characterFragment.SavingThrows.AssignValues();
            });

            AssignAdjustmentEvent(_chaTempScore, charisma, () => AssignValues());
        }
    }
}