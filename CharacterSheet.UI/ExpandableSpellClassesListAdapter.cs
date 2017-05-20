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
using Java.Lang;
using CharacterSheet.Pathfinder;
using Android.Graphics.Drawables;
using CharacterSheet.UI.Views;
using CharacterSheet.UI.Dialogs;
using CharacterSheet.UI.Fragments.Spells.Segments;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI
{
    public class ExpandableSpellClassesListAdapter : BaseExpandableListAdapter
    {
        private MainActivity _activity;
        private Dictionary<string, SpellClass> _spellsList;
        private List<string> _keys;
        private LinearLayout _spellsSummaryView;
        private bool _isShowingSpellDialog;
        private bool _showPreparedOnly;

        public ExpandableSpellClassesListAdapter(Activity activity, Dictionary<string, SpellClass> spellsList, LinearLayout spellsSummaryView, bool showPreparedOnly) : base()
        {
            _activity = (MainActivity)activity;
            _spellsList = spellsList;
            _spellsSummaryView = spellsSummaryView;
            _showPreparedOnly = showPreparedOnly;

            _activity.CurrentSpellsLevelListAdapters = new List<ExpandableSpellsLevelListAdapter>();            

            _keys = _spellsList.Keys.ToList();
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = _activity.LayoutInflater.Inflate(Resource.Layout.spell_classes_list_parent, null);
            }

            header.FindViewById<TextView>(Resource.Id.DataHeader).Text = _keys[groupPosition];

            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = _activity.LayoutInflater.Inflate(Resource.Layout.spell_classes_list_child, null);
            }
            
            var spellsLevelListView = row.FindViewById<NestedExpandableListView>(Resource.Id.spells_level_list);
            var adapter = new ExpandableSpellsLevelListAdapter(_activity, _spellsList[_keys[groupPosition]].PopulatedLevels[childPosition], _showPreparedOnly);
            spellsLevelListView.SetAdapter(adapter);

            _activity.CurrentSpellsLevelListAdapters.Add(adapter);

            spellsLevelListView.ChildClick += SpellsLevel_ChildClick;

            return row;
        }

        private void SpellsLevel_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            if (!_isShowingSpellDialog)
            {
                var expListView = (NestedExpandableListView)sender;
                var adapter = (ExpandableSpellsLevelListAdapter)expListView.ExpandableListAdapter;

                var spell = adapter.GetChild(e.ChildPosition);

                if (_spellsSummaryView == null)
                {
                    var spellDialog = new SpellDialog(_activity, _activity.LayoutInflater, spell, adapter);
                    _isShowingSpellDialog = true;

                    spellDialog.SetOnDismissListener(new OnDismissListener(() =>
                    {
                        _isShowingSpellDialog = false;
                    }));
                }
                else
                {
                    new SpellSummarySegment(_activity, _spellsSummaryView, spell, adapter);
                }
            }
        }        

        public override int GetChildrenCount(int groupPosition)
        {
            return _spellsList[_keys[groupPosition]].PopulatedLevels.Count;
        }

        public override int GroupCount
        {
            get
            {
                return _keys.Count;
            }
        }

        #region implemented abstract members of BaseExpandableListAdapter

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        #endregion
    }
}