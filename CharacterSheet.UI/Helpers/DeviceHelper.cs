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
using Android.Content.Res;
using Android.Graphics;
using Android.InputMethodServices;
using Android.Util;

namespace CharacterSheet.UI.Helpers
{
    public class DeviceHelper
    {
        private Activity _activity;
        public int StageWidth { get; protected set; }
        public bool IsPhonePortrait
        {
            get
            {
                return TypeOfDevice == DeviceType.Phone && OrientationOfDevice == DeviceOrientation.Portrait;
            }
        }

        public enum DeviceType
        {
            Phone,
            Tablet,
            XLargeTablet
        }

        public enum DeviceOrientation
        {
            Portrait,
            Landscape
        }
        
        public DeviceHelper(Activity activity)
        {
            _activity = activity;

            var adjustmentFactor = IsPhonePortrait ? 2.5 : 1.18;

            DisplayMetrics realMetrics = new DisplayMetrics();

            _activity.WindowManager.DefaultDisplay.GetRealMetrics(realMetrics);

            StageWidth = (int)((CanBeFullScreen ? realMetrics.WidthPixels : _activity.Resources.DisplayMetrics.WidthPixels) * adjustmentFactor);
        }

        public DeviceType TypeOfDevice
        {
            get
            {
                var screenLayout = _activity.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask;

                if (screenLayout >= ScreenLayout.SizeXlarge)
                {
                    return DeviceType.XLargeTablet;
                }
                else if (screenLayout >= ScreenLayout.SizeLarge)
                {
                    return DeviceType.Tablet;
                }
                else
                {
                    return DeviceType.Phone;
                }
            }
        }

        public DeviceOrientation OrientationOfDevice
        {
            get
            {
                return _activity.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
            }
        }

        private bool? _canBeFullScreen;
      
        public bool CanBeFullScreen
        {
            get
            {
                if (!_canBeFullScreen.HasValue)
                {
                    int buildVersion;

                    int.TryParse(Android.OS.Build.VERSION.Sdk, out buildVersion);

                    _canBeFullScreen = buildVersion >= 19 && TypeOfDevice == DeviceType.Phone;
                }

                return _canBeFullScreen.Value;
            }
        }
    }
}