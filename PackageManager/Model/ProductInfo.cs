using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CoApp.PackageManager.Model
{
    public class ProductInfo
    {
        public BitmapSource Icon { get; set; }

        public string Name { get; set; }

        public string CanonicalName { get; set; }

        public string Summary { get; set; }

        public DateTime Posted { get; set; }

        public double? Rating { get; set; }


    }
}
