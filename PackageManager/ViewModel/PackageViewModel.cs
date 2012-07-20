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
using CoApp.PackageManager.Support;
using CoApp.PackageManager.ViewModel.Filter;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Packaging.Common.Model;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.ViewModel
{
    public class PackageViewModel : PackageProductCommonViewModel
    {
        public readonly ReadOnlyCollection<PackageStateToName> StatesToName =
            new ReadOnlyCollection<PackageStateToName>(new[]
                {
                    new PackageStateToName
                        {
                            State = Packaging.Common.PackageState.Upgradable,
                            Name = "Upgradable"
                        },
                    new PackageStateToName
                        {
                            State = Packaging.Common.PackageState.Updatable,
                            Name = "Updatable"
                        },
                    new PackageStateToName
                        {
                            State = Packaging.Common.PackageState.DoNotChange,
                            Name = "Do not change"
                        },
                    new PackageStateToName
                        {
                            State = Packaging.Common.PackageState.Blocked,
                            Name = "Blocked"
                        }
                });

        internal IColorManager ColorManager;
        private string _bindingPolicyRange;


        private ObservableCollection<License> _licenses = new ObservableCollection<License>();

        private PackageStateToName _packageState;

        public PackageViewModel()
        {

            var loc = new LocalServiceLocator();
           
         
            ColorManager = loc.ColorManager;

            Loaded += OnLoad;
        }

        public ObservableCollection<License> Licenses
        {
            get { return _licenses; }
            set
            {
                _licenses = value;
                RaisePropertyChanged("Licenses");
            }
        }

        public PackageStateToName PackageState
        {
            get { return _packageState; }
            set
            {
                _packageState = value;
                RaisePropertyChanged("PackageState");
            }
        }


        public ReadOnlyCollection<PackageStateToName> AllPackageStates
        {
            get { return StatesToName; }
        }


        public string BindingPolicyRange
        {
            get { return _bindingPolicyRange; }
            set
            {
                _bindingPolicyRange = value;
                RaisePropertyChanged("BindingPolicyRange");
            }
        }

        public RelayCommand ElevateSetState { get; set; }
        public RelayCommand SetState { get; set; }



        private void OnLoad()
        {
            AddPostLoadTask(CoApp.GetPackage(InitializationName, true).
                                ContinueAlways(t =>
                                    {
                                        //TODO throw some error when faulted
                                        t.RethrowWhenFaulted();
                                        LoadFromPackage(t.Result);

                                        ElevateInstall = new RelayCommand(ExecuteElevateInstall,
                                                                          () => NoActionsInProgress);
                                        Install = new RelayCommand(() => ExecuteInstallPackage(t.Result),
                                                                   () => NoActionsInProgress);
                                        ElevateRemove = new RelayCommand(ExecuteElevateRemove, () => NoActionsInProgress);
                                        Remove = new RelayCommand(() => ExecuteRemovePackage(t.Result),
                                                                  () => NoActionsInProgress);
                                        ElevateSetState = new RelayCommand(ExecuteElevateSetState,
                                                                           () => NoActionsInProgress);
                                        SetState =
                                            new RelayCommand(() => ExecuteSetState(t.Result), () => NoActionsInProgress);
                                    }
                                ));
        }

        private void ExecuteElevateInstall()
        {
            DisableActions();
            var elevate = CoApp.Elevate();
            elevate.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("install packages"));
                }).Continue(() => ReenableActions());
            elevate.Continue(() => Install.Execute(null)).Continue(() => ReenableActions());
        }

        private void ExecuteElevateRemove()
        {
            DisableActions();
            var elevate = CoApp.Elevate();
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

        private void ExecuteElevateSetState()
        {
            DisableActions();
            var elevate = CoApp.Elevate();
            elevate.ContinueOnFail(ex =>
                {
                    if (ex.InnerException != null)
                        Logger.Warning("Elevate failed {0}, {1}", ex.InnerException.Message,
                                       ex.InnerException.StackTrace);
                    else
                        Logger.Warning("Elevate failed {0}, {1}", ex.Message, ex.StackTrace);
                    Messenger.Default.Send(NotEnoughPermissions("set package state"));
                }).ContinueAlways(t => ReenableActions());
            elevate.Continue(() => SetState.Execute(null)).ContinueAlways(t => ReenableActions());

        }



        private void ExecuteRemovePackage(Package p)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).ContinueAlways(t => Activity.RemovePackage(p)).
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

        private void ExecuteInstallPackage(Package result)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).ContinueAlways(
                                      t => Activity.InstallPackage(result)).
                ContinueAlways(t =>
                    {
                        if (t.IsCompleted)
                        {
                            //we get the installed status

                            var newPack = CoApp.GetPackage(result.CanonicalName);
                            newPack.Continue(task => UpdateOnUI(() => IsInstalled = task.IsInstalled));


                        }



                    }).ContinueAlways(t => ReenableActions());

        }

        private void ExecuteSetState(Package p)
        {
            Task.Factory.StartNew(DisableActions,
                                  TaskCreationOptions.AttachedToParent).ContinueAlways(
                                      t => Activity.SetState(p, PackageState.State)).
                ContinueAlways(t =>
                    {
                        if (t.IsCompleted)
                        {
                            //we get the installed status

                            var newPack = CoApp.GetPackage(p.CanonicalName);
                            newPack.Continue(task =>
                                {
                                    var pToS = AllPackageStates.First(s => s.State == task.PackageState);
                                    UpdateOnUI(() => PackageState = pToS);
                                });
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
            foreach (var cmd in ButtonRelayCommands())
            {
                if (cmd != null)
                {
                    UpdateOnUI(() => cmd.RaiseCanExecuteChanged());
                }
            }
        }

        private void ReenableActions()
        {

            EndAction();
            NotifyAllRelayCommands();
        }


        private IEnumerable<RelayCommand> ButtonRelayCommands()
        {
            return new[] {Install, ElevateInstall, ElevateRemove, Remove, ElevateSetState, SetState};
        }


        private void LoadFromPackage(IPackage p)
        {
            //TODO what happens if this throws?

           // UpdateOnUI(() => DisplayName = p.DisplayName);

            UpdateOnUI(() => Summary = p.PackageDetails.SummaryDescription);
            UpdateOnUI(() => Description = p.PackageDetails.Description);


            UpdateOnUI(() => PublisherName = p.PackageDetails.Publisher.Name);

            // Icon = LoadBitmap(p.PackageDetails.Icon);
            ColorManager.GetColorPacket().Continue(i =>
                {
                    i.Icon.Freeze();
                    i.BackgroundColor.Freeze();
                    i.ForegroundColor.Freeze();
                    UpdateOnUI(() => Icon = i.Icon);
                    UpdateOnUI(() => PrimaryColor = i.BackgroundColor);
                    UpdateOnUI(() => TextColor = i.ForegroundColor);
                });

            var nicestName = p.GetNicestName();

            UpdateOnUI(() => DisplayName = nicestName);



            UpdateOnUI(() => Title = nicestName);

            foreach (License l in p.PackageDetails.Licenses)
            {
                License lic = l;
                UpdateOnUI(() => Licenses.Add(lic));
            }

            if (p.BindingPolicy != null)
            {
                UpdateOnUI(() => BindingPolicyRange = p.BindingPolicy.Minimum + "-" + p.BindingPolicy.Maximum);
            }

            var pToS = AllPackageStates.First(s => s.State == p.PackageState);
            UpdateOnUI(() => PackageState = pToS);

            SetDependencies(p);
            SetTags(p);

        }

    }
}