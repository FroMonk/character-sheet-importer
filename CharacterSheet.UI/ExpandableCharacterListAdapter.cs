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

namespace CharacterSheet.UI
{
    public class ExpandableCharacterListAdapter : BaseExpandableListAdapter
    {
        private Activity _context;
        private List<CharacterStats> _characterList { get; set; }
        private Drawable _groupIcon;
        private Drawable _soloIcon;
        private Drawable _childIcon;

        public ExpandableCharacterListAdapter(Activity context, List<CharacterStats> characterList) : base()
		{
            _context = context;
            _characterList = characterList;

            _groupIcon = _context.GetDrawable(Resource.Drawable.ic_group_black);
            _soloIcon = _context.GetDrawable(Resource.Drawable.ic_person_black);
            _childIcon = _context.GetDrawable(Resource.Drawable.ic_person_outline_black);
        }

        private void AddIcon(TextView textview, Drawable drawable)
        {
            textview.SetCompoundDrawablesWithIntrinsicBounds(drawable, null, null, null);
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = _context.LayoutInflater.Inflate(Resource.Layout.characterlist_parent, null);
            }

            var headerText = header.FindViewById<TextView>(Resource.Id.DataHeader);
            headerText.Text = string.Concat(" ", _characterList[groupPosition].Name);

            AddIcon(headerText, GetChildrenCount(groupPosition) == 0 ? _soloIcon : _groupIcon);

            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = _context.LayoutInflater.Inflate(Resource.Layout.characterlist_child, null);
            }

            var rowText = row.FindViewById<TextView>(Resource.Id.DataId);
            rowText.Text = string.Concat(" ", _characterList[groupPosition].Minions[childPosition].Name);

            AddIcon(rowText, _childIcon);

            return row;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _characterList[groupPosition].Minions.Count;
        }

        public override int GroupCount
        {
            get
            {
                return _characterList.Count;
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