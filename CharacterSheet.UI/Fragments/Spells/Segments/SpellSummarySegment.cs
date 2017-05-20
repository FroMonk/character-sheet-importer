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

namespace CharacterSheet.UI.Fragments.Spells.Segments
{
    public class SpellSummarySegment
    {
        private MainActivity _activity;
        private Spell _spell;
        private ExpandableSpellsLevelListAdapter _adapter;

        private static EventHandler _currentPreparePlusButtonClickEvent;
        private static EventHandler _currentPrepareMinusButtonClickEvent;
        private static EventHandler _currentCastButtonClickEvent;

        private ScaledTextView _name;
        private ScaledTextView _dc;
        private ScaledTextView _save;
        private ScaledTextView _time;
        private ScaledTextView _duration;
        private ScaledTextView _range;
        private ScaledTextView _components;
        private ScaledTextView _school;
        private ScaledTextView _targetArea;
        private ScaledTextView _numberPrepared;
        private ScaledTextView _castsLeft;
        private ScaledTextView _description;

        private Button _preparePlusButton;
        private Button _prepareMinusButton;
        private Button _castButton;

        public SpellSummarySegment(Activity activity, ViewGroup root, Spell spell, ExpandableSpellsLevelListAdapter adapter)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(root, nameof(root));
            Check.ForNullArgument(spell, nameof(spell));
            Check.ForNullArgument(adapter, nameof(adapter));

            _activity = (MainActivity)activity;
            _spell = spell;
            _adapter = adapter;

            _activity.CurrentSpellSummarySegment = this;

            AssociateViews(root);
            AssignValues();
            AssignEvents();
        }

        private void AssignValues()
        {
            if (_name != null)
            {
                _name.SetText(_spell.Name);
            }

            _dc.SetText(_spell.DC);
            _save.SetText(_spell.Save);
            _time.SetText(_spell.CastTime);
            _duration.SetText(_spell.Duration);
            _range.SetText(_spell.Range);
            _components.SetText(_spell.Components);
            _school.SetText(_spell.School);
            _targetArea.SetText(_spell.Target);
            UpdateNumberPreparedText();
            UpdateCastsLeftText();
            _description.SetText(_spell.Description);

            _preparePlusButton.Visibility = _spell.NumberPrepared == -1 ? ViewStates.Gone : ViewStates.Visible;
            _prepareMinusButton.Visibility = _preparePlusButton.Visibility;

            _castButton.Visibility = ViewStates.Visible;
        }

        private void AssociateViews(ViewGroup root)
        {
            _name = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_name);
            _dc = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_dc);
            _save = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_save);
            _time = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_time);
            _duration = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_duration);
            _range = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_range);
            _components = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_components);
            _school = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_school);
            _targetArea = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_target_area);
            _numberPrepared = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_number_prepared);
            _castsLeft = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_casts_left);
            _description = (ScaledTextView)root.FindViewById(Resource.Id.spell_summary_description);

            _preparePlusButton = (Button)root.FindViewById(Resource.Id.spell_summary_prepare_plus_button);
            _prepareMinusButton = (Button)root.FindViewById(Resource.Id.spell_summary_prepare_minus_button);
            _castButton = (Button)root.FindViewById(Resource.Id.spell_summary_cast_button);
        }

        private void AssignEvents()
        {
            UpdateCastButton();
            UpdatePreparePlusButton();
            UpdatePrepareMinusButton();
        }

        private void UpdateNumberPrepared(int adjustment)
        {
            if ((adjustment > 0 && _spell.SpellClassLevel.CanPrepareSpell) || (adjustment < 0 && _spell.NumberPrepared + adjustment > -1))
            {
                _spell.NumberPrepared += adjustment;
                _spell.CastsLeft += adjustment;

                if (_spell.CastsLeft < 0)
                {
                    _spell.CastsLeft = 0;
                }
                
                if (_spell.CastsLeft > _spell.NumberPrepared)
                {
                    _spell.CastsLeft = _spell.NumberPrepared;
                }

                UpdateNumberPreparedText();
                UpdateCastsLeftText();

                _adapter.NotifyDataSetChanged();
            }
        }

        private void UpdateCastsLeftText()
        {
            _castsLeft.SetText(_spell.CastsLeft == -1 ? "At will" : _spell.CastsLeft.ToString());
        }

        private void UpdateNumberPreparedText()
        {
            _numberPrepared.SetText(_spell.NumberPrepared == -1 ? "N/A" : _spell.NumberPrepared.ToString());
        }

        public void Reset()
        {
            UpdateCastsLeftText();
            AssignEvents();
        }

        private void UpdateCastButton()
        {
            if (_currentCastButtonClickEvent != null)
            {
                _castButton.Click -= _currentCastButtonClickEvent;
                _currentCastButtonClickEvent = null;
            }

            if (_spell.CanCast)
            {
                SetButtonBackgroundColour(_castButton, true);

                _currentCastButtonClickEvent = delegate
                {
                    var hasCast = _spell.CastSpell();

                    if (hasCast)
                    {
                        UpdateCastsLeftText();
                        AssignEvents();
                        _adapter.NotifyDataSetChanged();
                    }
                };

                _castButton.Click += _currentCastButtonClickEvent;
            }
            else
            {
                SetButtonBackgroundColour(_castButton, false);
            }
        }

        private void UpdatePreparePlusButton()
        {
            if (_currentPreparePlusButtonClickEvent != null)
            {
                _preparePlusButton.Click -= _currentPreparePlusButtonClickEvent;

                _currentPreparePlusButtonClickEvent = null;
            }

            if (_spell.NumberPrepared != -1 && _spell.SpellClassLevel.CanPrepareSpell)
            {
                SetButtonBackgroundColour(_preparePlusButton, true);

                _currentPreparePlusButtonClickEvent = delegate
                {
                    UpdateNumberPrepared(1);
                    AssignEvents();
                };

                _preparePlusButton.Click += _currentPreparePlusButtonClickEvent;
            }
            else
            {
                SetButtonBackgroundColour(_preparePlusButton, false);
            }
        }

        private void UpdatePrepareMinusButton()
        {
            if (_currentPrepareMinusButtonClickEvent != null)
            {
                _prepareMinusButton.Click -= _currentPrepareMinusButtonClickEvent;

                _currentPrepareMinusButtonClickEvent = null;
            }

            if (_spell.NumberPrepared > 0)
            {
                SetButtonBackgroundColour(_prepareMinusButton, true);

                _currentPrepareMinusButtonClickEvent = delegate
                {
                    UpdateNumberPrepared(-1);
                    AssignEvents();
                    
                };

                _prepareMinusButton.Click += _currentPrepareMinusButtonClickEvent;
            }
            else
            {
                SetButtonBackgroundColour(_prepareMinusButton, false);
            }
        }

        private void SetButtonBackgroundColour(Button button, bool isEnabled)
        {
            button.SetBackgroundColor(isEnabled ? Color.ParseColor("#33b5e5") : Color.ParseColor("#808080"));
            button.SetTextColor(isEnabled ? Color.White : Color.Black);
        }
    }
}