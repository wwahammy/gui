using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Support;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Toolkit.Win32;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.ViewModel
{
    public class ProductViewModel : PackageProductCommonViewModel
    {
        internal IColorManager ColorManager;
        private ObservableCollection<PackageToCommand> _allVersions;

        private bool _is32BitInstalled;
        private string _latestAuthorVersion;
        private string _latestVersion;


        private bool _show32Bit;

        public ProductViewModel()
        {
            var loc = new LocalServiceLocator();


            ColorManager = loc.ColorManager;
          
            Loaded += OnLoad;
        }

        public bool Show32Bit
        {
            get { return _show32Bit; }
            set
            {
                _show32Bit = value;
                RaisePropertyChanged("Show32Bit");
            }
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


        public RelayCommand Install32Bit { get; set; }


        public RelayCommand ElevateInstall32Bit { get; set; }

        public RelayCommand ElevateRemove32Bit { get; set; }

        public RelayCommand Remove32Bit { get; set; }


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


                                            Install = new RelayCommand(() => ExecuteInstallPackage(p),
                                                                       () => NoActionsInProgress);
                                            ElevateInstall = new RelayCommand(ExecuteElevateInstall,
                                                                              () => NoActionsInProgress);
                                            Remove = new RelayCommand(() => ExecuteRemovePackage(p),
                                                                      () => NoActionsInProgress);
                                            ElevateRemove = new RelayCommand(ExecuteElevateRemove,
                                                                             () => NoActionsInProgress);

                                            LoadFromPackage(p);
                                        }
                                ));
        }

        private void ExecuteRemovePackage(IPackage p)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).
                ContinueAlways(t => Activity.RemovePackage(p)).
                ContinueAlways(t =>
                    {
                        if (t.IsCompleted)
                        {
                            //we get the installed status

                            Task<Package> newPack = CoApp.GetPackage(p.CanonicalName);
                            newPack.Continue(task => UpdateOnUI(() => IsInstalled = task.IsInstalled));
                        }
                    }).ContinueAlways(t => ReenableActions());
        }


        private void ExecuteInstallPackage(IPackage p)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).
                ContinueAlways(t => Activity.InstallPackage(p)).
                ContinueAlways(t =>
                    {
                        if (t.IsCompleted)
                        {
                            //we get the installed status

                            var newPack = CoApp.GetPackage(p.CanonicalName);
                            newPack.Continue(task => UpdateOnUI(() => IsInstalled = task.IsInstalled));
                        }
                    }).ContinueAlways(t => ReenableActions());
        }


        private void DisableActions()
        {
            StartAction();
            //gets all relaycommands on this or parent types

            NotifyAllRelayCommands();
        }

        private void NotifyAllRelayCommands()
        {
            foreach (RelayCommand cmd in ButtonRelayCommands())
            {
                if (cmd != null)
                {
                    UpdateOnUI(() => cmd.RaiseCanExecuteChanged());
                }
            }
        }


        private IEnumerable<RelayCommand> ButtonRelayCommands()
        {
            return new[] {Install, Install32Bit, ElevateInstall, ElevateRemove, Remove, Remove32Bit};
        }

        private void ReenableActions()
        {
            EndAction();
            NotifyAllRelayCommands();
        }

        private void ExecuteElevateRemove()
        {
            DisableActions();
            Task elevate = CoApp.Elevate();
            elevate.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("remove packages"));
                }).ContinueAlways(t => ReenableActions());
            elevate.Continue(() => Remove.Execute(null)).ContinueAlways(t => ReenableActions());
        }

        private void ExecuteElevateInstall()
        {
            DisableActions();
            Task elevate = CoApp.Elevate();
            elevate.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("install packages"));
                }).Continue(() => ReenableActions());
            elevate.Continue(() => 
                Install.Execute(null)).Continue(() => ReenableActions());
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

            ColorManager.GetColorPacket().Continue(i =>
                {
                    i.Icon.Freeze();
                    i.BackgroundColor.Freeze();
                    i.ForegroundColor.Freeze();
                    UpdateOnUI(() => Icon = i.Icon);
                    UpdateOnUI(() => PrimaryColor = i.BackgroundColor);
                    UpdateOnUI(() => TextColor = i.ForegroundColor);
                });

            string nicestName = p.GetNicestName();

            UpdateOnUI(() => DisplayName = nicestName);

            UpdateOnUI(() => Summary = p.PackageDetails.SummaryDescription);
            UpdateOnUI(() => Description = p.PackageDetails.Description);
            UpdateOnUI(() => LatestAuthorVersion = p.PackageDetails.AuthorVersion);
            UpdateOnUI(() => LatestVersion = p.Version);

            UpdateOnUI(() => PublisherName = p.PackageDetails.Publisher.Name);

            UpdateOnUI(() => IsInstalled = p.IsInstalled);


            UpdateOnUI(() => Title = nicestName);

            SetTags(p);
            SetDependencies(p);
            SetAllVersions(packages);

            Handle32bit(p);
        }

        private void Handle32bit(IPackage package)
        {
            if (package.CanonicalName.Architecture == Architecture.x64)
            {
                //we check for a 32 bit one
                var s = new CanonicalName(package.CanonicalName);
                // s.Architecture = Architecture.x86;
                //TODO set architecture get package


                IPackage pack32 = null;
                Install32Bit = new RelayCommand(() => ExecuteInstall32Bit(pack32), () => NoActionsInProgress);
                ElevateInstall32Bit = new RelayCommand(ExecuteElevateInstall32Bit, () => NoActionsInProgress);
                Remove32Bit = new RelayCommand(() => ExecuteRemove32Bit(pack32), () => NoActionsInProgress);
                ElevateInstall32Bit = new RelayCommand(ExecuteElevateRemove32Bit, () => NoActionsInProgress);
            }
        }

        private void ExecuteElevateRemove32Bit()
        {
            DisableActions();
            Task elevate = CoApp.Elevate();
            elevate.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("remove packages"));
                }).Continue(() => ReenableActions());
            elevate.Continue(() => Install32Bit.Execute(null)).Continue(() => ReenableActions());
        }

        private void ExecuteRemove32Bit(IPackage p)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).
                ContinueAlways(t => Activity.RemovePackage(p)).
                ContinueAlways(t =>
                    {
                        if (t.IsCompleted)
                        {
                            //we get the installed status

                            Task<Package> newPack = CoApp.GetPackage(p.CanonicalName);
                            newPack.Continue(task => UpdateOnUI(() => IsInstalled = task.IsInstalled));
                        }
                    }).ContinueAlways(t => ReenableActions());
        }

        private void ExecuteElevateInstall32Bit()
        {
            DisableActions();
            Task elevate = CoApp.Elevate();
            elevate.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("install packages"));
                }).Continue(() => ReenableActions());
            elevate.Continue(() => Install32Bit.Execute(null)).Continue(() => ReenableActions());
        }

        private void ExecuteInstall32Bit(IPackage p)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).
                ContinueAlways(t => Activity.InstallPackage(p)).
                ContinueAlways(t =>
                    {
                        if (t.IsCompleted)
                        {
                            //we get the installed status

                            Task<Package> newPack = CoApp.GetPackage(p.CanonicalName);
                            newPack.Continue(task => UpdateOnUI(() => IsInstalled = task.IsInstalled));
                        }
                    }).ContinueAlways(t => ReenableActions());
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