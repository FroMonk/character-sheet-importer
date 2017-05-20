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

namespace CharacterSheet.UI.Views
{
    [Register("apps.dysfunctional.com.NestedExpandableListView")]
    public class NestedExpandableListView : ExpandableListView
    {
        public NestedExpandableListView(Context context) : base(context)
        {
        }

        public NestedExpandableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public NestedExpandableListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            heightMeasureSpec = MeasureSpec.MakeMeasureSpec(MeasuredSizeMask, MeasureSpecMode.AtMost);

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
    }
}