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
    public class AcSegment : BaseCharacterSegement, ISegment
    {
        private ScaledTextView _total;
        private ScaledTextView _touch;
        private ScaledTextView _flat;
        private ScaledTextView _base;
        private ScaledTextView _armor;
        private ScaledTextView _shield;
        private ScaledTextView _stat;
        private ScaledTextView _size;
        private ScaledTextView _natural;
        private ScaledTextView _dodge;
        private ScaledTextView _deflection;
        private ScaledTextView _misc;
        private ScaledTextView _missChance;
        private ScaledTextView _arcaneFailure;
        private ScaledTextView _armorCheck;
        private ScaledTextView _maxDex;
        private ScaledTextView _spellResist;
        private ScaledTextView _temp;

        public AcSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _total = (ScaledTextView)root.FindViewById(Resource.Id.acTotal);
            _touch = (ScaledTextView)root.FindViewById(Resource.Id.acTouch);
            _flat = (ScaledTextView)root.FindViewById(Resource.Id.acFlat);
            _base = (ScaledTextView)root.FindViewById(Resource.Id.acBase);
            _armor = (ScaledTextView)root.FindViewById(Resource.Id.acArmorBonus);
            _shield = (ScaledTextView)root.FindViewById(Resource.Id.acShieldBonus);
            _stat = (ScaledTextView)root.FindViewById(Resource.Id.acStatBonus);
            _size = (ScaledTextView)root.FindViewById(Resource.Id.acSizeBonus);
            _natural = (ScaledTextView)root.FindViewById(Resource.Id.acNaturalBonus);
            _dodge = (ScaledTextView)root.FindViewById(Resource.Id.acDodgeBonus);
            _deflection = (ScaledTextView)root.FindViewById(Resource.Id.acDeflectionBonus);
            _misc = (ScaledTextView)root.FindViewById(Resource.Id.acMiscBonus);
            _missChance = (ScaledTextView)root.FindViewById(Resource.Id.acMissChance);
            _arcaneFailure = (ScaledTextView)root.FindViewById(Resource.Id.acArcaneFailure);
            _armorCheck = (ScaledTextView)root.FindViewById(Resource.Id.acArmorCheck);
            _maxDex = (ScaledTextView)root.FindViewById(Resource.Id.acMaxDex);
            _spellResist = (ScaledTextView)root.FindViewById(Resource.Id.acSpellResist);
            _temp = (ScaledTextView)root.FindViewById(Resource.Id.acTemp);
        }

        public override void AssignValues()
        {
            var ac = _selectedCharacter.AC;

            _total.SetStatValue(ac.Total);
            _touch.SetStatValue(ac.Touch);
            _flat.SetStatValue(ac.Flat);
            _base.SetStatValue(ac.Base);
            _armor.SetStatValue(ac.ArmorBonus);
            _shield.SetStatValue(ac.ShieldBonus);
            _stat.SetStatValue(ac.StatBonus);
            _size.SetStatValue((int)_selectedCharacter.Bio.Size);
            _natural.SetStatValue(ac.NaturalArmor);
            _dodge.SetStatValue(ac.DodgeBonus);
            _deflection.SetStatValue(ac.DeflectionBonus);
            _misc.SetStatValue(ac.MiscBonus);
            _missChance.SetText(ac.MissChance);
            _arcaneFailure.SetStatValue(ac.ArcaneFailure);
            _armorCheck.SetStatValue(ac.ArmorCheck);
            _maxDex.SetText(ac.MaxDex == 99999 ? "-" : ac.MaxDex.ToString());
            _spellResist.SetStatValue(ac.SpellResist);
            _temp.SetStatValue(ac.Adjustment);
        }

        protected override void AssignEvents()
        { 
            AssignAdjustmentEvent(_temp, _selectedCharacter.AC, () => AssignValues());
        }
    }
}