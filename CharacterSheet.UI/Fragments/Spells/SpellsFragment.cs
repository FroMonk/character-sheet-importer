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
using CharacterSheet.UI.Activities;
using CharacterSheet.UI.Views;
using CharacterSheet.UI.Dialogs;
using Android.Graphics;
using static CharacterSheet.UI.Helpers.DeviceHelper;

namespace CharacterSheet.UI.Fragments.Spells
{
    public class SpellsFragment : Fragment
    {
        private const string SHOW_PREPARED_ONLY = "showPreparedOnly";

        private ExpandableSpellClassesListAdapter _spellsListAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public static SpellsFragment NewInstance(bool showPreparedOnly)
        {
            SpellsFragment fragment = new SpellsFragment();
            Bundle bundle = new Bundle(1);
            bundle.PutBoolean(SHOW_PREPARED_ONLY, showPreparedOnly);
            fragment.Arguments = bundle;

            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var activity = (MainActivity)Activity;
            var showPreparedOnly = Arguments.GetBoolean(SHOW_PREPARED_ONLY);
            activity.CurrentTab = showPreparedOnly ? TabType.PreparedSpells : TabType.Spells;
            activity.UpdateOptionsMenu();

            var selectedCharacter = activity.Game.SelectedCharacter;

            var isPhone = activity.FormatHelper.DeviceHelper.TypeOfDevice == DeviceType.Phone;

            var root = (ViewGroup)inflater.Inflate(isPhone ? Resource.Layout.spells_fragment_min : Resource.Layout.spells_fragment, null);

            var spellSummary = (LinearLayout)root.FindViewById(Resource.Id.spell_summary_layout);

            var spellsExpandableList = (ExpandableListView)root.FindViewById(Resource.Id.spells_list);

            _spellsListAdapter = new ExpandableSpellClassesListAdapter(activity, selectedCharacter.Spells.SpellClasses, spellSummary, showPreparedOnly);
            spellsExpandableList.SetAdapter(_spellsListAdapter);

            return root;
        }
    }
}