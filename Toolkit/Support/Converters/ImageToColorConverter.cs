using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;

// from http://www.rudigrobler.net/blog/2011/9/26/make-your-wpf-buttons-color-hot-track.html under CC-BY licence

namespace CoApp.Gui.Toolkit.Support.Converters
{
    [ValueConversion(typeof(BitmapSource), typeof(Color))]
    public class ImageToColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public SolidColorBrush DefaultBackgroundColor = new SolidColorBrush( new Color {A = 255, B = 236, G = 114, R = 38});

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as BitmapImage;
            if (source != null)
            {/*
                if (source.Format.BitsPerPixel != 32 || source.Format != PixelFormats.Bgra32)
                {
                    var formatted = new FormatConvertedBitmap();
                    formatted.BeginInit();
                    formatted.Source = source;
                    formatted.DestinationFormat = PixelFormats.Bgra32;
                    formatted.EndInit();
                    source = formatted;
                    

                }
                


                var colorDist = new Dictionary<Color, double>();

                var sz = new Size(source.PixelWidth, source.PixelHeight);

                //read bitmap 
                int pixelsSz = (int) sz.Width*(int) sz.Height*(source.Format.BitsPerPixel/8);
                int stride = ((int) sz.Width*source.Format.BitsPerPixel + 7)/8;
                int pixelBytes = (source.Format.BitsPerPixel/8);

                var pixels = new byte[pixelsSz];
                source.CopyPixels(pixels, stride, 0);

                const int alphaThershold = 10;
                UInt64 pixelCount = 0;
                UInt64 avgAlpha = 0;

                for (int y = 0; y < sz.Height; y++)
                {
                    for (int x = 0; x < sz.Width; x++)
                    {
                        int index = (int) ((y*sz.Width) + x)*(pixelBytes);
                        byte r1, g1, b1, a1;
                        r1 = g1 = b1 = a1 = 0;
                        a1 = pixels[index + 3];
                        r1 = pixels[index + 2];
                        g1 = pixels[index + 1];
                        b1 = pixels[index];

                        if (a1 <= alphaThershold)
                            continue; //ignore

                        pixelCount++;
                        avgAlpha += a1;

                        Color cl = Color.FromArgb(0, r1, g1, b1);
                        double dist = 0;
                        if (!colorDist.ContainsKey(cl))
                        {
                            colorDist.Add(cl, 0);

                            for (int y2 = 0; y2 < sz.Height; y2++)
                            {
                                for (int x2 = 0; x2 < sz.Width; x2++)
                                {
                                    int index2 = (int) (y2*sz.Width) + x2;
                                    byte r2, g2, b2, a2;
                                    r2 = g2 = b2 = a2 = 0;
                                    a2 = pixels[index2 + 3];
                                    r2 = pixels[index2 + 2];
                                    g2 = pixels[index2 + 1];
                                    b2 = pixels[index2];

                                    if (a2 <= alphaThershold)
                                        continue; //ignore

                                    dist += Math.Sqrt(Math.Pow(r2 - r1, 2) +
                                                      Math.Pow(g2 - g1, 2) +
                                                      Math.Pow(b2 - b1, 2));
                                }
                            }

                            colorDist[cl] = dist;
                        }
                    }
                }

                //clamp alpha
                avgAlpha = avgAlpha/pixelCount;
                if (avgAlpha >= (255 - alphaThershold))
                    avgAlpha = 255;

                //take weighted average of top 2% of colors         
                var clrs = (from entry in colorDist
                            orderby entry.Value ascending
                            select new {Color = entry.Key, Dist = 1.0/Math.Max(1, entry.Value)}).ToList().Take(
                                Math.Max(1, (int) (colorDist.Count*0.02))).ToList();

                double sumDist = clrs.Sum(x => x.Dist);
                Color result = Color.FromArgb((byte) avgAlpha,
                                              (byte) (clrs.Sum(x => x.Color.R*x.Dist)/sumDist),
                                              (byte) (clrs.Sum(x => x.Color.G*x.Dist)/sumDist),
                                              (byte) (clrs.Sum(x => x.Color.B*x.Dist)/sumDist));

                return result;*/
                using (var m = new MemoryStream())
                {
                    System.Windows.Media.Imaging.BmpBitmapEncoder e = new BmpBitmapEncoder();
                    var frame = BitmapFrame.Create(source);
                    e.Frames.Add(frame);
                    e.Save(m);
                    Bitmap bitmap = new Bitmap(m);
                    int tr = 0;
                    int tg = 0;
                    int tb = 0;


                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            System.Drawing.Color pixel = bitmap.GetPixel(x, y);
                            tr += pixel.R;
                            tg += pixel.G;
                            tb += pixel.B;
                        }
                    }

                    byte r = (byte) Math.Floor((double) (tr/(bitmap.Height*bitmap.Width)));
                    byte g = (byte) Math.Floor((double) (tg/(bitmap.Height*bitmap.Width)));
                    byte b = (byte) Math.Floor((double) (tb/(bitmap.Height*bitmap.Width)));

                    return new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, r, g, b));
                }


            }
            return DefaultBackgroundColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}