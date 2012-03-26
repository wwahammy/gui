#if SAMPLEDATA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoApp.Toolkit.DynamicXml;
using CoApp.Toolkit.Engine.Client;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model.Sample
{
    public class CoAppServiceSample : ICoAppService
    {
        private const int MsToWait = 30;
        private readonly List<PackageDecl> _prods = new List<PackageDecl>();
        private readonly dynamic _root;
        private readonly List<string> _sysFeeds = new List<string>();
        private bool _autoTrim = true;
        private bool _optedIn = true;

        private UpdateChoice _updateChoice = UpdateChoice.AutoInstallAll;
        private UpdateDayOfWeek _updateDayOfWeek = UpdateDayOfWeek.Everyday;
        private int _updateTime = 3;

        public CoAppServiceSample()
        {
            //AllPossibleProducts = new Dictionary<Product, bool>();

            _root = new DynamicNode(XDocument.Load("SampleData.xml"));
            LoadFeeds();
            LoadPackages();
            LoadLastInstalled();

            LoadAutoInstallType();
           
        }

        private IEnumerable<Package> AllUpgradesAndUpdates
        {
            get { return _prods.Select(p => p.Update).Concat(_prods.Select(p => p.Upgrade)); }
        }

        #region ICoAppService Members

        public Task<Policy> GetPolicy(PolicyType type)
        {
            throw new NotImplementedException();
        }

        public Task AddPrincipalToPolicy(PolicyType type, string principal)
        {
            throw new NotImplementedException();
        }

        public Task RemovePrincipalFromPolicy(PolicyType type, string principal)
        {
            throw new NotImplementedException();
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
                                                     _prods.Where(p => p.CanonicalName == canonicalName).First();

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
                                                     Thread.Sleep(MsToWait);
                                                     installProgress.Invoke(prod.Update.CanonicalName, i, i);
                                                 }

                                                 packageInstalled.Invoke(prod.Update.CanonicalName);
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
                                                     Thread.Sleep(MsToWait);
                                                     installProgress.Invoke(prod.Upgrade.CanonicalName, i, i);
                                                 }

                                                 packageInstalled.Invoke(prod.Upgrade.CanonicalName);
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
            throw new NotImplementedException();
        }

        public Task SetScheduledTask(string name, string executable, string commandline, int hour, int minutes,
                                     DayOfWeek? dayOfWeek, int intervalInMinutes)
        {
            throw new NotImplementedException();
        }

        public Task RemoveScheduledTask(string name)
        {
            throw new NotImplementedException();
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
                                var product = new PackageDecl()
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
                PublishDate = DateTime.Today.ToString(),
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
}

#endif