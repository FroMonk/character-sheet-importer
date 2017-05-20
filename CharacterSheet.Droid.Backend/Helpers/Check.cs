using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Helpers
{
    public class Check
    {
        public static void ForNullArgument(object arg, string argName)
        {
            if (arg == null) throw new ArgumentNullException(string.Format("A null value for {0} was passed into a method/constructor", argName));
        }
    }
}
