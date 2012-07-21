using System;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IColorManager
    {
        Task<IconColorPacket> GetColorPacket(string iconUrl);
        Task<IconColorPacket> GetColorPacket();
        Task<IconColorPacket> GetColorPacket(Uri iconUrl);
    }

    public class IconColorPacket
    {
        public BitmapImage Icon { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }
        public SolidColorBrush ForegroundColor { get; set; }
    }

}