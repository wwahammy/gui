using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Support;
using CoApp.PackageManager.Support;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;

namespace CoApp.PackageManager.Model
{
    public class ProductInfo
    {
        public BitmapSource Icon { get; set; }

        public string Name { get; set; }

        public string CanonicalName { get; set; }

        public string Summary { get; set; }

        public DateTime? Posted { get; set; }

        public double? Rating { get; set; }

        public string Description { get; set; }

        public static ProductInfo FromIPackage(IPackage package)
        {
            return new ProductInfo
                       {
                           Name = package.GetNicestPossibleName(),
                           CanonicalName = package.CanonicalName,
                           Description = package.PackageDetails.Description,
                           Summary = package.PackageDetails.SummaryDescription,
                           Posted = package.PackageDetails.PublishDate
                       };
        }
    }
}
