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
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public class MeleeSegment : BaseAttacksSegment
    {
        private bool _isEven;

        public ViewGroup Layout { get; protected set; }

        public MeleeSegment(Activity activity, LayoutInflater inflater, Weapon weapon, bool isEven) : base(activity)
        {
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(weapon, nameof(weapon));

            _isEven = isEven;

            var mainActivity = (MainActivity)activity;

            Layout = (ViewGroup)inflater.Inflate(mainActivity.FormatHelper.DeviceHelper.IsPhonePortrait ? Resource.Layout.attacks_fragment_min_melee : Resource.Layout.attacks_fragment_melee, null);

            SetupAttack(weapon.Melee.OneWeaponPrimaryHand, Resource.Id.attacks_attack_melee_one_weapon_primary_hand_bonus, Resource.Id.attacks_attack_melee_one_weapon_primary_hand_damage);
            SetupAttack(weapon.Melee.OneWeaponOffhand, Resource.Id.attacks_attack_melee_one_weapon_off_hand_bonus, Resource.Id.attacks_attack_melee_one_weapon_off_hand_damage);
            SetupAttack(weapon.Melee.TwoHands, Resource.Id.attacks_attack_melee_two_handed_bonus, Resource.Id.attacks_attack_melee_two_handed_damage);
            SetupAttack(weapon.Melee.TwoWeaponsPrimaryHandOtherHeavy, Resource.Id.attacks_attack_melee_two_weapons_primary_hand_other_heavy_bonus, Resource.Id.attacks_attack_melee_two_weapons_primary_hand_other_heavy_damage);
            SetupAttack(weapon.Melee.TwoWeaponsPrimaryHandOtherLight, Resource.Id.attacks_attack_melee_two_weapons_primary_hand_other_light_bonus, Resource.Id.attacks_attack_melee_two_weapons_primary_hand_other_light_damage);
            SetupAttack(weapon.Melee.TwoWeaponsOffhand, Resource.Id.attacks_attack_melee_two_weapons_off_hand_bonus, Resource.Id.attacks_attack_melee_two_weapons_off_hand_damage);
        }

        private void SetupAttack(Attack attack, int bonusViewId, int damageViewId)
        {
            var bonusView = (ScaledTextView)Layout.FindViewById(bonusViewId);
            var damageView = (ScaledTextView)Layout.FindViewById(damageViewId);

            if (attack != null)
            {
                bonusView.SetBackgroundColor(Color.ParseColor("#33b5e5"));
                bonusView.SetTextColor(Color.White);

                var toHit = Convert.AttackToString(attack.ToHit);

                bonusView.SetText(toHit);
                damageView.SetText(attack.DamageText);

                if (toHit.Length > 11)
                {
                    bonusView.SetScaledTextSize(11);
                }

                AssignAttackRollEvent(bonusView, attack);
            }
            else
            {
                if (_isEven)
                {
                    bonusView.SetBackgroundColor(Color.LightGray);
                }

                bonusView.SetText("-");
                damageView.SetText("-");
            }

            if (_isEven)
            {
                damageView.SetBackgroundColor(Color.LightGray);
            }
        }
    }
}