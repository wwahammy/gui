using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Gui.Toolkit.Support
{
    public static class EnumerableExtensions
    {
        public static bool ReturnsTrueOr<T> (this IEnumerable<Func<T, bool>> filters, T item)
        {
            return filters.Any(func => func.Invoke(item));
        }
    }
}
