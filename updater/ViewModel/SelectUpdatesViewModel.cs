using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Gui.Toolkit.ViewModels.Modal;
using CoApp.Packaging.Common.Exceptions;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Updater.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <from>http://stackoverflow.com/questions/639809/how-do-i-group-items-in-a-wpf-listview</from>
    public class SelectUpdatesViewModel : ScreenViewModel
    {
        private readonly IUpdateService _updateService;
        internal ICoAppService CoApp;

        private ObservableCollection<SelectableProduct> _productList = new ObservableCollection<SelectableProduct>();

        public SelectUpdatesViewModel()
        {
            Title = "Select the updates you want to install";
            Products = new CollectionViewSource {Source = ProductList};

            Products.GroupDescriptions.Add(new PropertyGroupDescription("Product.IsUpgrade",
                                                                        new IsUpgradeToNiceConverter()));
            var loc = new LocalServiceLocator();
            _updateService = loc.UpdateService;
            CoApp = loc.CoAppService;


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

                                                                                            
                                                                                        });
                                            loc.NavigationService.GoTo(ViewModelLocator.PrimaryViewModelStatic);
                                        });


            ElevateBlock = new RelayCommand<SelectableProduct>(ExecuteElevateBlock, p => true);
            Block = new RelayCommand<SelectableProduct>(ExecuteBlock, p => true);

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

        public ICommand Block { get; set; }
        public ICommand ElevateBlock { get; set; }

        private void ExecuteElevateBlock(SelectableProduct p)
        {
            Task e = CoApp.Elevate();
            e.ContinueOnFail(ex =>
                                 {
                                     if (ex.InnerException != null)
                                         Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                                        ex.InnerException.StackTrace);
                                     else
                                         Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);

                                     Messenger.Default.Send(NotEnoughPermissions());
                                 });

            e.Continue(() => Block.Execute(p));
        }


        private void ExecuteBlock(SelectableProduct p)
        {
            var t = _updateService.BlockProduct(p.Product);
            t.ContinueOnFail(e =>
                                 {
                                     Logger.Error(e);
                                     if (e is RequiresPermissionException || (e.InnerException != null && e.InnerException is RequiresPermissionException))
                                     {
                                         Messenger.Default.Send(NotEnoughPermissions());
                                     }
                                     Messenger.Default.Send(ProductStillBlocked());
                                 });
        }

        private void OnLoaded()
        {
            LoadSelectedProducts();
        }


        private void LoadSelectedProducts()
        {
            var test = new ObservableCollection<SelectableProduct>(_updateService.AllPossibleProducts.Select(
                kp =>
                new SelectableProduct
                    {
                        Product = kp.Key,
                        IsSelected = kp.Value,
                        ViewModel = this,
                        DependenciesNeededToUpdate = new ObservableCollection<string>(kp.Key.DependenciesThatNeedToUpdate)
                    }));


            ProductList = test;
        }

        private static MetroDialogBoxMessage NotEnoughPermissions()
        {
            var vm = new BasicModalViewModel
                         {Title = "Permissions problem", Content = "You don't have permission to block packages."};
            vm.SetViaButtonDescriptions(new List<ButtonDescription>
                                            {
                                                new ButtonDescription
                                                    {
                                                        Title
                                                            =
                                                            "Continue"
                                                    }
                                            });

            return new MetroDialogBoxMessage {ModalViewModel = vm};
        }

        private static MetroDialogBoxMessage ProductStillBlocked()
        {
            var vm = new BasicModalViewModel
                         {
                             Title = "Product could not be blocked",
                             Content = "There was in error in blocking this update."
                         };
            vm.SetViaButtonDescriptions(new List<ButtonDescription>
                                            {
                                                new ButtonDescription
                                                    {
                                                        Title
                                                            =
                                                            "Continue"
                                                    }
                                            });

            return new MetroDialogBoxMessage {ModalViewModel = vm};
        }
    }

    public class SelectableProduct : ViewModelBase
    {
        private ObservableCollection<string> _dependenciesNeededToUpdate;
        private bool _isSelected;

        private ObservableCollection<string> _usedByPackages;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public ObservableCollection<string> UsedByPackages
        {
            get { return _usedByPackages; }
            set
            {
                _usedByPackages = value;
                RaisePropertyChanged("UsedByPackages");
            }
        }


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
        #region IValueConverter Members

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

        #endregion
    }
}