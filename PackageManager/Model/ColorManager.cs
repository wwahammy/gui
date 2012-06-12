using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CoApp.PackageManager.Model.Interfaces;

namespace CoApp.PackageManager.Model
{
    public class ColorManager : IColorManager
    {
        public Task<SolidColorBrush> GetColorForBitmapSource(BitmapSource source)
        {
            //if (source is BitmapCacheBrush)
            return null;
        }
    }
}
