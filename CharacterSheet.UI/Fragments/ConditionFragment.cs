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
using Dropins.Chooser.Android;
using System.IO;
using CharacterSheet.Pathfinder.XMLReader;
using CharacterSheet.Pathfinder;
using System.IO.Compression;
using CharacterSheet.UI.Activities;
using CharacterSheet.UI.Dialogs;

namespace CharacterSheet.UI.Fragments
{
    public class ConditionFragment : Android.Support.V4.App.Fragment
    {
        public ConditionFragment()
        {

        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.condition_fragment, null);

            return root;
        }     
    }
}