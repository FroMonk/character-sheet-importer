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
using Android.Util;
using Android.Graphics;
using CharacterSheet.UI.Helpers;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.Views
{
    [Register("apps.dysfunctional.com.ScaledImageButton")]
    public class ScaledImageButton : ImageButton
    {
        private Context _context;

        private float _widthScale = -1;
        private float _heightScale = -1;
        private string _dynamicContentDescription = null;

        public ScaledImageButton(Context context) : base(context)
        {
            _context = context;
        }

        public ScaledImageButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
            SetupCustomAttributes(attrs);
        }

        public ScaledImageButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;
            SetupCustomAttributes(attrs);           
        }

        private void SetupCustomAttributes(IAttributeSet attrs)
        {
            var customAttrs = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ScaledTextView);

            _widthScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_widthScale, -1f);
            _heightScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_heightScale, -1f);
            _dynamicContentDescription = customAttrs.GetString(Resource.Styleable.ScaledTextView_dynamicContentDescription);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var _formatHelper = ((MainActivity)_context).FormatHelper;

            int newWidthMeasureSpec = widthMeasureSpec;
            int newHeightMeasureSpec = heightMeasureSpec;

            if (_widthScale != -1)
            {
                var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
                int scaledWidth = _formatHelper.Scale(_widthScale);
                newWidthMeasureSpec = MeasureSpec.MakeMeasureSpec(scaledWidth, widthMode);
            }
            
            if (_heightScale != -1)
            {
                var heightMode = MeasureSpec.GetMode(heightMeasureSpec);
                int scaledHeight = _formatHelper.Scale(_heightScale);
                newHeightMeasureSpec = MeasureSpec.MakeMeasureSpec(scaledHeight, heightMode);
            }

            base.OnMeasure(newWidthMeasureSpec, newHeightMeasureSpec);
        }

        public void SetDynamicContentDescription(params string[] args)
        {
            if (_dynamicContentDescription != null)
            {
                ContentDescription = string.Format(_dynamicContentDescription, args);
            }
        }
    }
}