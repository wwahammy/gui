using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Properties;
using CoApp.PackageManager.Support;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Win32;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace CoApp.PackageManager.Model
{
    public class ProductInfo : ScreenViewModel
    {
        public static readonly SolidColorBrush DefaultBackgroundColor =
            new SolidColorBrush(new Color {A = 255, B = 236, G = 114, R = 38});


        private string _canonicalName;
        private string _description;
        private BitmapSource _icon;
        private string _name;
        private Uri _possibleIconSource;
        private DateTime? _posted;
        private SolidColorBrush _primaryColor;
        private double? _rating;
        private string _summary;
        private SolidColorBrush _textColor;
        private FourPartVersion _version;

        public ProductInfo()
        {
            PrimaryColor = CreateMyFrozenBackgroundColor();
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
                HandleIconChanged();
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

        private void HandleIconChanged()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          SolidColorBrush primaryColor = CreateBackgroundColorBrush();
                                          SolidColorBrush textColor = CreateTextColorBrush(primaryColor);
                                          primaryColor.Freeze();
                                          textColor.Freeze();
                                          UpdateOnUI(() => PrimaryColor = primaryColor);
                                          UpdateOnUI(() => TextColor = textColor);

                                      });
        }

        static ProductInfo()
        {
            if (!DefaultBackgroundColor.IsFrozen)
            {
                DefaultBackgroundColor.Freeze();
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <from>http://www.rudigrobler.net/blog/2011/9/26/make-your-wpf-buttons-color-hot-track.html</from>
        /// <license>CC-BY</license>
        public SolidColorBrush CreateBackgroundColorBrush()
        {
            try
            {



                if (Icon != null)
                {

                    var e = new BmpBitmapEncoder();
                    var pixels = PixelRetrievalExtensions.GetPixels(Icon);
                    var tempLock = new object();
                    int tr = 0;
                    int tg = 0;
                    int tb = 0;


                    Parallel.For(0, pixels.GetLength(0),
                                 () => new TemporaryColors(),
                                 (x, state, current) =>
                                     {
                                         for (int y = 0; y < pixels.GetLength(1); y++)
                                         {
                                             var pixel = pixels[x, y];
                                             current.Red += pixel.Red;
                                             current.Green += pixel.Green;
                                             current.Blue += pixel.Blue;

                                         }
                                         return current;
                                     },
                                 (current) =>
                                     {
                                         lock (tempLock)
                                         {
                                             tr += current.Red;
                                             tg += current.Green;
                                             tb += current.Blue;
                                         }
                                     });

                    /*
                    for (int x = 0; x < bitmap.Width; x++)
                    {

                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            System.Drawing.Color pixel = bitmap.GetPixel(x, y);
                            tr += pixel.R;
                            tg += pixel.G;
                            tb += pixel.B;
                        }
                    }*/

                    var r = (byte) Math.Floor((double) (tr/(pixels.Length)));
                    var g = (byte) Math.Floor((double) (tg/(pixels.Length)));
                    var b = (byte) Math.Floor((double) (tb/(pixels.Length)));

                    return new SolidColorBrush(Color.FromArgb(0xFF, r, g, b));

                }
            }
            catch (Exception)
            {

                throw;
            }
            return CreateMyFrozenBackgroundColor();
        }

        private SolidColorBrush CreateMyFrozenBackgroundColor()
        {
            var color = DefaultBackgroundColor.Clone();
            color.Freeze();
            return color;
        }

        public static SolidColorBrush CreateTextColorBrush(SolidColorBrush primaryInput)
        {
            if (primaryInput != null)
            {
                var c = new Color();

                c = primaryInput.Color;


                double l = 0.2126*c.ScR + 0.7152*c.ScG + 0.0722*c.ScB;
                if (l < 0.5)
                {
                    return Brushes.White;
                }
            }
            return Brushes.Black;
        }
    }

    internal class TemporaryColors
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

    }
}