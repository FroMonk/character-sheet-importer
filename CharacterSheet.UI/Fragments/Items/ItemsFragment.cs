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
using CharacterSheet.Pathfinder;
using CharacterSheet.UI.Fragments.Items.Segments;
using Environment = System.Environment;

namespace CharacterSheet.UI.Fragments.Items
{
    public class ItemsFragment : Fragment
    {
        private MainActivity _activity;
        private LayoutInflater _inflater;
        private ViewGroup _root;

        private bool _isPhone;

        private GenericListAdapter<Item> _adapter;
        private ScaledTextView _header;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = (MainActivity)Activity;
            _activity.CurrentTab = TabType.Items;
            _activity.UpdateOptionsMenu();

            _inflater = inflater;

            var selectedCharacter = _activity.Game.SelectedCharacter;

            _isPhone = _activity.FormatHelper.DeviceHelper.TypeOfDevice == DeviceType.Phone;

            _root = (ViewGroup)_inflater.Inflate(_isPhone ? Resource.Layout.items_fragment_min : Resource.Layout.items_fragment, null);
            
            var listView = (ListView)_root.FindViewById(Resource.Id.list);

            _adapter = new GenericListAdapter<Item>(_activity, selectedCharacter.Gear.Items);
            listView.Adapter = _adapter;
            listView.ItemClick += List_Click;

            _header = (ScaledTextView)_root.FindViewById(Resource.Id.carried_weight_header);
            SetHeaderText();

            _activity.CurrentItemsFragment = this;

            return _root;
        }

        private void List_Click(object sender, ListView.ItemClickEventArgs e)
        {
            var item = _adapter[e.Position];

            if (_isPhone)
            {
                new ItemDialog(_activity, _inflater, item, _adapter);
            }
            else
            {
                new ItemSummarySegment(_activity, _root, item, _adapter);
            }
        }
         
        public void Update()
        {
            SetHeaderText();

            _adapter.NotifyDataSetChanged();
        }   

        private void SetHeaderText()
        {
            var selectedCharacter = _activity.Game.SelectedCharacter;

            _header.SetText(string.Format("Carried: {0} lbs.", selectedCharacter.Gear.Carried));
        }
    }
}