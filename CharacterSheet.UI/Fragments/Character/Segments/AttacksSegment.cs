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
    public class AttacksSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _meleeTotal;
        private ScaledTextView _meleeBase;
        private ScaledTextView _meleeStat;
        private ScaledTextView _meleeSize;
        private ScaledTextView _meleeEpic;
        private ScaledTextView _meleeMisc;
        private ScaledTextView _meleeTemp;
        private ScaledTextView _rangedTotal;
        private ScaledTextView _rangedBase;
        private ScaledTextView _rangedStat;
        private ScaledTextView _rangedSize;
        private ScaledTextView _rangedEpic;
        private ScaledTextView _rangedMisc;
        private ScaledTextView _rangedTemp;
        private ScaledTextView _cmbTotal;
        private ScaledTextView _cmbBase;
        private ScaledTextView _cmbStat;
        private ScaledTextView _cmbSize;
        private ScaledTextView _cmbEpic;
        private ScaledTextView _cmbMisc;
        private ScaledTextView _cmbTemp;
        private ScaledTextView _babTotal;
        private ScaledTextView _babTemp;

        private ScaledImageButton _meleeRoll;
        private ScaledImageButton _rangedRoll;
        private ScaledImageButton _cmbRoll;
        private ScaledImageButton _babRoll;

        public AttacksSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
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
            _meleeBase.SetStatValue(melee.Base);
            _meleeStat.SetStatValue(melee.Stat);
            _meleeSize.SetStatValue(melee.Size);
            _meleeEpic.SetStatValue(melee.Epic);
            _meleeMisc.SetStatValue(melee.Misc);
            _meleeTemp.SetStatValue(melee.Adjustment);
            _rangedTotal.SetStatValue(ranged.ToHit);
            _rangedBase.SetStatValue(ranged.Base);
            _rangedStat.SetStatValue(ranged.Stat);
            _rangedSize.SetStatValue(ranged.Size);
            _rangedEpic.SetStatValue(ranged.Epic);
            _rangedMisc.SetStatValue(ranged.Misc);
            _rangedTemp.SetStatValue(ranged.Adjustment);
            _cmbTotal.SetStatValue(cmb.ToHit);
            _cmbBase.SetStatValue(cmb.Base);
            _cmbStat.SetStatValue(cmb.Stat);
            _cmbSize.SetStatValue(cmb.Size);
            _cmbEpic.SetStatValue(cmb.Epic);
            _cmbMisc.SetStatValue(cmb.Misc);
            _cmbTemp.SetStatValue(cmb.Adjustment);
            _babTotal.SetStatValue(bab.ToHit);
            _babTemp.SetStatValue(bab.Adjustment);
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _meleeTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeTotal);
            _meleeBase = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeBaseAttack);
            _meleeStat = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeAbility);
            _meleeSize = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeSize);
            _meleeEpic = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeEpic);
            _meleeMisc = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeMisc);
            _meleeTemp = (ScaledTextView)root.FindViewById(Resource.Id.attacksMeleeTemp);
            _rangedTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedTotal);
            _rangedBase = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedBaseAttack);
            _rangedStat = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedAbility);
            _rangedSize = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedSize);
            _rangedEpic = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedEpic);
            _rangedMisc = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedMisc);
            _rangedTemp = (ScaledTextView)root.FindViewById(Resource.Id.attacksRangedTemp);
            _cmbTotal = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbTotal);
            _cmbBase = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbBaseAttack);
            _cmbStat = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbAbility);
            _cmbSize = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbSize);
            _cmbEpic = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbEpic);
            _cmbMisc = (ScaledTextView)root.FindViewById(Resource.Id.attacksCmbMisc);
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