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
using CharacterSheet.Helpers;

using Convert = CharacterSheet.Helpers.Convert;
using Android.Graphics;
using CharacterSheet.UI.Dialogs;
using CharacterSheet.UI.Activities;
using CharacterSheet.Droid.Backend.Pathfinder;

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public class RangedFifthEditionSegment : BaseAttacksSegment
    {
        private bool _isEven;

        private readonly Dictionary<int, int> _headerViewMap = new Dictionary<int, int>
        {
            [0] = Resource.Id.attacks_attack_ranged_range_one_header,
            [1] = Resource.Id.attacks_attack_ranged_range_two_header
        };

        private readonly Dictionary<int, int> _bonusViewMap = new Dictionary<int, int>
        {
            [0] = Resource.Id.attacks_attack_ranged_range_one_bonus,
            [1] = Resource.Id.attacks_attack_ranged_range_two_bonus
        };

        private readonly Dictionary<int, int> _damageViewMap = new Dictionary<int, int>
        {
            [0] = Resource.Id.attacks_attack_ranged_range_one_damage,
            [1] = Resource.Id.attacks_attack_ranged_range_two_damage
        };

        public ViewGroup Layout { get; protected set; }

        public RangedFifthEditionSegment(Activity activity, LayoutInflater inflater, Weapon weapon, bool isEven) : base(activity)
        {
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(weapon, nameof(weapon));

            var mainActivity = (MainActivity)activity;
            
            _isEven = isEven;

            Layout = (ViewGroup)inflater.Inflate(mainActivity.FormatHelper.DeviceHelper.IsPhonePortrait ? Resource.Layout.attacks_fragment_5e_min_ranged : Resource.Layout.attacks_fragment_5e_ranged, null);

            var grid = (GridLayout)Layout.FindViewById(Resource.Id.attacks_attack_ranged);

            var attacks = weapon.Ranged.RangedAttacks;

            for (int i = 0; i < 2; i++)
            {
                SetupAttack(i, attacks[i]);
            }
                        
            weapon.Ranged.AmmunitionList.ForEach(x =>
            {
                AddAmmoToGrid(mainActivity, inflater, x, weapon, isEven, grid);
            });

            var addAmmo = (ScaledTextView)grid.FindViewById(Resource.Id.attacks_attack_ranged_ammunition_add);

            addAmmo.Click += delegate
            {
                var ammo = new Ammunition("Unknown ammunition");
                weapon.Ranged.AmmunitionList.Add(ammo);
                var segment = AddAmmoToGrid(mainActivity, inflater, ammo, weapon, isEven, grid);
                grid.Invalidate();
                new AmmunitionDialog(activity, ammo,
                    updateAction: () =>
                    {
                        segment.SetText();
                    },
                    removeAction: () =>
                    {
                        weapon.Ranged.AmmunitionList.Remove(ammo);
                        grid.RemoveView(segment.AmmoNameView);
                        grid.RemoveView(segment.AmmoView);
                        grid.Invalidate();
                    },
                    shouldRemoveOnCancel: true);
            };
        }

        private RangedAmmunitionSegment AddAmmoToGrid(MainActivity activity, LayoutInflater inflater, Ammunition ammo, Weapon weapon, bool isEven, GridLayout grid)
        {
            var margin = _activity.FormatHelper.Scale(0.0025);
            var height = _activity.FormatHelper.Scale(0.03);

            var isPhonePortrait = activity.FormatHelper.DeviceHelper.IsPhonePortrait;

            var ammoNameLayoutParams = new GridLayout.LayoutParams();
            ammoNameLayoutParams.Height = height;
            ammoNameLayoutParams.BottomMargin = margin;

            if (isPhonePortrait)
            {
                ammoNameLayoutParams.ColumnSpec = GridLayout.InvokeSpec(0, 3);
                ammoNameLayoutParams.Width = _activity.FormatHelper.Scale(0.4025d);
                ammoNameLayoutParams.LeftMargin = margin;
            }
            else
            {
                ammoNameLayoutParams.SetGravity(GravityFlags.CenterVertical | GravityFlags.Right);
            }

            var ammoLayoutParams = new GridLayout.LayoutParams();
            ammoLayoutParams.ColumnSpec = isPhonePortrait ? GridLayout.InvokeSpec(0, 3) : GridLayout.InvokeSpec(1, 2);
            ammoLayoutParams.Height = height;
            ammoLayoutParams.Width = _activity.FormatHelper.Scale(isPhonePortrait ? 0.405d : 0.6225d);
            ammoLayoutParams.BottomMargin = margin;

            var ammoSegment = new RangedAmmunitionSegment(activity, inflater, ammo, weapon, isEven, grid);

            grid.AddView(ammoSegment.AmmoNameView, ammoNameLayoutParams);
            grid.AddView(ammoSegment.AmmoView, ammoLayoutParams);

            return ammoSegment;
        }

        private void SetupAttack(int count, RangedAttack attack)
        {
            var headerView = (ScaledTextView)Layout.FindViewById(_headerViewMap[count]);
            var bonusView = (ScaledTextView)Layout.FindViewById(_bonusViewMap[count]);
            var damageView = (ScaledTextView)Layout.FindViewById(_damageViewMap[count]);

            var toHit = Convert.AttackToString(attack.ToHit);

            headerView.SetText(attack.Advantage == Advantage.Disadvantage ? string.Format("{0} (Disadvantage)", attack.Range) : attack.Range);
            bonusView.SetText(toHit);
            damageView.SetText(attack.DamageText);

            if (toHit.Length > 11)
            {
                bonusView.SetScaledTextSize(11);
            }

            if (_isEven)
            {
                damageView.SetBackgroundColor(Color.LightGray);
            }

            AssignAttackRollEvent(bonusView, attack, isFifthEdition: true);
        }
    }
}