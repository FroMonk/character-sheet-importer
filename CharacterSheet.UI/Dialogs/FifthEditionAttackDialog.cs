
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CharacterSheet.Helpers;
using CharacterSheet.Droid.Backend.Pathfinder;
using LayoutParams = Android.Views.ViewGroup.LayoutParams;
using Convert = CharacterSheet.Helpers.Convert;
using Environment = System.Environment;
using Android.App;
using CharacterSheet.UI.Helpers;
using CharacterSheet.Pathfinder;
using CharacterSheet.Droid.Backend.Pathfinder.Dice;

namespace CharacterSheet.UI.Dialogs
{
    public class FifthEditionAttackDialog : ImmersiveAlertDialog
    {
        private const string HEADER_DIVIDER = "---------------";
        private readonly string ATTACK_DIVIDER = string.Format("{0}--------------------------------------------------------------------------------{1}", Environment.NewLine, Environment.NewLine);

        public FifthEditionAttackDialog(Activity activity, IAttackRoll attackToRoll) : base(activity)
        {
            Check.ForNullArgument(activity, "context");
            Check.ForNullArgument(attackToRoll, "attackToRoll");

            var viewHelper = new ViewHelper(activity);

            var result = attackToRoll.Roll(isFifthEdition: true);

            this.SetTitle(attackToRoll.Name);

            var resultOutput = new StringBuilder();

            for (int i = 1; i <= attackToRoll.ToHit.Length; i++)
            {
                var toHit = result.ToHitMap[i];
                var discardedToHit = result.DiscardedToHitMap[i];
                var damage = result.DamageMap[i];
                var critDamage = result.CritDamageMap[i];

                string outcome = string.Empty;

                switch (result.OutcomeMap[i])
                {
                    case AttackRollOutcome.Fumble:
                        outcome = " (FUMBLED)";
                        break;
                    case AttackRollOutcome.Crit:
                        outcome = " (CRIT)";
                        break;
                    case AttackRollOutcome.Natural20:
                        outcome = " (NATURAL 20)";
                        break;
                }

                resultOutput.AppendLine(string.Format("Attack {0}{1}:", i, outcome));
                resultOutput.AppendLine(HEADER_DIVIDER);
                resultOutput.AppendLine(string.Format("To hit:\t{0}", toHit));

                if (discardedToHit != null)
                {
                    resultOutput.AppendLine(string.Format("Discarded To hit:\t{0}", discardedToHit));
                }

                resultOutput.AppendLine(critDamage == null ? string.Format("Damage:\t{0}", damage) : string.Format("Crit Damage:\t{0}", critDamage.FirstOrDefault()));

                if (i < attackToRoll.ToHit.Length)
                {
                    resultOutput.AppendLine(ATTACK_DIVIDER);
                }

            }

            var layout = viewHelper.CreateLinearLayout(Orientation.Vertical, LayoutParams.MatchParent, LayoutParams.WrapContent);

            layout.AddView(viewHelper.CreateTextView(resultOutput.ToString(), 12, LayoutParams.MatchParent, LayoutParams.WrapContent, GravityFlags.CenterHorizontal));
            layout.AddView(viewHelper.CreateTextView(string.Format("Rolled at {0}", result.RolledAt), 10, LayoutParams.MatchParent, LayoutParams.WrapContent, GravityFlags.CenterHorizontal));

            this.SetView(layout);

            this.Show();

            this.SetCanceledOnTouchOutside(true);
        }
    }
}