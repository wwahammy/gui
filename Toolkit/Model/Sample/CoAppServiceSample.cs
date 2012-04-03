using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Toolkit.DynamicXml;
using CoApp.Toolkit.Engine.Client;

namespace CoApp.Gui.Toolkit.Model.Sample
{
#if SAMPLEDATA
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
   


    public class CoAppServiceSample : ICoAppService
    {
        private const int MS_TO_WAIT = 30;
        private readonly List<PackageDecl> _prods = new List<PackageDecl>();
        private readonly dynamic _root;
        private readonly List<string> _sysFeeds = new List<string>();
        private bool _optedIn = true;
        private readonly Dictionary<PolicyType, IList<string>> _membersForPolicies = new Dictionary<PolicyType, IList<string>>();

        private ScheduledTask _scheduledTask;

        public CoAppServiceSample()
        {
            //AllPossibleProducts = new Dictionary<Product, bool>();

            _root = new DynamicNode(XDocument.Load("SampleData.xml"));
            LoadFeeds();
            LoadPackages();
            LoadLastInstalled();

            LoadAutoInstallType();
            LoadScheduledTask();
            LoadPolicyTypes();
        }

        private void LoadPolicyTypes()
        {
            var everyone = new[] {PolicyType.Connect, PolicyType.UpdatePackage, PolicyType.EnumeratePackages};
            var admins = new[]
                             {
                                 PolicyType.ChangeActivePackage, PolicyType.FreezePackage, PolicyType.RemovePackage,
                                 PolicyType.InstallPackage, PolicyType.EditSystemFeeds, PolicyType.ModifyPolicy,
                                 PolicyType.ChangeBlockedState, PolicyType.ChangeActivePackage, PolicyType.ChangeRequiredState, PolicyType.EditSessionFeeds
                             };
            foreach (var p in everyone)
            {
                _membersForPolicies[p] = new[] { "S-1-1-0" }.ToList();
            }


            foreach (var p in admins)
            {
                _membersForPolicies[p] = new List<string> { "Administrators" };    
            }
        }

        private void LoadScheduledTask()
        {
            dynamic element = _root.ScheduledTask;
            if (element != null)
            {
                _scheduledTask = new ScheduledTask
                                     {
                                         Name = element.Attributes.Name,
                                         DayOfWeek =
                                             String.IsNullOrWhiteSpace(element.Attributes.DayOfWeek)
                                                 ? null
                                                 : Enum.Parse(typeof (DayOfWeek), element.Attributes.DayOfWeek),
                                         Hour = int.Parse( element.Attributes.Time),
                                         CommandLine = element.Attributes.CommandLine,
                                         Executable = element.Attributes.Executable

                                     };
            }

        }

        private IEnumerable<Package> AllUpgradesAndUpdates
        {
            get { return _prods.Select(p => p.Update).Concat(_prods.Select(p => p.Upgrade)); }
        }

        #region ICoAppService Members

 
        public Task<PolicyProxy> GetPolicy(PolicyType type)
        {
            return Task.Factory.StartNew(
                () => 
                    new PolicyProxy {Name = type.ToString(), Members = _membersForPolicies[type]});
        }

        public Task AddPrincipalToPolicy(PolicyType type, string principal)
        {
            return Task.Factory.StartNew(() => _membersForPolicies[type].Add(principal));
        }

        public Task RemovePrincipalFromPolicy(PolicyType type, string principal)
        {
            return Task.Factory.StartNew(() => _membersForPolicies[type].Remove(principal));
        }


        public Task<IEnumerable<string>> SystemFeeds
        {
            get { return Task.Factory.StartNew(() => (IEnumerable<string>) _sysFeeds); }
        }

        public Task AddSystemFeed(string feedUrl)
        {
            return Task.Factory.StartNew(() => _sysFeeds.Add(feedUrl));
        }

        public Task RemoveSystemFeed(string feedUrl)
        {
            return Task.Factory.StartNew(() => _sysFeeds.Remove(feedUrl));
        }

        public Task<IEnumerable<string>> SessionFeeds
        {
            get { return Task.Factory.StartNew(() => Enumerable.Empty<string>()); }
        }

        public Task<bool> OptedIn
        {
            get { return Task.Factory.StartNew(() => _optedIn); }
        }

        public Task SetOptedIn(bool optedIn)
        {
            return Task.Factory.StartNew(() => _optedIn = optedIn);
        }


