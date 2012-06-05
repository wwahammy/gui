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

        private const byte mask = 15;
        private const string hex = "0123456789ABCDEF";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pkt"></param>
        /// <returns></returns>
        /// <from>http://msdn.microsoft.com/en-us/library/system.reflection.assemblyname.getpublickeytoken(v=vs.95).aspx</from>
        public static string PublicKeyTokenAsString(this byte[] pkt)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            foreach (byte b in pkt)
            {
                output.Append(hex[b / 16 & mask]);
                output.Append(hex[b & mask]);
            }

            return output.ToString();
        }
    }
}
