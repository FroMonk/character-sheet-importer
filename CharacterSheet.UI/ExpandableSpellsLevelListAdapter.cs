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
using Environment = System.Environment;

namespace CharacterSheet.UI
{
    public class ExpandableSpellsLevelListAdapter : BaseExpandableListAdapter
    {
        private Activity _context;
        private SpellClassLevel _spellClassLevel;
        private bool _showPreparedOnly;

        public ExpandableSpellsLevelListAdapter(Activity context, SpellClassLevel spellClassLevel, bool showPreparedOnly) : base()
		{
            _context = context;
            _spellClassLevel = spellClassLevel;
            _showPreparedOnly = showPreparedOnly;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = _context.LayoutInflater.Inflate(Resource.Layout.spells_level_list_parent, null);
            }
            
            header.FindViewById<TextView>(Resource.Id.DataHeader).Text = string.Format("Level {0}{1}Max: {2} Left: {3}", _spellClassLevel.Level, Environment.NewLine,_spellClassLevel.Max, _spellClassLevel.Remaining);
            
            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = _context.LayoutInflater.Inflate(Resource.Layout.spells_level_list_child, null);
            }

            var spell = _showPreparedOnly ? _spellClassLevel.PreparedSpells[childPosition] : _spellClassLevel.Spells[childPosition];

            row.FindViewById<TextView>(Resource.Id.DataId).Text = string.Format("{0}{1}", spell.Name, spell.NumberPrepared <= 0 ? string.Empty : string.Format(" ({0}/{1})", spell.CastsLeft, spell.NumberPrepared));

            return row;
        }

        public Spell GetChild(int childPosition)
        {
            return _showPreparedOnly ? _spellClassLevel.PreparedSpells[childPosition] : _spellClassLevel.Spells[childPosition];
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _showPreparedOnly ? _spellClassLevel.PreparedSpells.Count : _spellClassLevel.Spells.Count;
        }

        public override int GroupCount
        {
            get
            {
                return 1;
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