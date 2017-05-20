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
    public class MinAttacksSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _meleeTotal;
        private ScaledTextView _meleeTemp;
        private ScaledTextView _rangedTotal;
        private ScaledTextView _rangedTemp;
        private ScaledTextView _cmbTotal;
        private ScaledTextView _cmbTemp;
        private ScaledTextView _babTotal;
        private ScaledTextView _babTemp;

        private ScaledImageButton _meleeRoll;
        private ScaledImageButton _rangedRoll;
        private ScaledImageButton _cmbRoll;
        private ScaledImageButton _babRoll;

        public MinAttacksSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            var attacksBonuses = _selectedCharacter.AttackBonuses;

            var melee = attacksBonuses.Bonuses[AttackBonusType.Melee];
            var ranged = attacksBonuses.Bonuses[AttackBonusType.Ranged];
            var cmb = attacksBonuses.Bonuses[AttackBonusType.Cmb];
            var bab = attacksBonuses.Bonuses[AttackBonusType.Bab];

            _meleeTotal.SetStatValue(melee.ToHit);
            _meleeTemp.SetStatValue(melee.Adjustment);
            _rangedTotal.SetStatValue(ranged.ToHit);
            _rangedTemp.SetStatValue(ranged.Adjustment);
            _cmbTotal.SetStatValue(cmb.ToHit);
            _cmbTemp.SetStatValue(cmb.Adjustment);
            _babTotal.SetStatValue(bab.ToHit);
            _babTemp.SetStatValue(bab.Adjustment);
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _meleeTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeTotal);
            _meleeTemp = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeTemp);
            _rangedTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedTotal);
            _rangedTemp = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedTemp);
            _cmbTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbTotal);
            _cmbTemp = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbTemp);
            _babTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksBabTotal);
            _babTemp = (ScaledTextView)root.FindViewById(Resource.Id.attacksBabTemp);

            _meleeRoll = (ScaledImageButton)root.FindViewById(Resource.Id.attacksMeleeRoll);
            _rangedRoll = (ScaledImageButton)root.FindViewById(Resource.Id.attacksRangedRoll);
            _cmbRoll = (ScaledImageButton)root.FindViewById(Resource.Id.attacksCmbRoll);
            _babRoll = (ScaledImageButton)root.FindViewById(Resource.Id.babRoll);
        }

        protected override void AssignEvents()
        {
            var attacksBonuses = _selectedCharacter.AttackBonuses;

            var melee = attacksBonuses.Bonuses[AttackBonusType.Melee];
            var ranged = attacksBonuses.Bonuses[AttackBonusType.Ranged];
            var cmb = attacksBonuses.Bonuses[AttackBonusType.Cmb];
            var bab = attacksBonuses.Bonuses[AttackBonusType.Bab];

            AssignAttackRollEvent(_meleeRoll, melee);
            AssignAttackRollEvent(_rangedRoll, ranged);
            AssignAttackRollEvent(_cmbRoll, cmb);
            AssignAttackRollEvent(_babRoll, bab);

            Action action = () =>
            {
                AssignValues();
                _characterFragment.CombatManeuvers.UpdateValues();
            };

            AssignAdjustmentEvent(_meleeTemp, melee, action);
            AssignAdjustmentEvent(_rangedTemp, ranged, action);
            AssignAdjustmentEvent(_cmbTemp, cmb, action);
            AssignAdjustmentEvent(_babTemp, bab, action);
        }
    }
}