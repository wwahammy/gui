using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.PackageManager.Properties;
using CoApp.PackageManager.Support;
using CoApp.Toolkit.Logging;

namespace CoApp.PackageManager.Model
{
    public class ColorManager : IColorManager
    {
        public static readonly SolidColorBrush DefaultBackgroundColor =
            new SolidColorBrush(new Color {A = 255, B = 236, G = 114, R = 38});

        #region IColorManager Members

        public Task<IconColorPacket> GetColorPacket(string iconUrl)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 try
                                                 {
                                                     var uri = new Uri(iconUrl);
                                                     return GetColorPacket(uri).Result;
                                                 }
                                                 catch (Exception e)
                                                 {
                                                     Logger.Error(e);
                                                     return GetColorPacket().Result;
                                                 }
                                             }, TaskCreationOptions.AttachedToParent);
        }


        public Task<IconColorPacket> GetColorPacket()
        {
            return GetColorPacket(Resources.software);
        }

        public Task<IconColorPacket> GetColorPacket(Uri iconUrl)
        {
            if (iconUrl == null)
            {
                return GetColorPacket();
            }
            return Task.Factory.StartNew(() =>
                                             {
                                                 try
                                                 {
                                                     return ExecuteGetColorPacket(iconUrl);
                                                 }
                                                 catch (Exception e)
                                                 {
                                                     Logger.Error(e);
                                                     return GetColorPacket().Result;
                                                 }
                                             }, TaskCreationOptions.AttachedToParent);
        }

        #endregion

        private Task<IconColorPacket> GetColorPacket(byte[] iconData)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var bitmapSource = new BitmapImage();
                                                 using (var mem = new MemoryStream(iconData))
                                                 {
                                                     bitmapSource.BeginInit();
                                                     bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                                                     bitmapSource.StreamSource = mem;
                                                     bitmapSource.EndInit();
                                                     bitmapSource.Freeze();
                                                 }

                                                 SolidColorBrush bgBrush = CreateBackgroundColorBrush(bitmapSource);
                                                 SolidColorBrush fgBrush = CreateTextColorBrush(bgBrush);
                                                 return new IconColorPacket
                                                            {
                                                                BackgroundColor = bgBrush,
                                                                ForegroundColor = fgBrush,
                                                                Icon = bitmapSource
                                                            };
                                             }, TaskCreationOptions.AttachedToParent);
        }

        private IconColorPacket ExecuteGetColorPacket(Uri iconUrl)
        {
            var bitmapSource = new BitmapImage();
            var tcs = new TaskCompletionSource<bool>();
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = iconUrl;
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.EndInit();
            bi.DownloadFailed += (sender, args) => tcs.SetException(args.ErrorException);
            bi.DecodeFailed += (sender, args) => tcs.SetException(args.ErrorException);
            bi.DownloadCompleted += (sender, args) =>
                                        {
                                            bi.Freeze();
                                            tcs.SetResult(true);
                                        };

            tcs.Task.Wait();

            SolidColorBrush bgBrush = CreateBackgroundColorBrush(bi);
            SolidColorBrush fgBrush = CreateTextColorBrush(bgBrush);

            return new IconColorPacket
                       {
                           BackgroundColor = bgBrush,
                           ForegroundColor = fgBrush,
                           Icon = bi
                       };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <from>http://www.rudigrobler.net/blog/2011/9/26/make-your-wpf-buttons-color-hot-track.html</from>
        /// <license>CC-BY</license>
        private SolidColorBrush CreateBackgroundColorBrush(BitmapSource icon)
        {
            try
            {
                if (icon != null)
                {
                    var e = new BmpBitmapEncoder();
                    PixelColor[,] pixels = icon.GetPixels();
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
                                             PixelColor pixel = pixels[x, y];
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

                    var brush = new SolidColorBrush(Color.FromArgb(0xFF, r, g, b));
                    brush.Freeze();
                    return brush;
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
            SolidColorBrush color = DefaultBackgroundColor.Clone();
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

        static ColorManager()
        {
            if (!DefaultBackgroundColor.IsFrozen)
            {
                DefaultBackgroundColor.Freeze();
            }
            
        }

    }

    internal class TemporaryColors
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}