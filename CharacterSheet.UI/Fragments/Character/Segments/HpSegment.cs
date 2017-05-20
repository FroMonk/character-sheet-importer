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
using CharacterSheet.Helpers;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public class HpSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _max;
        private ScaledTextView _current;
        private ScaledTextView _subdual;
        private ScaledTextView _damageReduction;
        private ScaledTextView _speed;

        public HpSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _max = (ScaledTextView)root.FindViewById(Resource.Id.hpMax);
            _current = (ScaledTextView)root.FindViewById(Resource.Id.hpWoundsCurrent);
            _subdual = (ScaledTextView)root.FindViewById(Resource.Id.hpSubdualDamage);
            _damageReduction = (ScaledTextView)root.FindViewById(Resource.Id.hpDamageReduction);
            _speed = (ScaledTextView)root.FindViewById(Resource.Id.hpSpeed);
        }

        public override void AssignValues()
        {
            var health = _selectedCharacter.Health;

            _max.SetStatValue(health.MaxHp);
            _current.SetStatValue(health.CurrentHp);
            _subdual.SetStatValue(_selectedCharacter.SubdualDamage.Adjustment);
            _damageReduction.SetText(health.DamageReduction);

            _speed.SetText(_selectedCharacter.Speed.GetSpeed());
        }

        protected override void AssignEvents()
        {
            Action action = () => AssignValues();

            AssignAdjustmentEvent(_current, _selectedCharacter.Health, action);
            AssignAdjustmentEvent(_subdual, _selectedCharacter.SubdualDamage, action);
        }
    }
}