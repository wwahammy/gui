using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoApp.Toolkit.Extensions;

namespace CoApp.Updater.Support
{
    public static class Extensions
    {
       public static Exception[] FindAllExceptions(this IEnumerable<Task> tasks)
       {
           return tasks.Where(t => t.IsFaulted).Select(t => t.Exception.Unwrap()).ToArray();
       }

    }
}
