using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoApp.Gui.Toolkit.Support
{
    public static class TaskExtensions
    {
        public static int WaitAny(this IEnumerable<Task> tasks)
        {
            return Task.WaitAny(tasks.ToArray());
        }

        public static int WaitAny (this IEnumerable<Task> tasks, CancellationToken cancellationToken)
        {
            return Task.WaitAny(tasks.ToArray(), cancellationToken);
        }

        public static int WaitAny(this IEnumerable<Task> tasks, TimeSpan timeout)
        {
            return Task.WaitAny(tasks.ToArray(), timeout);
        }

        public static int WaitAny(this IEnumerable<Task> tasks, int millisecondsTimeout )
        {
            return Task.WaitAny(tasks.ToArray(), millisecondsTimeout);
        }

        public static int WaitAny(this IEnumerable<Task> tasks, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            return Task.WaitAny(tasks.ToArray(), millisecondsTimeout, cancellationToken);
        }

        public static void WaitAll(this IEnumerable<Task> tasks)
        {
            Task.WaitAll(tasks.ToArray());
        }

        public static void WaitAll(this IEnumerable<Task> tasks, CancellationToken cancellationToken)
        {
            Task.WaitAll(tasks.ToArray(), cancellationToken);
        }

        public static void WaitAll(this IEnumerable<Task> tasks, TimeSpan timeout)
        {
            Task.WaitAll(tasks.ToArray(), timeout);
        }

        public static void WaitAll(this IEnumerable<Task> tasks, int millisecondsTimeout)
        {
            Task.WaitAll(tasks.ToArray(), millisecondsTimeout);
        }

        public static void WaitAll(this IEnumerable<Task> tasks, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            Task.WaitAll(tasks.ToArray(), millisecondsTimeout, cancellationToken);
        }

    }
}
