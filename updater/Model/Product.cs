using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Updater.Model
{
    public class Product
    {
        public string OldId { get; set; }
        public string DisplayName { get; set; }
        public string Summary { get; set; }
        public DateTime UpdateTime { get; set; }

        public string NewId { get; set; }

        public bool IsUpgrade { get; set; }

    }
}
