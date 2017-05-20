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
    [Register("apps.dysfunctional.com.ScaledHorizontalScrollView")]
    public class ScaledHorizontalScrollView : HorizontalScrollView
    {
        private Context _context;

        private float _widthScale = -1;
        private float _heightScale = -1;
        private float _maxWidthScale = -1;
        private float _marginLeftScale = -1;
        private float _marginTopScale = -1;
        private float _marginRightScale = -1;
        private float _marginBottomScale = -1;

        public ScaledHorizontalScrollView(Context context) : base(context)
        {
            _context = context;
        }

        public ScaledHorizontalScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
            SetupCustomAttributes(attrs);
        }

        public ScaledHorizontalScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;
            SetupCustomAttributes(attrs);           
        }

        private void SetupCustomAttributes(IAttributeSet attrs)
        {
            var customAttrs = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ScaledTextView);

            _widthScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_widthScale, -1f);
            _heightScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_heightScale, -1f);
            _maxWidthScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_maxWidthScale, -1f);
            _marginLeftScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginLeftScale, -1f);
            _marginTopScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginTopScale, -1f);
            _marginRightScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginRightScale, -1f);
            _marginBottomScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginBottomScale, -1f);
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

            if (_maxWidthScale != -1)
            {
                var measuredWidth = MeasureSpec.GetSize(widthMeasureSpec);

                var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
                int scaledWidth = _formatHelper.Scale(_maxWidthScale);

                if (measuredWidth > scaledWidth)
                {
                    newWidthMeasureSpec = MeasureSpec.MakeMeasureSpec(scaledWidth, widthMode);
                }
            }

            if (_marginLeftScale != -1 || _marginTopScale != -1 || _marginRightScale != -1 || _marginBottomScale != -1)
            {
                ViewGroup.MarginLayoutParams layoutParams = (ViewGroup.MarginLayoutParams)LayoutParameters;

                var left = _marginLeftScale == -1 ? layoutParams.LeftMargin : _formatHelper.Scale(_marginLeftScale);
                var top = _marginTopScale == -1 ? layoutParams.TopMargin : _formatHelper.Scale(_marginTopScale);
                var right = _marginRightScale == -1 ? layoutParams.RightMargin : _formatHelper.Scale(_marginRightScale);
                var bottom = _marginBottomScale == -1 ? layoutParams.BottomMargin : _formatHelper.Scale(_marginBottomScale);

                layoutParams.SetMargins(left, top, right, bottom);

                LayoutParameters = layoutParams;
            }

            base.OnMeasure(newWidthMeasureSpec, newHeightMeasureSpec);
        }
    }
}