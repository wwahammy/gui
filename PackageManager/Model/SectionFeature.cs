using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.PackageManager.Model
{
    public class SectionFeature
    {
        public string Name { get; set; }
        public ProductInfo TopLeft { get; set; }
        public ProductInfo BottomLeft { get; set; }
        public ProductInfo BottomCenter { get; set; }
        public ProductInfo BottomRight { get; set; }

        public string UniqueId { get; set; }
    }
}
