using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CoApp.PackageManager.Model
{
    public class ItemComment
    {
        public DateTime PostTime { get; set; }
        public string Name { get; set; }
        public BitmapImage Icon { get; set; }
        public string Link { get; set; }
        public string Comment { get; set; }
    }
}
