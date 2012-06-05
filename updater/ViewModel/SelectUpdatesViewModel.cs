using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Updater.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <from>http://stackoverflow.com/questions/639809/how-do-i-group-items-in-a-wpf-listview</from>
    public class SelectUpdatesViewModel : ScreenViewModel
    {
        private readonly IUpdateService _updateService;


        private ObservableCollection<SelectableProduct> _productList = new ObservableCollection<SelectableProduct>();

        public SelectUpdatesViewModel()
        {
            Title = "Select the updates you want to install";
            Products = new CollectionViewSource() {Source = ProductList};
            
            Products.GroupDescriptions.Add(new PropertyGroupDescription("Product.IsUpgrade", new IsUpgradeToNiceConverter()));
            var loc = new LocalServiceLocator();
            _updateService = loc.UpdateService;

            
            Save = new RelayCommand(() =>
                                        {
                                            Parallel.ForEach(ProductList.ToArray(), i =>
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

            Loaded += OnLoaded;
        }


        public CollectionViewSource Products { get; private set; }

        /// <summary>
        /// bool is whether it's selected
        /// </summary>
        public ObservableCollection<SelectableProduct> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;


                Products.Source = _productList;
                RaisePropertyChanged("ProductList");
            }
        }


        public ICommand Save { get; set; }

        public ICommand BlockProduct { get; set; }

        private void OnLoaded()
        {
            LoadSelectedProducts();
        }

  


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

            ProductList = test;
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

        private ObservableCollection<string> _usedByPackages;

        public ObservableCollection<string> UsedByPackages
        {
            get { return _usedByPackages; }
            set
            {
                _usedByPackages = value;
                RaisePropertyChanged("UsedByPackages");
            }
        }


        private ObservableCollection<string> _dependenciesNeededToUpdate;

        public ObservableCollection<string> DependenciesNeededToUpdate
        {
            get { return _dependenciesNeededToUpdate; }
            set
            {
                _dependenciesNeededToUpdate = value;
                RaisePropertyChanged("DependenciesNeededToUpdate");
            }
        }

        
        
        


        public SelectUpdatesViewModel ViewModel { get; set; }

        public Product Product { get; set; }
    }

    internal class IsUpgradeToNiceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var realVal = (bool) value;
                return realVal ? "Product Upgrades" : "Compatibility Updates";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}