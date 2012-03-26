using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Updater.Model;

namespace CoApp.Updater.Messages
{
    class InstallationProgressMessage
    {
        public double TotalProgressCompleted;
        public double ProductProgressCompleted;
        public Product CurrentProduct;
        public int CurrentProductNumber;
        public int TotalNumberOfProducts;
    }
}
