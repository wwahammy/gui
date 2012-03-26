using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Updater.Model;

namespace CoApp.Updater.Messages
{
    public class InstallationFailedMessage
    {
        public Dictionary<Product, IEnumerable<string>> ErrorsByProduct 
            = new Dictionary<Product, IEnumerable<string>>(); 
    }
}

