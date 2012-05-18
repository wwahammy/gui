using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Packaging.Client;

namespace CoApp.PackageManager.Messages
{
    public class InstallationProgressMessage
    {
        public Package Package { get; set; }
        public double InstallationProgress { get; set; }
    }
}
