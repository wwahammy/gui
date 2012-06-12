using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IColorManager
    {
        Task<SolidColorBrush> GetColorForBitmapSource(BitmapSource source);
    }
}