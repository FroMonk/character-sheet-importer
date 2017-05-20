using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using LayoutParams = Android.Views.ViewGroup.LayoutParams;
using CharacterSheet.UI.Activities;
using CharacterSheet.UI.Views;
using CharacterSheet.UI.Dialogs;
using Android.Graphics;
using CharacterSheet.UI.Fragments.Attacks.Segments;
using CharacterSheet.UI.Helpers;
using CharacterSheet.Droid.Backend.Pathfinder;

namespace CharacterSheet.UI.Fragments.Attacks
{
    public class AttacksFragment : Fragment
    {
        private MainActivity _activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = (MainActivity)Activity;
            _activity.CurrentTab = TabType.Attacks;
            _activity.UpdateOptionsMenu();

            var isPhonePortrait = _activity.FormatHelper.DeviceHelper.IsPhonePortrait;

            var root = (ViewGroup)inflater.Inflate(Resource.Layout.attacks_fragment, null);

            var attacksLayout = (LinearLayout)root.FindViewById(Resource.Id.attacks_layout);

            var selectedCharacter = _activity.Game.SelectedCharacter;
            var isFifthEdition = selectedCharacter.GameSystem == GameSystem.D20FifthEdition;

            var weapons = selectedCharacter.Equipment.Weapons;

            var viewHelper = new ViewHelper(_activity);

            for (int i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                var isEven = (i + 1) % 2 == 0;

                if (weapon.IsUnarmed || weapon.IsNonStandardMelee)
                {
                    attacksLayout.AddView(new UnarmedSegment(_activity, inflater, weapon, isEven).Layout);
                }
                else
                {
                    if (!isFifthEdition || isFifthEdition && weapon.Melee == null)
                    {
                        attacksLayout.AddView(new StandardDetailSegment(_activity, inflater, weapon, isEven).Layout);
                    }
                    
                    if (weapon.Melee != null)
                    {
                        if (isFifthEdition)
                        {
                            attacksLayout.AddView(new UnarmedSegment(_activity, inflater, weapon, isEven).Layout);
                        }
                        else
                        {
                            attacksLayout.AddView(new MeleeSegment(_activity, inflater, weapon, isEven).Layout);
                        }
                    }

                    if (weapon.Melee != null && weapon.Ranged != null)
                    {
                        var spacer = CreateSpacer<View>();
                        spacer.SetBackgroundColor(Color.Black);

                        attacksLayout.AddView(spacer);
                    }

                    if (weapon.Ranged != null)
                    {
                        if (isFifthEdition)
                        {
                            attacksLayout.AddView(new RangedFifthEditionSegment(_activity, inflater, weapon, isEven).Layout);
                        }
                        else
                        {
                            attacksLayout.AddView(new RangedSegment(_activity, inflater, weapon, isEven).Layout);
                        }
                    }
                }                

                if (!string.IsNullOrWhiteSpace(weapon.Description))
                {
                    attacksLayout.AddView(new SpecialPropertiesSegment(_activity, inflater, weapon, isEven).Layout);
                }

                if (i != weapons.Count - 1)
                {
                    attacksLayout.AddView(CreateSpacer<Space>());
                }
            }            

            return root;
        }

        private T CreateSpacer<T>() where T : View
        {

            var view = (T)Activator.CreateInstance(typeof(T), _activity);
            
            view.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, _activity.FormatHelper.Scale(0.015));

            return view;
        }
    }
}