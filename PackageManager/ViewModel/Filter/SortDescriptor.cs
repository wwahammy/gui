using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoApp.Packaging.Common;
using System.Linq.Expressions;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class SortDescriptor
    {
        public SortDescriptor(string title, Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> ascending, Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> descending)
        {
            Title = title;
            Ascending = ascending;
            Descending = descending;
        }


        public string Title { get; protected set; }
        public Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> Ascending { get; private set; }
        public Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>> Descending { get; private set; }

        public FrictionlessSort Create (ListSortDirection direction)
        {
            return new FrictionlessSort {Direction = direction, Ascending = Ascending, Descending = Descending, Title = Title};
        }
    }
}