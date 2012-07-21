using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Updater.Support
{
    public static class EnumExtensions
    {
        public static T? ParseToEnum<T>(this string input) where T : struct, IConvertible
        {
            T? ret = null;
            T test;
            if (Enum.TryParse(input, out test))
            {
                ret = test;
            }

            return ret;
        }
    }
}
