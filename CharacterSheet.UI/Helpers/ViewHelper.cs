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
using CharacterSheet.Helpers;
using LayoutParams = Android.Views.ViewGroup.LayoutParams;
using Android.Graphics;

namespace CharacterSheet.UI.Helpers
{
    public class ViewHelper
    {
        private Activity _activity;
        private FormatHelper _formatHelper;

        public ViewHelper(Activity activity)
        {
            Check.ForNullArgument(activity, nameof(activity));

            _activity = activity;

            _formatHelper = new FormatHelper(activity, new DeviceHelper(activity));
        }

        public Button CreateDialogButton(string text, int textSize = 0, int width = LayoutParams.MatchParent, int height = LayoutParams.WrapContent, float weight = 0f)
        {
            var button = new Button(_activity);
            button.SetBackgroundColor(Color.ParseColor("#33b5e5"));
            button.SetTextColor(Color.White);

            if (textSize > 0)
            {
                button.TextSize = _formatHelper.AdjustTextSize(textSize);
            }

            if (weight > 0f)
            {
                button.LayoutParameters = new TableRow.LayoutParams(width, LayoutParams.MatchParent, 1f);
            }
            else
            {
                button.LayoutParameters = new LayoutParams(width, height);
            }

            button.Text = text;

            return button;
        }

        public TextView CreateTextView(string text, int textSize = 0, int width = LayoutParams.WrapContent, int height = LayoutParams.WrapContent, GravityFlags gravity = GravityFlags.NoGravity)
        {
            var view = new TextView(_activity);
            view.SetTextColor(Color.Black);
            view.Text = text;

            if (textSize > 0)
            {
                view.TextSize = _formatHelper.AdjustTextSize(textSize);
            }

            view.LayoutParameters = new LayoutParams(width, height);
            view.Gravity = gravity;

            return view;
        }

        public LinearLayout CreateLinearLayout(Orientation orientation = Orientation.Horizontal, int width = LayoutParams.WrapContent, int height = LayoutParams.WrapContent, float? weight = null, int padding = 0)
        {
            var view = new LinearLayout(_activity);
            view.Orientation = orientation;
            view.LayoutParameters = weight.HasValue ? new LinearLayout.LayoutParams(width, height, weight.Value) : new LayoutParams(width, height);
            view.SetPadding(padding, padding, padding, padding);

            return view;
        }

        public EditText CreateEditText(string text, int textSize = 0, int width = LayoutParams.WrapContent, int height = LayoutParams.WrapContent, GravityFlags gravity = GravityFlags.NoGravity)
        {
            var view = new EditText(_activity);
            view.Text = text;

            if (textSize > 0)
            {
                view.TextSize = _formatHelper.AdjustTextSize(textSize);
            }

            view.LayoutParameters = new LayoutParams(width, height);
            view.Gravity = gravity;

            return view;
        }
    }
}