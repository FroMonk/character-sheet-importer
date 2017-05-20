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
using CharacterSheet.Pathfinder;
using CharacterSheet.UI.Views;
using System.Reflection;
using System.ComponentModel;
using CharacterSheet.Helpers;
using CharacterSheet.UI.Activities;
using Android.Graphics;
using CharacterSheet.UI.Dialogs;

namespace CharacterSheet.UI.Fragments.Items.Segments
{
    public class ItemSummarySegment
    {
        private MainActivity _activity;
        private Item _item;
        private GenericListAdapter<Item> _adapter;
        private ImmersiveAlertDialog _dialog;

        private static EventHandler _currentPlusButtonClickEvent;
        private static EventHandler _currentMinusButtonClickEvent;
        private static EventHandler _currentEditButtonClickEvent;

        private ScaledTextView _descriptionView;
        private ScaledTextView _titleView;
        private ScaledTextView _quantityView;
        private ScaledTextView _costView;
        private ScaledTextView _unitWeightView;
        private ScaledTextView _weightView;
        private ScaledTextView _locationView;

        private Button _plusButton;
        private Button _minusButton;
        private Button _editButton;

        public ItemSummarySegment(Activity activity, ViewGroup root, Item item, GenericListAdapter<Item> adapter, ImmersiveAlertDialog dialog = null)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(root, nameof(root));
            Check.ForNullArgument(item, nameof(item));
            Check.ForNullArgument(adapter, nameof(adapter));

            _activity = (MainActivity)activity;
            _item = item;
            _adapter = adapter;
            _dialog = dialog;

            AssociateViews(root);
            AssignValues();
            AssignEvents();
        }

        private void AssignValues()
        {
            if (_titleView != null)
            {
                _titleView.SetText(_item.Name);
            }

            _descriptionView.SetText(_item.Description);
            _quantityView.SetText(_item.Qty.ToString());
            _costView.SetText(_item.Cost.ToString());
            _unitWeightView.SetText(_item.UnitWeight.ToString());
            _weightView.SetText(_item.Weight.ToString());

            if (!string.IsNullOrWhiteSpace(_item.Location))
            {
                _locationView.SetText(_item.Location);
            }
            else
            {
                _locationView.Visibility = ViewStates.Gone;
            }
        }

        private void AssociateViews(ViewGroup root)
        {
            _titleView = (ScaledTextView)root.FindViewById(Resource.Id.title);
            _descriptionView = (ScaledTextView)root.FindViewById(Resource.Id.item_description);
            _quantityView = (ScaledTextView)root.FindViewById(Resource.Id.item_qty);
            _costView = (ScaledTextView)root.FindViewById(Resource.Id.item_cost);
            _unitWeightView = (ScaledTextView)root.FindViewById(Resource.Id.item_unit_weight);
            _weightView = (ScaledTextView)root.FindViewById(Resource.Id.item_weight);
            _locationView = (ScaledTextView)root.FindViewById(Resource.Id.item_location);

            _plusButton = (Button)root.FindViewById(Resource.Id.item_summary_plus_button);
            _minusButton = (Button)root.FindViewById(Resource.Id.item_summary_minus_button);
            _editButton = (Button)root.FindViewById(Resource.Id.item_summary_edit_button);

            ToggleVisibility(ViewStates.Visible);
        }

        private void AssignEvents()
        {
            UpdateEditButton();
            UpdatePreparePlusButton();
            UpdatePrepareMinusButton();
        }

        private void ToggleVisibility(ViewStates visibility)
        {
            if (_titleView != null)
            {
                _titleView.Visibility = visibility;
            }

            _descriptionView.Visibility = visibility;
            _quantityView.Visibility = visibility;
            _costView.Visibility = visibility;
            _unitWeightView.Visibility = visibility;
            _weightView.Visibility = visibility;
            _locationView.Visibility = visibility;

            _plusButton.Visibility = visibility;
            _minusButton.Visibility = visibility;
            _editButton.Visibility = visibility;
        }

        private void UpdateEditButton()
        { 
            if (_currentEditButtonClickEvent != null)
            {
                _editButton.Click -= _currentEditButtonClickEvent;

                _currentEditButtonClickEvent = null;
            }

            _currentEditButtonClickEvent = delegate
            {
                new EditItemDialog(_activity,
                                   _item,
                                   updateAction: () =>
                                   {
                                       AssignValues();

                                       _activity.CurrentItemsFragment.Update();
                                   },
                                   removeAction: () =>
                                   {
                                       _adapter.Remove(_item);

                                       _activity.CurrentItemsFragment.Update();

                                       if (_dialog == null)
                                       {
                                           ToggleVisibility(ViewStates.Gone);
                                       }
                                       else
                                       {
                                           _dialog.Dismiss();
                                       }
                                   });
            };

            _editButton.Click += _currentEditButtonClickEvent;           
        }

        private void UpdateQuantity(int adjustment)
        {
            _item.Qty += adjustment;
            _quantityView.SetText(_item.Qty.ToString());
            _weightView.SetText(_item.Weight.ToString());

            _activity.CurrentItemsFragment.Update();
        }

        private void UpdatePreparePlusButton()
        {
            if (_currentPlusButtonClickEvent != null)
            {
                _plusButton.Click -= _currentPlusButtonClickEvent;

                _currentPlusButtonClickEvent = null;
            }
            
            _currentPlusButtonClickEvent = delegate
            {
                UpdateQuantity(1);
                AssignEvents();
            };

            _plusButton.Click += _currentPlusButtonClickEvent;
        }

        private void UpdatePrepareMinusButton()
        {
            if (_currentMinusButtonClickEvent != null)
            {
                _minusButton.Click -= _currentMinusButtonClickEvent;

                _currentMinusButtonClickEvent = null;
            }

            if (_item.Qty > 0)
            {
                SetButtonBackgroundColour(_minusButton, true);

                _currentMinusButtonClickEvent = delegate
                {
                    UpdateQuantity(-1);
                    AssignEvents();                    
                };

                _minusButton.Click += _currentMinusButtonClickEvent;
            }
            else
            {
                SetButtonBackgroundColour(_minusButton, false);
            }
        }

        private void SetButtonBackgroundColour(Button button, bool isEnabled)
        {
            button.SetBackgroundColor(isEnabled ? Color.ParseColor("#33b5e5") : Color.ParseColor("#808080"));
            button.SetTextColor(isEnabled ? Color.White : Color.Black);
        }
    }
}