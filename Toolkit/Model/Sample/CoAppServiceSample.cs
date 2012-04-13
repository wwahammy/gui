using System.IO;
using CoApp.Gui.Toolkit.Model.Interfaces;

using CoApp.Toolkit.Engine.Client;
using CoApp.Toolkit.Exceptions;
using CoApp.Toolkit.Win32;
using CoApp.Toolkit.Extensions;
namespace CoApp.Gui.Toolkit.Model.Sample
{
#if SAMPLEDATA
    using CoApp.Toolkit.DynamicXml;
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
        private readonly Dictionary<string,IList<PackageDecl>> _feedToPackages = new Dictionary<string,IList<PackageDecl>>();

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
                                                 var ps =
                                                     AllUpgradesAndUpdates.Where(p => p.CanonicalName == packageName);
                                                 if (ps.Any())
                                                 {
                                                     var first = ps.First();
                                                     first.IsBlocked = true;
                                                 }
                                             });
        }

        public Task UnblockPackage(string packageName)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var ps =
                                                     AllUpgradesAndUpdates.Where(p => p.CanonicalName == packageName);
                                                 if (ps.Any())
                                                 {
                                                     var first = ps.First();
                                                     first.IsBlocked = false;
                                                 }
                                             });
        }

        public Task<IEnumerable<Package>> GetPackages()
        {
            return Task.Factory.StartNew(() => _prods.Cast<Package>());
        }

        public Task<IEnumerable<Package>> GetPackages(
            string packageName, FourPartVersion? minVersion = new FourPartVersion?(), FourPartVersion? maxVersion = new FourPartVersion?(), bool? dependencies = new bool?(), bool? installed = new bool?(), bool? active = new bool?(), bool? requested = new bool?(), bool? blocked = new bool?(), bool? latest = new bool?(), string locationFeed = null, bool? updates = new bool?(), bool? upgrades = new bool?(), bool? trimable = new bool?())
        {

            return Task.Factory.StartNew(() =>
                                      {
                                          IEnumerable<PackageDecl> packages = null;
                                          if (packageName == "*")
                                              packages = _prods;

                                          if (installed != null)
                                              packages = packages.Where(p => p.IsInstalled == installed);

                                          if (locationFeed != null)
                                          {
                                              if (_feedToPackages.ContainsKey(locationFeed))
                                              {
                                                  packages =
                                                      packages.Where(p => _feedToPackages[locationFeed].Contains(p));
                                              }
                                              else
                                              {
                                                  packages = Enumerable.Empty<PackageDecl>();
                                              }
                                              
                                          }
                                          return packages.Cast<Package>();
                                      });

        }

        public Task<IEnumerable<Package>> GetUpdatablePackages()
        {
            return Task.Factory.StartNew(() => _prods.Where(p => p.Update != null && p.IsInstalled).Cast<Package>());
        }

        public Task<IEnumerable<Package>> GetUpgradablePackages()
        {
            return Task.Factory.StartNew(() => _prods.Where(p => p.Upgrade != null && p.IsInstalled).Cast<Package>());
        }

        public Task<PackageSet> GetPackageSet(string canonicalName)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var ret = new PackageSet();

                                                 var prod =
                                                     _prods.First(p => p.CanonicalName == canonicalName);

                                                 if (prod.Update != null)

                                                     ret.AvailableNewerCompatible = prod.Update;

                                                 if (prod.Upgrade != null)
                                                     ret.AvailableNewer = prod.Upgrade;

                                                 ret.Package = prod;
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
                                                 var prod = _prods.First(p => p.CanonicalName == canonicalName);
                                                 if (prod == null)
                                                     throw new Exception("this is totally invalid");

                                                 if (prod.Update.WillFailToInstall)
                                                     throw new CoAppException("Install");


                                                 for (var i = 0; i < 100; i++)
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
                                                 var prod = _prods.First(p => p.CanonicalName == canonicalName);
                                                 if (prod.Upgrade.WillFailToInstall)
                                                     throw new CoAppException("Install");
                                                 for (var i = 0; i < 100; i++)
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
           
            foreach (dynamic item in _root.Feeds)
            {
                if (item.Element.Name == "Feed")
                {
                    _sysFeeds.Add(item.Attributes.Url);
                    _feedToPackages[item.Attributes.Url] = new List<PackageDecl>();
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
            try
            {

            

            foreach (var item in _root.Products)
            {
                if (item.Element.Name == "Package")
                {
                    var attribs = item.Attributes;
                    var product = new PackageDecl
                                      {

                                          DisplayName = attribs.Has("DisplayName") ? attribs.DisplayName : null,
                                          Name = attribs.Name,
                                          Version = attribs.Version,
                                          Architecture = attribs.Architecture,
                                          PublicKeyToken = attribs.PublicKeyToken,
                                          IsInstalled = attribs.Has("Installed") ? bool.Parse(attribs.Installed) : false,
                                          AuthorVersion = attribs.Has("AuthorVersion") ? attribs.AuthorVersion : null,
                                          UpdateString = attribs.Has("UpdateFor") ? attribs.UpdateFor : null,
                                          UpgradeString = attribs.Has("UpgradeFor") ? attribs.UpgradeFor : null,
                                          PublishDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
                                      };


                    if (item.Attributes.Has("Summary"))
                    {
                        product.Summary = item.Attributes.Summary;
                    }

                    if (item.Attributes.Has("Icon"))
                    {
                        product.Icon =
                            Convert.ToBase64String(File.ReadAllBytes(item.Attributes.Icon));
                    }

                    if (item.Tags != null)
                    {
                        var l = new List<string>();
                        foreach (dynamic i in item.Tags)
                        {
                            l.Add(i.Element.Value);
                        }
                        product.Tags = l;
                    }

                    if (attribs.Has("Feed"))
                    {
                        _feedToPackages[attribs.Feed].Add(product);
                    }

                    if (attribs.Has("FailToInstall"))
                    {
                        product.WillFailToInstall = bool.Parse(attribs.FailToInstall);
                    }


                    _prods.Add(product);
                }
            }
                SetupCanonicalNames();

            SetupUpgradesAndUpdates();
        }

            catch (Exception e)
            {
                
                throw;
            }

        }

        private void SetupCanonicalNames()
        {
            foreach (var p in _prods)
            {
                p.CanonicalName = "{0}-{1}-{2}-{3}".format(p.Name, p.Version, p.Architecture, p.PublicKeyToken);

            }
        }

        private void SetupUpgradesAndUpdates()
        {
            foreach (var p in _prods)
            {
                if (p.UpdateString != null)
                {
                    foreach (var part in p.UpdateString.Split(','))
                    {
                        //find what this package updates!
                        var updatedPackage = _prods.FirstOrDefault(prod => prod.CanonicalName == part);
                        if (updatedPackage != null)
                        {
                            updatedPackage.Update = p;
                        }
                    }
                }

                if (p.UpgradeString != null)
                {
                    foreach (var part in p.UpgradeString.Split(','))
                    {
                        //find what this package updates!
                        var updatedPackage = _prods.FirstOrDefault(prod => prod.CanonicalName == part);
                        if (updatedPackage != null)
                        {
                            updatedPackage.Upgrade = p;
                        }
                    }
                }
            }
        }

        /*
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
        }*/

    #region Nested type: PackageDecl

        private class PackageDecl : Package
        {
            public PackageDecl Update;
            public PackageDecl Upgrade;

            internal bool WillFailToInstall;


            internal string UpdateString;
            internal string UpgradeString;

            public override string ToString()
            {
                return CanonicalName;
            }
            
        }

        private void LoadAutoInstallType()
        {
            UpdateChoice = _root.UpgradeOrUpdate.Element.Value == "Update"
                               ? UpdateChoice.AutoInstallJustUpdates
                               : UpdateChoice.AutoInstallJustUpdates;
        }
        
    
    

    
        #endregion


        public Task<IEnumerable<PolicyProxy>> Policies
        {
            get
            {
                return
                    Task.Factory.StartNew(
                        () =>
                        _membersForPolicies.Keys.Select(
                            type => new PolicyProxy {Name = type.ToString(), Members = _membersForPolicies[type]}))
                            ;
            }
        }
    }

    internal class FeedItem
    {
        public string Url { get; set; }
        public bool Failed { get; set; }
    }


#endif
}