        public Task BlockPackage(string packageName)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 IEnumerable<Package> ps =
                                                     AllUpgradesAndUpdates.Where(p => p.CanonicalName == packageName);
                                                 if (ps.Any())
                                                 {
                                                     Package first = ps.First();
                                                     first.IsBlocked = true;
                                                 }
                                             });
        }

        public Task UnblockPackage(string packageName)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 IEnumerable<Package> ps =
                                                     AllUpgradesAndUpdates.Where(p => p.CanonicalName == packageName);
                                                 if (ps.Any())
                                                 {
                                                     Package first = ps.First();
                                                     first.IsBlocked = false;
                                                 }
                                             });
        }

        public Task<IEnumerable<Package>> GetPackages()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Package>> GetUpdatablePackages()
        {
            return Task.Factory.StartNew(() => _prods.Where(p => p.Update != null).Cast<Package>());
        }

        public Task<IEnumerable<Package>> GetUpgradablePackages()
        {
            return Task.Factory.StartNew(() => _prods.Where(p => p.Upgrade != null).Cast<Package>());
        }

        public Task<PackageSet> GetPackageSet(string canonicalName)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var ret = new PackageSet();

                                                 PackageDecl prod =
                                                     _prods.First(p => p.CanonicalName == canonicalName);

                                                 if (prod.Update != null)

                                                     ret.AvailableNewerCompatible = prod.Update;

                                                 if (prod.Upgrade != null)
                                                     ret.AvailableNewer = prod.Upgrade;
                                                 return ret;
                                             }
                );
        }

        public Task<Package> GetPackageDetails(Package package)
        {
            return Task.Factory.StartNew(() => package);
        }

        public Task<Package> GetPackageDetails(string canonicalName)
        {
            return Task.Factory.StartNew(() => (Package) _prods.First(p => p.CanonicalName == canonicalName));
        }

        public Task UpdateExistingPackage(string canonicalName, bool? autoUpgrade,
                                          Action<string, int, int> installProgress, Action<string> packageInstalled)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 PackageDecl prod = _prods.First(p => p.CanonicalName == canonicalName);
                                                 for (int i = 0; i < 100; i++)
                                                 {
                                                     Thread.Sleep(MS_TO_WAIT);
                                                     if (installProgress != null)
                                                        installProgress.Invoke(prod.Update.CanonicalName, i, i);
                                                 }

                                                 if (packageInstalled != null)
                                                    packageInstalled.Invoke(prod.Update.CanonicalName);

                                                 prod.Update = null;
                                                 if (prod.Update == null && prod.Upgrade == null)
                                                 {
                                                     _prods.Remove(prod);
                                                 }
                                             }
                );
        }

        public Task UpgradeExistingPackage(string canonicalName, bool? autoUpgrade,
                                           Action<string, int, int> installProgress, Action<string> packageInstalled)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 PackageDecl prod = _prods.First(p => p.CanonicalName == canonicalName);
                                                 for (int i = 0; i < 100; i++)
                                                 {
                                                     Thread.Sleep(MS_TO_WAIT);

                                                     if (installProgress != null)
                                                        installProgress.Invoke(prod.Upgrade.CanonicalName, i, i);
                                                 }

                                                 if (packageInstalled != null)
                                                    packageInstalled.Invoke(prod.Upgrade.CanonicalName);

                                                 prod.Upgrade = null;
                                                 if (prod.Update == null && prod.Upgrade == null)
                                                 {
                                                     _prods.Remove(prod);
                                                 }
                                             }
                );
        }

        public UpdateChoice UpdateChoice { get; set; }

        public bool TrimOnUpdate { get; set; }

        public DateTime? LastTimeInstalled { get; set; }

        public DateTime? LastTimeChecked
        {
            get { throw new NotImplementedException(); }
        }

        public Task SetAllFeedsStale()
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task<ScheduledTask> GetScheduledTask(string name)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 if (_scheduledTask != null && _scheduledTask.Name == name)
                                                     return _scheduledTask;
                                                 return null;

                                             });
        }

        public Task SetScheduledTask(string name, string executable, string commandline, int hour, int minutes,
                                     DayOfWeek? dayOfWeek, int intervalInMinutes)
        {
            return Task.Factory.StartNew(() =>
            {
                if (_scheduledTask == null ||_scheduledTask != null && _scheduledTask.Name == name)
                    _scheduledTask = new ScheduledTask
                                         {
                                             CommandLine = commandline,
                                             Name = name,
                                             Executable = executable,
                                             Hour = hour,
                                             IntervalInMinutes = intervalInMinutes,
                                             Minutes = minutes,
                                             DayOfWeek = dayOfWeek
                                         };

            });
        }

        public Task RemoveScheduledTask(string name)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 if (_scheduledTask != null && _scheduledTask.Name == name)
                                                     _scheduledTask = null;
                                             });
        }

        public Task TrimAll()
        {
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        private void LoadFeeds()
        {
            foreach (dynamic item in _root.Feeds.Feed)
            {
                if (item.Element.Name == "Feed")
                {
                    _sysFeeds.Add(item.Element.Attributes.Url);
                }
            }
        }


        private void LoadLastInstalled()
        {
            LastTimeInstalled =
                DateTime.Parse(_root.LastInstalled.Element.Value);
        }

        private void LoadPackages()
        {
             foreach (dynamic item in _root.Products)
                        {
                            if (item.Element.Name == "Package")
                            {
                                var product = new PackageDecl
                                                  {
                                                      CanonicalName = item.Attributes.Name
                                                  };
                                if (item["Type=update"] != null)
                                {
                                    product.Update = CreateItem(item["Type=update"]);
                                }

                                if (item["Type=upgrade"] != null)
                                {

                                    product.Upgrade = CreateItem(item["Type=upgrade"]);
                                }

                                _prods.Add(product);
                            }
                        }

        }

        private dynamic CreateItem(dynamic input)
        {
            return new PackageDecl
            {
                DisplayName =
                                     input.Attributes.Name,
                Summary =
                    input.Summary.Element.Value,
                PublishDate = DateTime.Today.ToString(CultureInfo.InvariantCulture),
                CanonicalName = input.Attributes.Name
            };
        }

        #region Nested type: PackageDecl

        private class PackageDecl : Package
        {
            public Package Update;
            public Package Upgrade;
        }

        private void LoadAutoInstallType()
        {
            UpdateChoice = _root.UpgradeOrUpdate.Element.Value == "Update"
                               ? UpdateChoice.AutoInstallJustUpdates
                               : UpdateChoice.AutoInstallJustUpdates;
        }
        
    
    

    
        #endregion
    }

    internal class FeedItem
    {
        public string Url { get; set; }
        public bool Failed { get; set; }
    }


#endif
}