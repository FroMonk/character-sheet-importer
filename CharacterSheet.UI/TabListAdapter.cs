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
using CharacterSheet.UI.Fragments;

namespace CharacterSheet.UI
{
    public class TabListAdapter : BaseAdapter<TabType>
    {
        Dictionary<string, TabType> _tabTypeMap;
        List<string> _tabs;

        Activity _context;

        public TabListAdapter(Activity context, Dictionary<string, TabType> tabTypeMap) : base()
        {
            _context = context;
            _tabTypeMap = tabTypeMap;
            _tabs = _tabTypeMap.Keys.ToList();
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override TabType this[int position]
        {
            get { return _tabTypeMap[_tabs[position]]; }
        }

        public string GetKey(int position)
        {
            return _tabs[position];
        }

        public override int Count
        {
            get { return _tabs.Count(); }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _tabs[position];
            return view;
        }
    }
}