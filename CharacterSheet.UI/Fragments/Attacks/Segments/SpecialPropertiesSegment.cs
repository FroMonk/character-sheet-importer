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
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public class SpecialPropertiesSegment
    {
        public ViewGroup Layout { get; protected set; }

        public SpecialPropertiesSegment(Activity activity, LayoutInflater inflater, Weapon weapon, bool isEven)
        {
            Layout = (ViewGroup)inflater.Inflate(Resource.Layout.attacks_fragment_special_properties, null);

            var mainActivity = (MainActivity)activity;

            var specialPropertiesView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_special_properties);

            specialPropertiesView.SetText(weapon.Description);

            if (isEven)
            {
                specialPropertiesView.SetBackgroundColor(Color.LightGray);
            }

            if (mainActivity.FormatHelper.DeviceHelper.IsPhonePortrait)
            {
                specialPropertiesView.Visibility = ViewStates.Gone;

                var specialPropertiesLabelView = (ScaledTextView)Layout.FindViewById(Resource.Id.attacks_attack_special_properties_label);

                specialPropertiesLabelView.SetTextColor(Color.ParseColor("#33b5e5"));

                specialPropertiesLabelView.Click += delegate
                {
                    specialPropertiesView.Visibility = specialPropertiesView.Visibility == ViewStates.Gone ? ViewStates.Visible : ViewStates.Gone;
                };
            }
        }
    }
}