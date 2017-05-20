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
using Android.Graphics;
using static CharacterSheet.UI.Helpers.DeviceHelper;

namespace CharacterSheet.UI.Helpers
{
    public class FormatHelper
    {
        private Context _context;
        public DeviceHelper DeviceHelper;
        public int HighlightColor { get; } = Color.ParseColor("#33b5e5");

        public FormatHelper(Context context, DeviceHelper deviceHelper)
        {
            _context = context;
            DeviceHelper = deviceHelper;
        }

        public int Scale(double value)
        {
            if (value < 0)
            {
                return (int)value;
            }

            return (int)(DeviceHelper.StageWidth * value);
        }

        public float AdjustTextSize(float size)
        {
            if (DeviceHelper.TypeOfDevice != DeviceHelper.DeviceType.Phone)
            {
                if (DeviceHelper.TypeOfDevice == DeviceHelper.DeviceType.XLargeTablet)
                {
                    if (DeviceHelper.OrientationOfDevice == DeviceHelper.DeviceOrientation.Landscape)
                    {
                        if (size != 0)
                        {
                            return (float)Math.Floor((double)(size) * 1.2);
                        }
                        else
                        {
                            return 15;
                        }
                    }
                    else
                    {
                        if (size != 0)
                        {
                            return (float)Math.Floor((double)(size) * 0.9);
                        }
                        else
                        {
                            return 11;
                        }
                    }
                }
                else
                {
                    if (DeviceHelper.OrientationOfDevice == DeviceHelper.DeviceOrientation.Landscape)
                    {
                        if (size != 0)
                        {
                            return (float)Math.Floor((double)(size) * 1.2);
                        }
                        else
                        {
                            return 15;
                        }
                    }
                    else
                    {
                        if (size != 0)
                        {
                            return (float)Math.Floor((double)(size) * 0.8);
                        }
                        else
                        {
                            return 11;
                        }
                    }
                }
            }
            else
            {
                if (DeviceHelper.OrientationOfDevice == DeviceHelper.DeviceOrientation.Landscape)
                {
                    if (size != 0)
                    {
                        return (float)Math.Floor((double)(size) * 0.75);
                    }
                    else
                    {
                        return 10;
                    }
                }
                else
                {
                    if (size != 0)
                    {
                        return (float)Math.Floor((double)(size) * 1);
                    }
                    else
                    {
                        return 12;
                    }
                }
            }
        }
    }
}