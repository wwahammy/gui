using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.PackageManager.Support
{
    static class StringExtensions
    {
        public static DateTime? ParseDate(this string s)
        {
            DateTime d;
            if (DateTime.TryParse(s, out d))
            {
                return d;
            }

            return null;
        }
    }
}
