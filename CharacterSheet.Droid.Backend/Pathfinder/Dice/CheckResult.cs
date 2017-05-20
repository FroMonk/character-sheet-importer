using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    public class CheckResult
    {
        private int _modifier { get; set; }

        public int Total { get; set; }
        public int Rolled { get; set; }

        private DateTime _rolledOn;
        public string RolledAt
        {
            get
            {
                return string.Format("{0:00}:{1:00}:{2:00}", _rolledOn.Hour, _rolledOn.Minute, _rolledOn.Second);
            }
        }

        public CheckResult(int modifier)
        {
            Check.ForNullArgument(modifier, "modifier");
            
            _modifier = modifier;

            Roll();
        }

        private void Roll()
        {
            Random rand = new Random();
            Rolled = rand.Next((int) DieType.D20) + 1;
            Total = Rolled + _modifier;

            _rolledOn = DateTime.Now;
        }
    }
}
