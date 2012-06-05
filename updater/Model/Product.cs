using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;

namespace CoApp.Updater.Model
{
    public class Product
    {
        public Product()
        {
            UsedBy = new XList<string>();
            DependenciesThatNeedToUpdate = new XList<string>();
        }

        public CanonicalName OldId { get; set; }
        public string DisplayName { get; set; }
        public string Summary { get; set; }
        public DateTime UpdateTime { get; set; }

        public CanonicalName NewId { get; set; }

        public bool IsUpgrade { get; set; }

        public IList<string> UsedBy { get; set; }

        public IList<string> DependenciesThatNeedToUpdate { get; set; }

    }
}
