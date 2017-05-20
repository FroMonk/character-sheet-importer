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
using CharacterSheet.UI.Activities;
using static CharacterSheet.UI.Helpers.DeviceHelper;

namespace CharacterSheet.UI.Dialogs
{
    public class ImmersiveAlertDialog : AlertDialog
    {
        private MainActivity _activity;

        public ImmersiveAlertDialog(Activity activity) : base(activity, AlertDialog.ThemeHoloLight)
        {
            _activity = (MainActivity)activity;
        }

        public override void Show()
        {
            this.Window.SetFlags(WindowManagerFlags.NotFocusable, WindowManagerFlags.NotFocusable);

            base.Show();

            int buildVersion;

            int.TryParse(Android.OS.Build.VERSION.Sdk, out buildVersion);

            if (_activity.FormatHelper.DeviceHelper.CanBeFullScreen)
            {
                View decorView = Window.DecorView;
                var uiOptions = (int)decorView.SystemUiVisibility;
                var newUiOptions = (int)uiOptions;

                newUiOptions |= (int)SystemUiFlags.LowProfile;
                newUiOptions |= (int)SystemUiFlags.Fullscreen;
                newUiOptions |= (int)SystemUiFlags.HideNavigation;
                newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

                decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            }
            
            this.Window.ClearFlags(WindowManagerFlags.NotFocusable);
        }
    }
}