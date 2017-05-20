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
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.UI.Helpers;
using Android.Graphics;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI.Dialogs
{
    public class EditItemDialog : ImmersiveAlertDialog
    {
        public EditItemDialog(Activity activity, Item item, Action updateAction, Action removeAction, bool shouldRemoveOnCancel = false) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(item, nameof(item));
            Check.ForNullArgument(updateAction, nameof(updateAction));
            Check.ForNullArgument(removeAction, nameof(removeAction));

            this.SetTitle(string.Format("Edit {0}", item.Name));

            var viewHelper = new ViewHelper(activity);

            var layout = viewHelper.CreateLinearLayout(Orientation.Vertical, width: LayoutParams.MatchParent);

            var scrollView = new ScrollView(activity);
            var scrollViewLayoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, 0, 3f);
            scrollView.LayoutParameters = scrollViewLayoutParams;

            var editablesLayout = viewHelper.CreateLinearLayout(Orientation.Vertical, width: LayoutParams.MatchParent);

            var nameLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent, padding: 10);
            var nameLabel = viewHelper.CreateTextView("Name:");
            var editName = viewHelper.CreateEditText(item.Name, width: LayoutParams.MatchParent);
            nameLayout.AddView(nameLabel);
            nameLayout.AddView(editName);

            var unitWeightLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent, padding: 10);
            var unitWeightLabel = viewHelper.CreateTextView("Unit weight (lbs):");
            var editUnitWeight = viewHelper.CreateEditText(item.UnitWeight.ToString(), width: LayoutParams.MatchParent);
            editUnitWeight.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal;
            unitWeightLayout.AddView(unitWeightLabel);
            unitWeightLayout.AddView(editUnitWeight);
            
            var costLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent, padding: 10);
            var costLabel = viewHelper.CreateTextView("Cost (gp):");
            var editCost = viewHelper.CreateEditText(item.Cost.ToString(), width: LayoutParams.MatchParent);
            editCost.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal;
            costLayout.AddView(costLabel);
            costLayout.AddView(editCost);

            var descriptionLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent, padding: 10);
            var descriptionLabel = viewHelper.CreateTextView("Description:");
            var editDescription = viewHelper.CreateEditText(item.Description, width: LayoutParams.MatchParent);
            descriptionLayout.AddView(descriptionLabel);
            descriptionLayout.AddView(editDescription);

            var buttonLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent, height: 0, weight: 1f);

            var okButton = viewHelper.CreateDialogButton("OK", 15, weight: 1f);
            var cancelButton = viewHelper.CreateDialogButton("Cancel", 15, weight: 1f);
            var removeButton = viewHelper.CreateDialogButton("Remove", 15, weight: 1f);

            buttonLayout.AddView(okButton);
            buttonLayout.AddView(cancelButton);
            buttonLayout.AddView(removeButton);

            editablesLayout.AddView(nameLayout);
            editablesLayout.AddView(unitWeightLayout);
            editablesLayout.AddView(costLayout);
            editablesLayout.AddView(descriptionLayout);
            scrollView.AddView(editablesLayout);

            layout.AddView(scrollView);
            layout.AddView(buttonLayout);

            this.SetView(layout);           

            this.Show();

            this.SetCanceledOnTouchOutside(true);

            okButton.Click += delegate
            {
                item.Name = editName.Text;
                item.UnitWeight = double.Parse(editUnitWeight.Text);
                item.Cost = int.Parse(editCost.Text);
                item.Description = editDescription.Text;

                updateAction();

                this.Dismiss();
            };

            cancelButton.Click += delegate
            {
                if (shouldRemoveOnCancel)
                {
                    removeAction();
                }
                this.Dismiss();
            };

            removeButton.Click += delegate
            {
                removeAction();
                this.Dismiss();
            };
        }    
    }
}
