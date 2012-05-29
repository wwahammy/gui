using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Common;

namespace CoApp.Updater.Model
{
    public class Product
    {
        public CanonicalName OldId { get; set; }
        public string DisplayName { get; set; }
        public string Summary { get; set; }
        public DateTime UpdateTime { get; set; }

        public CanonicalName NewId { get; set; }

        public bool IsUpgrade { get; set; }

    }
}
