using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using CharacterSheet.UI.Activities;
using CharacterSheet.UI.Views;
using CharacterSheet.UI.Dialogs;
using Android.Graphics;

namespace CharacterSheet.UI.Fragments.Skills
{
    public class SkillsFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var activity = (MainActivity)Activity;
            activity.CurrentTab = TabType.Skills;
            activity.UpdateOptionsMenu();

            var isPhonePortrait = activity.FormatHelper.DeviceHelper.IsPhonePortrait;

            var root = isPhonePortrait ? (ViewGroup)inflater.Inflate(Resource.Layout.skills_fragment_min, null)
                                       : (ViewGroup)inflater.Inflate(Resource.Layout.skills_fragment, null);

            var skillsLayout = (LinearLayout)root.FindViewById(Resource.Id.skills_layout);

            bool hasColouredBackground = false;

            foreach (var skill in activity.Game.SelectedCharacter.Skills.SkillsList)
            {
                ViewGroup skillLayout = isPhonePortrait ? (ViewGroup)inflater.Inflate(Resource.Layout.skills_fragment_min_skill, null) 
                                                        : (ViewGroup)inflater.Inflate(Resource.Layout.skills_fragment_skill, null);

                var useUntrainedView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_use_untrained);
                var nameView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_name);
                var abilityView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_ability);
                var rollView = (ScaledImageButton)skillLayout.FindViewById(Resource.Id.skill_roll);
                var skillModifierView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_skill_modifier);

                var dynamicContentSkillsNameArgs = new[] { skill.Name };

                var useUntrainedContentDescArgs = new string[2] { skill.Name, "can" };

                if (!skill.UseUntrained)
                {
                    useUntrainedView.Text = string.Empty;
                    useUntrainedContentDescArgs[1] = "can't";
                }

                useUntrainedView.SetDynamicContentDescription(useUntrainedContentDescArgs);

                abilityView.SetText(skill.Ability, dynamicContentSkillsNameArgs);
                rollView.SetDynamicContentDescription(skill.Name);

                if (!string.IsNullOrWhiteSpace(skill.Description))
                {
                    nameView.SetText(skill.Name, new[] { ", press for description" });
                    nameView.SetTextColor(Color.ParseColor("#33b5e5"));

                    nameView.Click += delegate
                    {
                        new DescriptionDialog(Activity, inflater, skill);
                    };
                }
                else
                {
                    nameView.SetText(skill.Name, new[] { string.Empty });
                }

                rollView.Click += delegate
                {
                    new CheckDialog(Activity, skill);
                };

                skillModifierView.SetStatValue(skill.SkillModifier, dynamicContentSkillsNameArgs);

                if (!isPhonePortrait)
                {
                    var abilityModifierView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_ability_modifier);
                    var ranksView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_ranks);
                    var miscModifierView = (ScaledTextView)skillLayout.FindViewById(Resource.Id.skill_misc_modifier);

                    abilityModifierView.SetText(skill.AbilityModifier.ToString(), dynamicContentSkillsNameArgs);
                    ranksView.SetText(skill.Ranks.ToString(), dynamicContentSkillsNameArgs);
                    miscModifierView.SetText(skill.MiscModifier.ToString(), dynamicContentSkillsNameArgs);
                }

                if (hasColouredBackground)
                {
                    skillLayout.SetBackgroundColor(Color.LightGray);
                }

                hasColouredBackground = !hasColouredBackground;

                skillsLayout.AddView(skillLayout);
            }

            var useUntrainedAnnotation = (ScaledTextView)inflater.Inflate(Resource.Layout.skills_fragment_use_untrained_annotation, null);

            skillsLayout.AddView(useUntrainedAnnotation);

            return root;
        }
    }
}