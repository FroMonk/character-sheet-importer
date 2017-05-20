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
using CharacterSheet.Helpers;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.Views
{
    [Register("apps.dysfunctional.com.ScaledTextView")]
    public class ScaledTextView : TextView
    {
        private Context _context;

        private FormatHelper _formatHelper;

        private float _widthScale = -1;
        private float _heightScale = -1;
        private float _marginLeftScale = -1;
        private float _marginTopScale = -1;
        private float _marginRightScale = -1;
        private float _marginBottomScale = -1;
        private float _baseTextSize = 0;
        private bool _isStatBlock = false;
        private bool _hasPrefixedText = false;
        private string _dynamicText = null;
        private string _dynamicContentDescription = null;

        private string _prefixedText;

        public ScaledTextView(Context context) : base(context)
        {
            _context = context;
        }

        public ScaledTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _context = context;
            SetupCustomAttributes(attrs);
        }

        public ScaledTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            _context = context;
            SetupCustomAttributes(attrs);           
        }

        private void SetupCustomAttributes(IAttributeSet attrs)
        {
            var customAttrs = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ScaledTextView);

            _widthScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_widthScale, -1f);
            _heightScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_heightScale, -1f);
            _marginLeftScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginLeftScale, -1f);
            _marginTopScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginTopScale, -1f);
            _marginRightScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginRightScale, -1f);
            _marginBottomScale = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_marginBottomScale, -1f);
            _baseTextSize = customAttrs.GetFloat(Resource.Styleable.ScaledTextView_baseTextSize, 0f);
            _isStatBlock = customAttrs.GetBoolean(Resource.Styleable.ScaledTextView_isStatBlock, false);
            _hasPrefixedText = customAttrs.GetBoolean(Resource.Styleable.ScaledTextView_hasPrefixedText, false);
            _dynamicText = customAttrs.GetString(Resource.Styleable.ScaledTextView_dynamicText);
            _dynamicContentDescription = customAttrs.GetString(Resource.Styleable.ScaledTextView_dynamicContentDescription);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            _formatHelper = ((MainActivity)_context).FormatHelper;

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

            SetScaledTextSize(_baseTextSize);
            
            base.OnMeasure(newWidthMeasureSpec, newHeightMeasureSpec);
        }

        public void SetText(string text)
        {
            if (_hasPrefixedText && _prefixedText == null)
            {
                _prefixedText = Text;
            }

            UpdateText(_hasPrefixedText ? string.Format("{0} {1}", _prefixedText, text) : text);
        }

        public void SetStatValue(int value)
        {
            UpdateText(_isStatBlock ? CharacterSheet.Helpers.Convert.StatToString(value) : value.ToString());
        }

        public void SetStatValue(int value, string[] additionalContentDescArgs)
        {
            SetText(_isStatBlock ? CharacterSheet.Helpers.Convert.StatToString(value) : value.ToString(), additionalContentDescArgs);
        }

        public void SetStatValue(int[] values)
        {
            UpdateText(CharacterSheet.Helpers.Convert.AttackToString(values));
        }

        private void UpdateText(string text)
        {
            Text = _dynamicText == null ? text : string.Format(_dynamicText, text);

            SetDynamicContentDescription(new[] { text });

            this.Invalidate();
        }

        public void SetText(string text, string[] additionalContentDescArgs)
        {
            Text = _dynamicText == null ? text : string.Format(_dynamicText, text);

            var dynamicContentDescArgs = new string[additionalContentDescArgs.Length + 1];
            dynamicContentDescArgs[0] = text;
            Array.Copy(additionalContentDescArgs, 0, dynamicContentDescArgs, 1, additionalContentDescArgs.Length);
            SetDynamicContentDescription(dynamicContentDescArgs);

            this.Invalidate();
        }

        public void SetDynamicContentDescription(string[] contentDescArgs)
        {
            if (_dynamicContentDescription != null)
            {
                ContentDescription = string.Format(_dynamicContentDescription, contentDescArgs);
            }
        }

        public void SetScaledTextSize(float baseTextSize)
        {
            TextSize = _formatHelper.AdjustTextSize(baseTextSize);
        }
    }
}