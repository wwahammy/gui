using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Updater.Model;

namespace CoApp.Updater.Messages
{
    public class BlockedProductsMessage
    {
        public IEnumerable<Product> Products;
    }
}
