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
using CharacterSheet.Droid.Backend.Pathfinder.Notes;

namespace CharacterSheet.UI.Fragments.Notes
{
    public class NotesFragment : Fragment
    {
        private MainActivity _activity;
        private LayoutInflater _inflater;
        
        private ScaledTextView _detailView;
        private ScaledTextView _titleView;
        private GenericListAdapter<Note> _adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = (MainActivity)Activity;
            _activity.CurrentNotesFragment = this;
            _activity.CurrentTab = TabType.Notes;
            _activity.UpdateOptionsMenu();

            _inflater = inflater;

            var selectedCharacter = _activity.Game.SelectedCharacter;

            var isPhone = _activity.FormatHelper.DeviceHelper.TypeOfDevice == DeviceType.Phone;

            var root = (ViewGroup)_inflater.Inflate(isPhone ? Resource.Layout.notes_fragment_min : Resource.Layout.notes_fragment, null);

            _titleView = (ScaledTextView)root.FindViewById(Resource.Id.title);
            _detailView = (ScaledTextView)root.FindViewById(Resource.Id.detail);

            var listView = (ListView)root.FindViewById(Resource.Id.list);

            _adapter = new GenericListAdapter<Note>(_activity, selectedCharacter.Notes);
            listView.Adapter = _adapter;
            listView.ItemClick += List_Click;

            return root;
        }

        void List_Click(object sender, ListView.ItemClickEventArgs e)
        {
            var note = _adapter[e.Position];

            if (_titleView != null && _detailView != null)
            {
                _titleView.SetText(note.Name);
                _detailView.Text = note.Description;
            }
            else
            {
                new DescriptionDialog(_activity, _inflater, note);
            }
        }
           
        public void Update()
        {
            _adapter.NotifyDataSetChanged();
        }
    }
}