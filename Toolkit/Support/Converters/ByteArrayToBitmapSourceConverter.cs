using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CoApp.Gui.Toolkit.Support.Converters
{
    [ValueConversion(typeof (byte[]), typeof (BitmapSource))]
    public class ByteArrayToBitmapSourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = value as byte[];
            if (array != null)
            {
                try
                {
                    using (var mem = new MemoryStream(array))
                    {
                        var bitmapSource = new BitmapImage();
                        bitmapSource.BeginInit();
                        bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                        bitmapSource.StreamSource = mem;
                        bitmapSource.EndInit();
                        return bitmapSource;
                    }
                }
                catch (Exception)
                {

                    try
                    {
                        if (parameter is byte[])
                        {
                            using (var mem = new MemoryStream((byte[])parameter))
                            {
                                var bitmapSource = new BitmapImage();
                                bitmapSource.BeginInit();
                                bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                                bitmapSource.StreamSource = mem;
                                bitmapSource.EndInit();
                                return bitmapSource;
                            }
                        }
                        else if (parameter is string)
                        {
                            var bitmapSource = new BitmapImage();
                            bitmapSource.BeginInit();
                            bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                            bitmapSource.UriSource = new Uri((string)parameter);
                            bitmapSource.EndInit();
                        }
                    }
                    catch (Exception)
                    {
                        return null;

                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}