using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Properties;
using CoApp.PackageManager.Support;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Win32;
using GalaSoft.MvvmLight;

namespace CoApp.PackageManager.Model
{
    public class ProductInfo : ScreenViewModel
    {
        private Uri _possibleIconSource;

        public Uri PossibleIconSource
        {
            get { return _possibleIconSource; }
            set
            {
                _possibleIconSource = value;
                RaisePropertyChanged("PossibleIconSource");
            }
        }

        

        private BitmapSource _icon;

        public BitmapSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }



        private string _canonicalName;

        public string CanonicalName
        {
            get { return _canonicalName; }
            set
            {
                _canonicalName = value;
                RaisePropertyChanged("CanonicalName");
            }
        }



        private string _summary;

        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged("Summary");
            }
        }



        private DateTime? _posted;

        public DateTime? Posted
        {
            get { return _posted; }
            set
            {
                _posted = value;
                RaisePropertyChanged("Posted");
            }
        }



        private double? _rating;

        public double? Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                RaisePropertyChanged("Rating");
            }
        }



        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }


        private FourPartVersion _version;

        public FourPartVersion Version
        {
            get { return _version; }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        

    

        public static ProductInfo FromIPackage(IPackage package)
        {
            
            

            var pi = new ProductInfo
                       {
                           Name = package.GetNicestPossibleName(),
                           CanonicalName = package.CanonicalName,
                           Description = package.PackageDetails.Description,
                           Summary = package.PackageDetails.SummaryDescription,
                           Posted = package.PackageDetails.PublishDate,
                           Version = package.Version,

                          Icon = GetDefaultIcon(),

                           PossibleIconSource = (package.PackageDetails.Icons != null && package.PackageDetails.Icons.Any()) ? package.PackageDetails.Icons.First() : null
                       };

            pi.LoadBitmapIfPossible();
            return pi;
        }

        private void LoadBitmapIfPossible()
        {
            if (PossibleIconSource != null)
            {
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = PossibleIconSource;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                bi.DownloadCompleted += (sender, args) =>
                                            {
                                                bi.Freeze();
                                                UpdateOnUI(() => Icon = bi);
                                            };
            }

        }

        public static BitmapSource GetDefaultIcon()
        {
            using (var mem = new MemoryStream(Resources.software))
            {
                var bitmapSource = new BitmapImage();
                bitmapSource.BeginInit();
                bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                bitmapSource.StreamSource = mem;
                bitmapSource.EndInit();
                bitmapSource.Freeze();
                return bitmapSource;
            }
        }
    }
}
