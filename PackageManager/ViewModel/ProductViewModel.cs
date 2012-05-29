using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.PackageManager.Properties;
using CoApp.PackageManager.Support;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;

namespace CoApp.PackageManager.ViewModel
{
    public class ProductViewModel : PackageProductCommonViewModel
    {
        internal ICoAppService CoApp;
        internal INavigationService Nav;
        internal IActivityService Activity;
        internal ViewModelLocator VmLoc;
        private ObservableCollection<PackageToCommand> _allVersions;
        private ICommand _install32Bit;
        private string _latestAuthorVersion;
        private string _latestVersion;

        public ProductViewModel()
        {
            var loc = new LocalServiceLocator();
            CoApp = loc.CoAppService;
            Nav = loc.NavigationService;
            Activity = loc.ActivityService;
            VmLoc = new ViewModelLocator();

            Loaded += OnLoad;
        }

        public string LatestVersion
        {
            get { return _latestVersion; }
            set
            {
                _latestVersion = value;
                RaisePropertyChanged("LatestVersion");
            }
        }

        public string LatestAuthorVersion
        {
            get { return _latestAuthorVersion; }
            set
            {
                _latestAuthorVersion = value;
                RaisePropertyChanged("LatestAuthorVersion");
            }
        }

        public ObservableCollection<PackageToCommand> AllVersions
        {
            get { return _allVersions; }
            set
            {
                _allVersions = value;
                RaisePropertyChanged("AllVersions");
            }
        }

        public ICommand Install32Bit
        {
            get { return _install32Bit; }
            set
            {
                _install32Bit = value;
                RaisePropertyChanged("Install32Bit");
            }
        }

        private ICommand _remove32Bit;

        public ICommand Remove32Bit
        {
            get { return _remove32Bit; }
            set
            {
                _remove32Bit = value;
                RaisePropertyChanged("Remove32Bit");
            }
        }

        

        private void OnLoad()
        {
            AddPostLoadTask(CoApp.GetPackage(InitializationName, true)
                                .ContinueAlways(
                                    t =>
                                        {
                                            t.RethrowWhenFaulted();

                                            //this is our package!
                                            IPackage p =
                                                t.Result.AvailableNewest ?? t.Result.InstalledNewest;


                                            Install = new RelayCommand(() => Activity.InstallPackage(p));
                                            Remove = new RelayCommand(() => Activity.RemovePackage(p));


                                            LoadFromPackage(p);
                                        }
                                ));
        }


        private Task<IEnumerable<Package>> GetAllPackageVersions(IPackage p)
        {
            return CoApp.GetAllVersionsOfPackage(p)
                .ContinueWith(t =>
                                  {
                                      t.RethrowWhenFaulted();

                                      return t.Result.Select(otherP => CoApp.GetPackageDetails(otherP))
                                          .ContinueAlways(tasks =>
                                                              {
                                                                  tasks.
                                                                      RethrowWhenFaulted();
                                                                  return
                                                                      tasks.Select(
                                                                          task => task.Result);
                                                              }).Result;
                                  }
                );
        }

        private void LoadFromPackage(IPackage p)
        {
            //TODO what happens if this throws?
            Task<IEnumerable<Package>> packages = GetAllPackageVersions(p);
            UpdateOnUI(() => DisplayName = p.DisplayName);

            UpdateOnUI(() => Summary = p.PackageDetails.SummaryDescription);
            UpdateOnUI(() => Description = p.PackageDetails.Description);
            UpdateOnUI(() => LatestAuthorVersion = p.PackageDetails.AuthorVersion);
            UpdateOnUI(() => LatestVersion = p.Version);

            UpdateOnUI(() => PublisherName = p.PackageDetails.Publisher.Name);

           // Icon = LoadBitmap(p.PackageDetails.Icon);
            string title = !String.IsNullOrEmpty(DisplayName) ? DisplayName : p.Name;
            UpdateOnUI(() => Title = title);
            var tags = new ObservableCollection<TagToCommand>(p.PackageDetails.Tags.Select(t =>
                                                                                {
                                                                                    string tag = t;
                                                                                    return new TagToCommand
                                                                                               {
                                                                                                   Tag = t,
                                                                                                   Navigate =
                                                                                                       new RelayCommand(
                                                                                                       () =>
                                                                                                       Nav.GoTo(
                                                                                                           VmLoc.
                                                                                                               GetSearchViewModel
                                                                                                               (tag)))
                                                                                               };
                                                                                }));

            UpdateOnUI(() => Tags = tags);

            var dep = new ObservableCollection<PackageToCommand>(
                p.Dependencies.Select(d => new PackageToCommand {Package = new ProductInfo {Name = d.Name}}));
            UpdateOnUI(() => Dependencies = dep);

            var allV =
                new ObservableCollection<PackageToCommand>(
                    packages.Result.Select(
                        pack => new PackageToCommand {Package = new ProductInfo {Name = pack.GetNicestPossibleName()}}));

            UpdateOnUI(() => AllVersions = allV);
        }




        //TODO where should this go?
        private BitmapSource LoadBitmap(string base64String)
        {
            try
            {
                byte[] array = Convert.FromBase64String(base64String);
                using (var mem = new MemoryStream(array))
                {
                    var bitmapSource = new BitmapImage();
                    bitmapSource.BeginInit();
                    bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                    bitmapSource.StreamSource = mem;
                    bitmapSource.EndInit();
                    bitmapSource.Freeze();
                    return bitmapSource;
                }
            }
            catch (Exception)
            {
                using (var mem = new MemoryStream(Resources.software))
                {
                    var bitmapSource = new BitmapImage();
                    bitmapSource.BeginInit();
                    bitmapSource.CacheOption = BitmapCacheOption.OnLoad;

                    bitmapSource.StreamSource = mem;
                    bitmapSource.EndInit();
                    bitmapSource.Freeze();
                    return bitmapSource;
                }
            }
        }
    }
}