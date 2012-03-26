using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Updater.Messages
{
    public class InstallationFinishedMessage
    {
        public int NumberOfProductsInstalled;
        public IEnumerable<string> Warnings;
        public IEnumerable<string> Errors;
    }
}
