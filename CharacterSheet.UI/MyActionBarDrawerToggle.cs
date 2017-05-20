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
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Android.Text;

namespace CharacterSheet.UI
{
    public class MyActionBarDrawerToggle : SupportActionBarDrawerToggle
    {
        private View _charactersDrawer;
        private View _tabsDrawer;
        private DrawerLayout _drawerLayout;

        private float _charactersDrawerSlideOffset;
        private float _tabsDrawerSlideOffset;

        private ActionBarActivity mHostActivity;
        private int mOpenedResource;
        private int mClosedResource;

        public MyActionBarDrawerToggle(ActionBarActivity host, DrawerLayout drawerLayout, Toolbar toolbar, int openedResource, int closedResource, string closedTitle, View charactersDrawer, View tabsDrawer) : base(host, drawerLayout, toolbar, openedResource, closedResource)
        {
            mHostActivity = host;
            mOpenedResource = openedResource;
            mClosedResource = closedResource;
            ClosedTitle = closedTitle;

            _charactersDrawer = charactersDrawer;
            _tabsDrawer = tabsDrawer;
            _drawerLayout = drawerLayout;
        }

        public string ClosedTitle { get; set; }

        private void CloseDrawer(View drawer)
        {
            _drawerLayout.CloseDrawer(drawer);
        }

        public override void OnDrawerOpened(Android.Views.View drawerView)
        {
            if (drawerView == _charactersDrawer)
            {
                base.OnDrawerOpened(drawerView);

                _charactersDrawerSlideOffset = 1f;

                mHostActivity.SupportActionBar.TitleFormatted = new SpannableString("Select a character");
            }
            else if (drawerView == _tabsDrawer)
            {
                _tabsDrawerSlideOffset = 1f;
            }
        }

        public override void OnDrawerClosed(Android.Views.View drawerView)
        {
            if (drawerView == _charactersDrawer)
            {
                base.OnDrawerClosed(drawerView);

                _charactersDrawerSlideOffset = 0f;

                mHostActivity.SupportActionBar.TitleFormatted = new SpannableString(ClosedTitle);
            }
            else if (drawerView == _tabsDrawer)
            {
                _tabsDrawerSlideOffset = 0f;
            }
        }

        public override void OnDrawerSlide(Android.Views.View drawerView, float slideOffset)
        {
            if (drawerView == _charactersDrawer)
            {
                base.OnDrawerSlide(drawerView, slideOffset);

                if (_charactersDrawerSlideOffset == 0f)
                {
                    CloseDrawer(_tabsDrawer);
                    _charactersDrawerSlideOffset = slideOffset;
                }
            }            
            else if (drawerView == _tabsDrawer && _tabsDrawerSlideOffset == 0f)
            {
                CloseDrawer(_charactersDrawer);
                _tabsDrawerSlideOffset = slideOffset;
            }
        }
    }
}