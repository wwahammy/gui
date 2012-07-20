using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Win32;

namespace CoApp.PackageManager.Model
{
    public class ProductInfo : ScreenViewModel
    {
        internal IColorManager ColorManager;
        private string _canonicalName;
        private string _description;
        private BitmapSource _icon;
        private bool _iconLoaded;
        private string _name;
        private IPackage _original;
        private Uri _possibleIconSource;
        private DateTime? _posted;
        private SolidColorBrush _primaryColor;
        private double? _rating;
        private string _summary;
        private SolidColorBrush _textColor;
        private FourPartVersion _version;


        public ProductInfo()
        {
            ColorManager = new LocalServiceLocator().ColorManager;
        }

        public bool IconLoaded
        {
            get { return _iconLoaded; }
            set
            {
                _iconLoaded = value;
                RaisePropertyChanged("IconLoaded");
            }
        }


        public IPackage Original
        {
            get { return _original; }
            set
            {
                _original = value;
                RaisePropertyChanged("Original");
            }
        }


        public Uri PossibleIconSource
        {
            get { return _possibleIconSource; }
            set
            {
                _possibleIconSource = value;
                RaisePropertyChanged("PossibleIconSource");
            }
        }


        public BitmapSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        public SolidColorBrush TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                RaisePropertyChanged("TextColor");
            }
        }


        public SolidColorBrush PrimaryColor
        {
            get { return _primaryColor; }
            set
            {
                _primaryColor = value;
                RaisePropertyChanged("PrimaryColor");
            }
        }


        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }


        public string CanonicalName
        {
            get { return _canonicalName; }
            set
            {
                _canonicalName = value;
                RaisePropertyChanged("CanonicalName");
            }
        }


        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged("Summary");
            }
        }


        public DateTime? Posted
        {
            get { return _posted; }
            set
            {
                _posted = value;
                RaisePropertyChanged("Posted");
            }
        }


        public double? Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                RaisePropertyChanged("Rating");
            }
        }


        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }


        public FourPartVersion Version
        {
            get { return _version; }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        public event Action IconLoadingCompleted = delegate { };


        public static ProductInfo FromIPackage(IPackage package)
        {
            var pi = new ProductInfo
                         {
                             Original = package,
                             Name = package.GetNicestPossibleName(),
                             CanonicalName = package.CanonicalName,
                             Description = package.PackageDetails.Description,
                             Summary = package.PackageDetails.SummaryDescription,
                             Posted = package.PackageDetails.PublishDate,
                             Version = package.Version,
                             PossibleIconSource =
                                 (package.PackageDetails.Icons != null && package.PackageDetails.Icons.Any())
                                     ? package.PackageDetails.Icons.First()
                                     : null
                         };

            pi.LoadBitmapIfPossible();
            return pi;
        }

        private void LoadBitmapIfPossible()
        {
            Task<IconColorPacket> task = ColorManager.GetColorPacket(PossibleIconSource);
            task.ContinueAlways(t =>
                                    {
                                        if (t.IsCompleted)
                                        {
                                            t.Result.BackgroundColor.Freeze();
                                            t.Result.ForegroundColor.Freeze();
                                            t.Result.Icon.Freeze();
                                            UpdateOnUI(() => Icon = t.Result.Icon);
                                            UpdateOnUI(() => TextColor = t.Result.ForegroundColor);
                                            UpdateOnUI(() => PrimaryColor = t.Result.BackgroundColor);
                                        }
                                        UpdateOnUI(() => IconLoaded = true);
                                        IconLoadingCompleted();
                                    });
        }
    }
}