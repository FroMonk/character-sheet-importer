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
using CharacterSheet.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CharacterSheet.Pathfinder;
using CharacterSheet.UI.Activities;
using CharacterSheet.UI.Views;
using Android.Text;
using System.Timers;
using System.ComponentModel;
using CharacterSheet.UI.Dialogs;
using CharacterSheet.Droid.Backend.Pathfinder;
using Android.Graphics;
using Android.Content.PM;
using CharacterSheet.UI.Fragments.Character.Segments;
using Fragment = Android.Support.V4.App.Fragment;
using CharacterSheet.UI.Helpers;

namespace CharacterSheet.UI.Fragments.Character
{
    public class CharacterFragment : Fragment
    {
        private const string CHARACTER_SCREEN_MODEL = "CharacterScreenModel";
        private const int OVERLAY_TIMEOUT = 3000;
        private const int ADJUSTMENT_UPDATE_VALUE_TIMEOUT = 250;
        
        public ISegment Hp { get; protected set; }
        public ISegment Ac { get; protected set; }
        public AbilityScoresSegment AbilityScores { get; protected set; }
        public BioSegment Bio { get; protected set; }
        public InitiativeSegment Initiative { get; protected set; }
        public ISegment SavingThrows { get; protected set; }
        public SavingThrowsModifiersSegment SavingThrowsModifiers { get; protected set; }
        public ISegment Attacks { get; protected set; }
        public CombatManeuversSegment CombatManeuvers { get; protected set; }
        public CombatManeuversModifiersSegment CombatManeuversModifiers { get; protected set; }
        public LanguagesSegment Languages { get; protected set; }
        public WeaponProficienciesSegment WeaponProficiencies { get; protected set; }

        public CharacterFragment()
        {
        }
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root;
            
            var activity = (MainActivity)this.Activity;
            activity.CurrentTab = TabType.Character;
            activity.UpdateOptionsMenu();

            var selectedCharacter = activity.Game.SelectedCharacter;

            var isFifthEdition = selectedCharacter.GameSystem == GameSystem.D20FifthEdition;

            if (activity.FormatHelper.DeviceHelper.IsPhonePortrait)
            {
                root = (ViewGroup)inflater.Inflate(isFifthEdition ? Resource.Layout.character_fragment_5e_min : Resource.Layout.character_fragment_min, null);
                Hp = new MinHpSegment(activity, root, selectedCharacter, this);
                Ac = new MinAcSegment(activity, root, selectedCharacter, this);
                Attacks = new MinAttacksSegment(activity, root, selectedCharacter, this);

                if (isFifthEdition)
                {
                    SavingThrows = new FifthEditionMinSavingThrowsSegment(activity, root, selectedCharacter, this);
                }
                else
                {
                    SavingThrows = new MinSavingThrowsSegment(activity, root, selectedCharacter, this);
                }
            }
            else
            {
                root = (ViewGroup)inflater.Inflate(isFifthEdition ? Resource.Layout.character_fragment_5e : Resource.Layout.character_fragment, null);
                Hp = new HpSegment(activity, root, selectedCharacter, this);
                Ac = new AcSegment(activity, root, selectedCharacter, this);
                Bio = new BioSegment(activity, root, selectedCharacter, this);
                SavingThrowsModifiers = new SavingThrowsModifiersSegment(activity, root, selectedCharacter, this);
                Attacks = new AttacksSegment(activity, root, selectedCharacter, this);
                CombatManeuversModifiers = new CombatManeuversModifiersSegment(activity, root, selectedCharacter, this);

                if (isFifthEdition)
                {
                    SavingThrows = new FifthEditionSavingThrowsSegment(activity, root, selectedCharacter, this);
                }
                else
                {
                    SavingThrows = new SavingThrowsSegment(activity, root, selectedCharacter, this);
                }
            }
            AbilityScores = new AbilityScoresSegment(activity, root, selectedCharacter, this);
            Initiative = new InitiativeSegment(activity, root, selectedCharacter, this);
            CombatManeuvers = new CombatManeuversSegment(activity, root, selectedCharacter, inflater, this);
            Languages = new LanguagesSegment(activity, root, selectedCharacter, this);
            WeaponProficiencies = new WeaponProficienciesSegment(activity, root, selectedCharacter, this);
                        
            var versionInfo = (ScaledTextView)root.FindViewById(Resource.Id.version);
            versionInfo.Text = Activity.BaseContext.PackageManager.GetPackageInfo(Activity.PackageName, 0).VersionName;
            
            return root;
        }
    }
}