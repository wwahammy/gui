using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoApp.Toolkit.Extensions;

namespace CoApp.Gui.Toolkit.Support
{
    public static class Extensions
    {
       public static Exception[] FindAllExceptions(this IEnumerable<Task> tasks)
       {
           return tasks.Where(t => t.IsFaulted).Select<Task, Exception>(t => t.Exception.Unwrap()).ToArray();
       }

    }
}
