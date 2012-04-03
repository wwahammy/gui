using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Updater.ViewModel
{
    public class SelectUpdatesViewModel : ScreenViewModel
    {
        private readonly IUpdateService _updateService;


        private ObservableCollection<SelectableProduct> _products;

        public SelectUpdatesViewModel()
        {
            Title = "Select the updates you want to install";
            var loc = new LocalServiceLocator();
            _updateService = loc.UpdateService;

            Loaded += OnLoaded;
            Save = new RelayCommand(() =>
                                        {
                                            Parallel.ForEach(Products.ToArray(), i =>
                                                                                     {
                                                                                         if (i.IsSelected)
                                                                                         {
                                                                                             _updateService.
                                                                                                 SelectProduct(
                                                                                                     i.Product);
                                                                                         }
                                                                                         else
                                                                                             _updateService.
                                                                                                 UnselectProduct(
                                                                                                     i.Product);

                                                                                         // go to primary page
                                                                                     });
                                            loc.NavigationService.GoTo(ViewModelLocator.PrimaryViewModelStatic);
                                        });

            BlockProduct = new RelayCommand<SelectableProduct>(p => _updateService.BlockProduct(p.Product));
        }

        /// <summary>
        /// bool is whether it's selected
        /// </summary>
        public ObservableCollection<SelectableProduct> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                RaisePropertyChanged("Products");
            }
        }


        public ICommand Save { get; set; }

        public ICommand BlockProduct { get; set; }

        private void OnLoaded()
        {
            LoadSelectedProducts();
        }

        /*
        private void HandleChanged(SelectedProductsChangedMessage obj)
        {
            LoadSelectedProducts();
        }
        */


        private void LoadSelectedProducts()
        {
            IEnumerable<SelectableProduct> products = //new ObservableCollection<SelectableProduct>(
                _updateService.AllPossibleProducts.Select(
                    kp => new SelectableProduct {Product = kp.Key, IsSelected = kp.Value, ViewModel = this});

            var test = new ObservableCollection<SelectableProduct>();
            foreach (SelectableProduct p in products)
            {
                test.Add(p);
            }

            Products = test;
        }


        public XElement Serialize()
        {
            var root = new XElement("SelectUpdatesViewModel");
            return root;
        }

        public void Deserialize(XElement element)
        {
            //nothing to do
            return;
        }
    }

    public class SelectableProduct : ViewModelBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }


        public SelectUpdatesViewModel ViewModel { get; set; }

        public Product Product { get; set; }
    }
}