using System;
using System.Collections.ObjectModel;
using System.Linq;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.PackageManager.Model;
using CoApp.Packaging.Common;
using CoApp.Packaging.Common.Model;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;
using NavigationService = CoApp.Gui.Toolkit.Model.NavigationService;

namespace CoApp.PackageManager.ViewModel
{
    public class PackageViewModel : PackageProductCommonViewModel
    {
        internal ICoAppService CoApp;
        internal INavigationService Nav;
        internal ViewModelLocator VmLoc;


        private ObservableCollection<License> _licenses = new ObservableCollection<License>();

        public ObservableCollection<License> Licenses
        {
            get { return _licenses; }
            set
            {
                _licenses = value;
                RaisePropertyChanged("Licenses");
            }
        }

        
        public PackageViewModel()
        {
            Loaded += OnLoad;
            var loc = new LocalServiceLocator();
            CoApp = loc.CoAppService;
            VmLoc = new ViewModelLocator();
            Nav = loc.NavigationService;
        }

        private void OnLoad()
        {
            AddPostLoadTask(CoApp.GetPackage(InitializationName, true).
                ContinueAlways(t =>
                                                                                           {
                                                                                               t.RethrowWhenFaulted();
                                                                                               LoadFromPackage(t.Result);
                                                                                           }

                                ));
        }

        private void LoadFromPackage(IPackage p)
        {
            //TODO what happens if this throws?
            
            UpdateOnUI(() => DisplayName = p.DisplayName);

            UpdateOnUI(() => Summary = p.PackageDetails.SummaryDescription);
            UpdateOnUI(() => Description = p.PackageDetails.Description);
 

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
                p.Dependencies.Select(d => new PackageToCommand { Package = ProductInfo.FromIPackage(d) }));
            UpdateOnUI(() => Dependencies = dep);

            foreach (var l in p.PackageDetails.Licenses)
            {
                var lic = l;
                UpdateOnUI(() => Licenses.Add(lic));
            }

            if (p.BindingPolicy != null)
            {
                UpdateOnUI(() => BindingPolicyRange = p.BindingPolicy.Minimum + "-" + p.BindingPolicy.Maximum);
            }

        }


        private string _bindingPolicyRange;

        public string BindingPolicyRange
        {
            get { return _bindingPolicyRange; }
            set
            {
                _bindingPolicyRange = value;
                RaisePropertyChanged("BindingPolicyRange");
            }
        }

        
    }
}