using System;
using System.ComponentModel;
using CoApp.Packaging.Common;
using System.Linq.Expressions;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public class SortDescriptor
    {
        public SortDescriptor(string title, Expression<Func<IPackage, dynamic>> property)
        {
            Title = title;
            Property = property;
        }


        public string Title { get; protected set; }
        public Expression<Func<IPackage,dynamic>> Property { get; private set; }

        public FrictionlessSort<IPackage> Create (ListSortDirection direction)
        {
            return new FrictionlessSort<IPackage> {Direction = direction, Property = Property, Title = Title};
        }
    }
}