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
using CharacterSheet.UI.Dialogs;
using Android.Graphics;
using CharacterSheet.UI.Helpers;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public class CombatManeuversSegment : BaseCharacterSegement
    {
        private ViewGroup _root;

        private List<ScaledTextView> _cmbList = new List<ScaledTextView>();
        private List<ScaledTextView> _cmdList = new List<ScaledTextView>();

        private Dictionary<string, Tuple<ScaledTextView, ScaledTextView>> _viewMap = new Dictionary<string, Tuple<ScaledTextView, ScaledTextView>>();

        private ScaledImageButton _roll;

        public CombatManeuversSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, LayoutInflater inflater, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
            Check.ForNullArgument(inflater, "inflater");

            _root = root;       

            if (_selectedCharacter.CombatManeuvers.Maneuvers.Any())
            {
                SetupCombatManeuvers(inflater);
            }
            else
            {
                var combatManeuversLayout = root.FindViewById(new DeviceHelper(activity).IsPhonePortrait ? Resource.Id.combat_maneuvers_layout : Resource.Id.combat_maneuvers_and_modifiers_layout);
                combatManeuversLayout.Visibility = ViewStates.Gone;
            }
        }

        private void SetupCombatManeuvers(LayoutInflater inflater)
        {
            var cmbCmdLayout = (LinearLayout)_root.FindViewById(Resource.Id.cmbCmdLayout);

            bool eventSetup = false;

            EventHandler currentEventHandler = null;

            foreach (var combatManeuver in _selectedCharacter.CombatManeuvers.Maneuvers)
            {
                ViewGroup combatManeuverLayout = (ViewGroup)inflater.Inflate(Resource.Layout.character_fragment_combatmaneuver, null);

                var combatManeuverTitle = (ScaledTextView)combatManeuverLayout.FindViewById(Resource.Id.combatManeuverTitle);
                var cmbView = (ScaledTextView)combatManeuverLayout.FindViewById(Resource.Id.cmbValue);
                var cmdView = (ScaledTextView)combatManeuverLayout.FindViewById(Resource.Id.cmdValue);

                _viewMap.Add(combatManeuver.Key, new Tuple<ScaledTextView, ScaledTextView>(cmbView, cmdView));

                combatManeuverTitle.SetText(combatManeuver.Key);
                cmbView.SetStatValue(combatManeuver.Value.Cmb);

                cmbView.Click += delegate
                {
                    if (currentEventHandler != null)
                    {
                        _roll.Click -= currentEventHandler;
                    }

                    foreach (var maneuverView in _viewMap)
                    {
                        var cmb = maneuverView.Value.Item1;

                        cmb.SetBackgroundColor(Color.ParseColor("#808080"));
                        cmb.SetTextColor(Color.Black);
                    }

                    cmbView.SetBackgroundColor(Color.ParseColor("#33b5e5"));
                    cmbView.SetTextColor(Color.White);

                    currentEventHandler = delegate
                    {
                        new CheckDialog(_activity, combatManeuver.Value);
                    };

                    _roll.Click += currentEventHandler;

                    _roll.ContentDescription = string.Format("Roll {0} Attack", combatManeuver.Key);
                };

                cmdView.SetStatValue(combatManeuver.Value.Cmd);

                combatManeuverTitle.ContentDescription = string.Format("{0} Combat Maneuver label", combatManeuver.Key);
                cmbView.ContentDescription = string.Format("{0} Combat Maneuver Bonus: {1}", combatManeuver.Key, cmbView.Text);
                cmdView.ContentDescription = string.Format("{0} Combat Maneuver Defense: {1}", combatManeuver.Key, cmdView.Text);

                cmbCmdLayout.AddView(combatManeuverLayout);


                if (!eventSetup)
                {
                    eventSetup = !eventSetup;
                    cmbView.PerformClick();
                }
            }
        }

        public override void AssignValues()
        {
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _roll = (ScaledImageButton)root.FindViewById(Resource.Id.cmbRoll);
        }

        public void UpdateValues()
        {
            foreach (var maneuver in _selectedCharacter.CombatManeuvers.Maneuvers)
            {
                var key = maneuver.Key;

                var views = _viewMap[key];
                var cmbView = views.Item1;
                var cmdView = views.Item2;

                var cmbValue = maneuver.Value.Cmb;
                var cmdValue = maneuver.Value.Cmd;

                cmbView.SetStatValue(cmbValue);
                cmbView.ContentDescription = string.Format("{0} Combat Maneuver Bonus: {1}", key, cmbView.Text);

                cmdView.SetStatValue(cmdValue);
                cmdView.ContentDescription = string.Format("{0} Combat Maneuver Defense: {1}", key, cmdView.Text);
            }
        }
    }
}