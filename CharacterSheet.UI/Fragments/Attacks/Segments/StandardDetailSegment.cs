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
using CharacterSheet.Helpers;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public class StandardDetailSegment
    {
        public ViewGroup Layout { get; protected set; }

        public StandardDetailSegment(Activity activity, LayoutInflater inflater, Weapon weapon, bool isEven)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(weapon, nameof(weapon));

            var mainActivity = (MainActivity)activity;

            Layout = (ViewGroup)inflater.Inflate(mainActivity.FormatHelper.DeviceHelper.IsPhonePortrait ? Resource.Layout.attacks_fragment_min_standard_detail : Resource.Layout.attacks_fragment_standard_detail, null);
            
            var nameView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_name);
            var criticalView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_critical);

            nameView.SetText(weapon.Name);
            criticalView.SetText(weapon.Critical.Text);

            if (isEven)
            {
                nameView.SetBackgroundColor(Color.LightGray);
                criticalView.SetBackgroundColor(Color.LightGray);
            }

            if (!mainActivity.FormatHelper.DeviceHelper.IsPhonePortrait)
            {
                var handView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_hand);
                var typeView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_type);
                var sizeView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_size);
                var reachView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_reach);

                handView.SetText(weapon.Hand);
                typeView.SetText(weapon.Type);
                sizeView.SetText(weapon.Size);
                reachView.SetText(weapon.Reach);

                if (isEven)
                {
                    handView.SetBackgroundColor(Color.LightGray);
                    typeView.SetBackgroundColor(Color.LightGray);
                    sizeView.SetBackgroundColor(Color.LightGray);
                    reachView.SetBackgroundColor(Color.LightGray);
                }
            }            
        }
    }
}