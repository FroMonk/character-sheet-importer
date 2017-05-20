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

namespace CharacterSheet.UI.Fragments.Attacks.Segments
{
    public class RangedAmmunitionSegment
    {
        private bool _isEven;
        private Ammunition _ammunition;
        
        private readonly string _unusedAmmoText;
        private readonly string _unusedBlockAmmoText;

        public ScaledTextView AmmoNameView { get; protected set; }
        public ScaledTextView AmmoView { get; protected set; }

        public RangedAmmunitionSegment(Activity activity, LayoutInflater inflater, Ammunition ammunition, Weapon weapon, bool isEven, GridLayout grid)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(ammunition, nameof(ammunition));
            Check.ForNullArgument(weapon, nameof(weapon));
            Check.ForNullArgument(grid, nameof(grid));

            _isEven = isEven;
            _ammunition = ammunition;

            var isPhonePortrait = ((MainActivity)activity).FormatHelper.DeviceHelper.IsPhonePortrait;

            _unusedAmmoText = activity.GetString(Resource.String.attacks_attack_ranged_ammunition_unused);
            _unusedBlockAmmoText = activity.GetString(Resource.String.attacks_attack_ranged_ammunition_unused_block);

            AmmoNameView = (ScaledTextView)inflater.Inflate(isPhonePortrait ? Resource.Layout.attacks_fragment_min_ranged_ammunition_name : Resource.Layout.attacks_fragment_ranged_ammunition_name, null);
            AmmoView = (ScaledTextView)inflater.Inflate(isPhonePortrait ? Resource.Layout.attacks_fragment_min_ranged_ammunition : Resource.Layout.attacks_fragment_ranged_ammunition, null);            
            
            SetText();

            AmmoView.Click += delegate
            { 
                if (_ammunition.Remaining > 0)
                {
                    ammunition.Remaining -= 1;
                    SetText();
                }
            };

            if (_isEven)
            {
                AmmoView.SetBackgroundColor(Color.LightGray);
            }


            AmmoNameView.Click += delegate
            {
                new AmmunitionDialog(activity, _ammunition,
                    updateAction: () =>
                    {
                        SetText();
                    },
                    removeAction: () =>
                    {
                        weapon.Ranged.AmmunitionList.Remove(ammunition);
                        grid.RemoveView(AmmoNameView);
                        grid.RemoveView(AmmoView);
                        grid.Invalidate();
                    });
            };
        }

        public void SetText()
        {
            AmmoNameView.SetText(_ammunition.Name);

            var ammoText = string.Empty;
            
            var remaining = _ammunition.Remaining;

            var tens = remaining / 10;
            var units = remaining % 10;

            for (int i = 0; i < tens; i++)
            {
                ammoText += _unusedBlockAmmoText;
            }

            for (int i = 0; i < units; i++)
            {
                ammoText += _unusedAmmoText;
            }

            AmmoView.SetText(ammoText);
        }
    }
}