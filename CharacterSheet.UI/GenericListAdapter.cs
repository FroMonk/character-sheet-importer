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
    public class GenericListAdapter<T> : BaseAdapter<T> where T : IHasDescription
    {
        List<T> _list;

        Activity _context;

        public GenericListAdapter(Activity context, List<T> list) : base()
        {
            _context = context;
            _list = list;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override T this[int position]
        {
            get { return _list[position]; }
        }

        public void Remove(T item)
        {
            _list.Remove(item);
            this.NotifyDataSetChanged();
        }

        public override int Count
        {
            get { return _list.Count(); }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _list[position].Name;
            return view;
        }
    }
}