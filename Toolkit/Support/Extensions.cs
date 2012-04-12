using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CoApp.Toolkit.Extensions;

namespace CoApp.Gui.Toolkit.Support
{
    public static class Extensions
    {
       public static Exception[] FindAllExceptions(this IEnumerable<Task> tasks)
       {
           return tasks.Where(t => t.IsFaulted).Select<Task, Exception>(t => t.Exception.Unwrap()).ToArray();
       }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> list, Func<T, int> getHashCode, Func<T, T, bool> areEqual)
        {
            return list.Distinct(new InnerEqualityComparer<T>(getHashCode, areEqual));
        }


        private class InnerEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _areEqualFunc;
            private readonly Func<T, int> _getHashCodeFunc;
            
            public InnerEqualityComparer(Func<T,int> getHashCode, Func<T,T,bool> areEqual)
            {
                _areEqualFunc = areEqual;
                _getHashCodeFunc = getHashCode;
            }

            public bool Equals(T x, T y)
            {
                return _areEqualFunc(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _getHashCodeFunc(obj);
            }
        }

    }
}
