using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoApp.Updater.Support.Errors
{
    public class BlockedProductError : ErrorObject
    {
        private ObservableCollection<ProductToUnblock> _blockedProducts;

        public ObservableCollection<ProductToUnblock> BlockedProducts
        {
            get { return _blockedProducts; }
            set
            {
                _blockedProducts = value;
                RaisePropertyChanged("BlockedProducts");
            }
        }

        
    }
}
