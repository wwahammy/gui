using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;

namespace CoApp.PackageManager.ViewModel
{
    public class ProductViewModel : PackageProductCommonViewModel
    {
        internal IActivityService Activity;
        internal ICoAppService CoApp;
        internal INavigationService Nav;
        internal ViewModelLocator VmLoc;
        private ObservableCollection<PackageToCommand> _allVersions;
        private ICommand _install32Bit;
        private bool _is32BitInstalled;
        private string _latestAuthorVersion;
        private string _latestVersion;
        private ICommand _remove32Bit;

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

        public bool Is32BitInstalled
        {
            get { return _is32BitInstalled; }
            set
            {
                _is32BitInstalled = value;
                RaisePropertyChanged("Is32BitInstalled");
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
            return CoApp.GetAllVersionsOfPackage(p).ContinueWith(t =>
                                                                     {
                                                                         t.RethrowWhenFaulted();

                                                                         return t.Result.Select(otherP =>
                                                                                                CoApp.GetPackageDetails(
                                                                                                    otherP).Result);
                                                                     }
                );
        }

        private void LoadFromPackage(IPackage p)
        {
            //TODO what happens if this throws?
            IEnumerable<Package> packages = GetAllPackageVersions(p).Result.ToArray();
            //var deps = p.Dependencies.Select(i => CoApp.GetPackageDetails(i.CanonicalName).Result).ToArray();

            var nicestName = p.GetNicestPossibleName();

            UpdateOnUI(() => DisplayName = nicestName);

            UpdateOnUI(() => Summary = p.PackageDetails.SummaryDescription);
            UpdateOnUI(() => Description = p.PackageDetails.Description);
            UpdateOnUI(() => LatestAuthorVersion = p.PackageDetails.AuthorVersion);
            UpdateOnUI(() => LatestVersion = p.Version);

            UpdateOnUI(() => PublisherName = p.PackageDetails.Publisher.Name);

            UpdateOnUI(() => Icon = ProductInfo.GetDefaultIcon());

            //get real Icon


            UpdateOnUI(() => Title = nicestName);

            SetTags(p);
            SetDependencies(p);
            SetAllVersions(packages);
        }


        private void SetTags(IPackage p)
        {
            try
            {
                if (p != null && p.PackageDetails != null && p.PackageDetails.Tags != null)
                {
                    var tags = new ObservableCollection<TagToCommand>(p.PackageDetails.Tags.Select(t =>
                                                                                                       {
                                                                                                           string tag =
                                                                                                               t;
                                                                                                           return new TagToCommand
                                                                                                                      {
                                                                                                                          Tag
                                                                                                                              =
                                                                                                                              t,
                                                                                                                          Navigate
                                                                                                                              =
                                                                                                                              new RelayCommand
                                                                                                                              (
                                                                                                                              ()
                                                                                                                              =>
                                                                                                                              Nav
                                                                                                                                  .
                                                                                                                                  GoTo
                                                                                                                                  (
                                                                                                                                      VmLoc
                                                                                                                                          .
                                                                                                                                          GetSearchViewModel
                                                                                                                                          (tag)))
                                                                                                                      };
                                                                                                       }));

                    UpdateOnUI(() => Tags = tags);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void SetDependencies(IPackage p)
        {
            try
            {
                if (p != null && p.Dependencies != null)
                {
                    var dep = new ObservableCollection<PackageToCommand>(
                        p.Dependencies.Select(d =>
                                              new PackageToCommand
                                                  {
                                                      Package =
                                                          ProductInfo.FromIPackage(
                                                              CoApp.GetPackageDetails(d.CanonicalName).Result),
                                                      Navigate =
                                                          new RelayCommand(
                                                          () =>
                                                          Nav.GoTo(VmLoc.GetPackageViewModel(d.CanonicalName.ToString())))
                                                  }));
                    UpdateOnUI(() => Dependencies = dep);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetAllVersions(IEnumerable<IPackage> packages)
        {
            try
            {
                if (packages != null)
                {
                    var allV =
                        new ObservableCollection<PackageToCommand>(
                            packages.Select(
                                pack =>
                                new PackageToCommand
                                    {
                                        Package = ProductInfo.FromIPackage(pack),
                                        Navigate =
                                            new RelayCommand(
                                            () => Nav.GoTo(VmLoc.GetPackageViewModel(pack.CanonicalName.ToString())))
                                    }));

                    UpdateOnUI(() => AllVersions = allV);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}