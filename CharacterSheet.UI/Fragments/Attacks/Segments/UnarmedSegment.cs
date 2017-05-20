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

using Convert = CharacterSheet.Helpers.Convert;
using CharacterSheet.UI.Activities;
using CharacterSheet.Droid.Backend.Pathfinder;

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public class UnarmedSegment : BaseAttacksSegment
    {
        public ViewGroup Layout { get; protected set; }

        public UnarmedSegment(Activity activity, LayoutInflater inflater, Weapon weapon, bool isEven) : base(activity)
        {
            var mainActivity = (MainActivity)activity;
            var isPhonePortait = mainActivity.FormatHelper.DeviceHelper.IsPhonePortrait;

            Layout = (ViewGroup)inflater.Inflate(isPhonePortait ? Resource.Layout.attacks_fragment_min_unarmed : Resource.Layout.attacks_fragment_unarmed, null);


            var nameView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_unarmed_name);
            var bonusView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_unarmed_bonus);
            var damageView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_unarmed_damage);
            var criticalView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_unarmed_critical);

            var toHit = Convert.AttackToString(weapon.Attack.ToHit);

            nameView.SetText(weapon.Name);
            bonusView.SetText(toHit);
            damageView.SetText(weapon.Attack.DamageText);
            criticalView.SetText(weapon.Critical.Text);

            if (toHit.Length > 11)
            {
                bonusView.SetScaledTextSize(11);
            }

            if (isEven)
            {
                damageView.SetBackgroundColor(Color.LightGray);
                criticalView.SetBackgroundColor(Color.LightGray);
            }

            if (!isPhonePortait)
            {
                var reachView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_unarmed_reach);
                reachView.SetText(weapon.Reach);
                
                if (isEven)
                {
                    reachView.SetBackgroundColor(Color.LightGray);
                }           
            }

            AssignAttackRollEvent(bonusView, weapon.Attack, isFifthEdition: mainActivity.Game.SelectedCharacter.GameSystem == GameSystem.D20FifthEdition);
        }
    }
}