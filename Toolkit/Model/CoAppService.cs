using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Configuration;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Toolkit.Tasks;

namespace CoApp.Gui.Toolkit.Model
{
    public class CoAppService : ICoAppService
    {
        public const string UPDATECHOICE_KEYNAME = "preferences#updatechoice";
        public const string TRIM_ON_UPDATE_KEYNAME = "preferences#trim_on_update";

        public const string LASTTIMECHECKED_KEYNAME = "preferences#last_time_checked_for_updates";
        public const string LASTTIMEINSTALLED_KEYNAME = "preferences#last_time_installed_updates";

        public const UpdateChoice DEFAULT_UPDATE_CHOICE = UpdateChoice.AutoInstallAll;
        public const bool DEFAULT_TRIM_ON_UPDATE = false;


        internal readonly PackageManager EPM;
        private readonly RegistryView _coAppSystem = RegistryView.CoAppSystem;


        public CoAppService()
        {
            //TODO downloadprogress, downloadcompleted
            EPM = new PackageManager();
        }

        #region ICoAppService Members

        public Task AddPrincipalToPolicy(PolicyType type, string principal)
        {
            return EPM.AddToPolicy(type.ToString(), principal);
        }

        public Task RemovePrincipalFromPolicy(PolicyType type, string principal)
        {
            return EPM.RemoveFromPolicy(type.ToString(), principal);
        }


        public Task<IEnumerable<string>> SystemFeeds
        {
            get
            {
                return EPM.Feeds.ContinueWith((a) =>
                                              a.Result.Where(f => !f.IsSession).Select(f => f.Location));
            }
        }

        public Task AddSystemFeed(string feedUrl)
        {
            return EPM.AddSystemFeed(feedUrl);
        }

        public Task RemoveSystemFeed(string feedUrl)
        {
            return EPM.RemoveSystemFeed(feedUrl);
        }

        public Task<IEnumerable<string>> SessionFeeds
        {
            get { throw new NotImplementedException(); }
        }

        public Task<bool> OptedIn
        {
            get { return EPM.GetTelemetry(); }
        }

        public Task SetOptedIn(bool optedIn)
        {
            return EPM.SetTelemetry(optedIn);
        }


        public Task BlockPackage(CanonicalName packageName)
        {
            return EPM.SetGeneralPackageInformation(50, packageName, "state", PackageState.Blocked.ToString());
        }

        public Task UnblockPackage(CanonicalName packageName)
        {
            return EPM.SetGeneralPackageInformation(50, packageName, "state", null);
        }


        public Task<IEnumerable<Package>> GetPackages(Expression<Func<IPackage, bool>> pkgFilter = null,
                                                      Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>>
                                                          collectionFilter = null, bool withDetails = false,
                                                      string locationFeed = null)
        {
            return GetPackages("coapp:*", pkgFilter, collectionFilter, withDetails, locationFeed);
        }

        public Task<IEnumerable<Package>> GetPackages(CanonicalName packageName,
                                                      Expression<Func<IPackage, bool>> pkgFilter = null,
                                                      Expression<Func<IEnumerable<IPackage>, IEnumerable<IPackage>>>
                                                          collectionFilter = null, bool withDetails = false,
                                                      string locationFeed = null)
        {
            return EPM.FindPackages(packageName, pkgFilter, collectionFilter, locationFeed)
                .ContinueWith(t =>
                                  {
                                      t.RethrowWhenFaulted();
                                      if (withDetails)
                                      {
                                          return
                                              GetPackagesWithDetails
                                                  (t.Result);
                                      }

                                      return t.Result;
                                  }
                );
        }

        public Task<Package> GetPackage(CanonicalName packageName)
        {
            return GetPackage(packageName, false);
        }

        public Task<Package> GetPackage(CanonicalName packageName, bool withDetails)
        {
            return withDetails ? EPM.GetPackageDetails(packageName) : EPM.GetPackage(packageName);
        }


        public Task<IEnumerable<Package>> GetAllVersionsOfPackage(IPackage p)
        {
            return EPM.GetAllVersionsOfPackage(p.CanonicalName);
        }


        public Task<IEnumerable<PolicyProxy>> Policies
        {
            get
            {
                return EPM.Policies.ContinueWith(t =>
                                                     {
                                                         t.RethrowWhenFaulted();
                                                         return t.Result.Select(PolicyProxy.Convert);
                                                     });
            }
        }

        public Task<IEnumerable<Package>> GetUpdatablePackages()
        {
            return GetPackages(pkgFilter: Package.Filters.PackagesWithUpdateAvailable, withDetails: true);
            // return EPM.GetUpdatablePackages("*");
        }

        public Task<IEnumerable<Package>> GetUpgradablePackages()
        {
            return GetPackages(Package.Filters.PackagesWithUpgradeAvailable, null, true);
            //return EPM.GetUpgradablePackages("*");
        }

        public Task<Package> GetPackageDetails(CanonicalName canonicalName)
        {
            return EPM.GetPackageDetails(canonicalName);
        }

        public Task UpdateExistingPackage(CanonicalName canonicalName, bool? autoUpgrade,
                                          Action<string, int, int> installProgress, Action<string> packageInstalled,
                                          CanonicalName packageToUpdateFrom)
        {
            return
                Task.Factory.StartNew(() =>
                                          {
                                              CurrentTask.Events +=
                                                  new PackageInstallProgress(
                                                      (name, progress, overallProgress) =>
                                                      installProgress(name, progress,
                                                                      overallProgress));
                                              //CurrentTask.Events += new PackageInstalled(name => packageInstalled(name));

                                              return EPM.Install(canonicalName, autoUpgrade,
                                                                 replacingPackage: packageToUpdateFrom).ContinueWith(
                                                                     t => LastTimeInstalled = DateTime.Now);
                                          }
                    );
        }

        public Task UpgradeExistingPackage(CanonicalName canonicalName, bool? autoUpgrade,
                                           Action<string, int, int> installProgress, Action<string> packageInstalled,
                                           CanonicalName packageToUpgradeFrom)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 CurrentTask.Events +=
                                                     new PackageInstallProgress(
                                                         (name, progress, overallProgress) =>
                                                         installProgress(name, progress,
                                                                         overallProgress));
                                                 CurrentTask.Events +=
                                                     new PackageInstalled(name => packageInstalled(name));
                                                 return
                                                     EPM.Install(canonicalName, autoUpgrade,
                                                                 replacingPackage: packageToUpgradeFrom).
                                                         ContinueWith(
                                                             t => LastTimeInstalled = DateTime.Now);
                                             });
        }

        public UpdateChoice UpdateChoice
        {
            get
            {
                if (!_coAppSystem[UPDATECHOICE_KEYNAME].HasValue)
                {
                    return DEFAULT_UPDATE_CHOICE;
                }
                return (UpdateChoice) Enum.Parse(typeof (UpdateChoice), _coAppSystem[UPDATECHOICE_KEYNAME].StringValue);
            }
            set { _coAppSystem[UPDATECHOICE_KEYNAME].StringValue = value.ToString(); }
        }


        public bool TrimOnUpdate
        {
            get
            {
                if (!_coAppSystem[TRIM_ON_UPDATE_KEYNAME].HasValue)
                {
                    return DEFAULT_TRIM_ON_UPDATE;
                }
                return _coAppSystem[UPDATECHOICE_KEYNAME].BoolValue;
            }
            set { _coAppSystem[UPDATECHOICE_KEYNAME].BoolValue = value; }
        }

        public DateTime? LastTimeInstalled
        {
            get
            {
                if (_coAppSystem[LASTTIMEINSTALLED_KEYNAME].HasValue)
                {
                    try
                    {
                        return DateTime.Parse(_coAppSystem[LASTTIMEINSTALLED_KEYNAME].StringValue);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return null;
            }

            private set
            {
                if (value != null)
                {
                    _coAppSystem[LASTTIMEINSTALLED_KEYNAME].StringValue = ((DateTime) value).ToString("en-us");
                }
            }
        }

        public DateTime? LastTimeChecked
        {
            get
            {
                if (_coAppSystem[LASTTIMECHECKED_KEYNAME].HasValue)
                {
                    try
                    {
                        return DateTime.Parse(_coAppSystem[LASTTIMECHECKED_KEYNAME].StringValue);
                    }
                    catch
                    {
                        return null;
                    }
                }
                return null;
            }

            private set
            {
                if (value != null)
                {
                    _coAppSystem[LASTTIMECHECKED_KEYNAME].StringValue = ((DateTime) value).ToString("en-us");
                }
            }
        }


        public Task SetAllFeedsStale()
        {
            return EPM.SetAllFeedsStale();
        }

        public Task<ScheduledTask> GetScheduledTask(string name)
        {
            return EPM.GetScheduledTask(name);
        }

        public Task SetScheduledTask(string name, string executable, string commandline, int hour, int minutes,
                                     DayOfWeek? dayOfWeek, int intervalInMinutes)
        {
            return EPM.AddScheduledTask(name, executable, commandline, hour, minutes, dayOfWeek, intervalInMinutes);
        }

        public Task RemoveScheduledTask(string name)
        {
            return EPM.RemoveScheduledTask(name);
        }

        public Task<int> TrimAll()
        {
            return GetPackages(Package.Filters.Trimable).ContinueWith(
                t =>
                    {
                        if (t.IsCanceled || t.IsFaulted)
                        {
                            Logger.Warning(t.Exception.Unwrap());
                            t.RethrowWhenFaulted();
                        }

                        Task<int> task = EPM.RemovePackages(t.Result.Cast<CanonicalName>(), false);
                        task.ContinueOnFail(e => Logger.Warning(e.Unwrap()));
                        return task.Result;
                    });
        }

        public Task InstallPackage(CanonicalName canonicalName, Action<string, int, int> installProgress,
                                   Action<string> packageInstalled)
        {
            return
                Task.Factory.StartNew(() =>
                                          {
                                              CurrentTask.Events +=
                                                  new PackageInstallProgress(
                                                      (name, progress, overallProgress) =>
                                                      installProgress(name, progress,
                                                                      overallProgress));
                                              CurrentTask.Events += new PackageInstalled(name => packageInstalled(name));
                                              return EPM.Install(canonicalName);
                                          });
        }

        public Task RemovePackage(CanonicalName canonicalName, Action<string, int> removeProgress,
                                  Action<string> packageRemoved)
        {
            return
                Task.Factory.StartNew(() =>
                                          {
                                              CurrentTask.Events +=
                                                  new PackageRemoveProgress(
                                                      (name, progress) =>
                                                      removeProgress(name, progress));
                                              CurrentTask.Events += new PackageRemoved(name => packageRemoved(name));
                                              return EPM.RemovePackage(canonicalName, false);
                                          });
        }

        #endregion

        private IList<Package> GetPackagesWithDetails(IEnumerable<Package> packages)
        {
            //doing this so hell doesn't break loose
            return packages.Select(p => GetPackageDetails(p).Result).ToList();
        }

        public Task<Package> GetPackageDetails(Package package)
        {
            return EPM.GetPackageDetails(package);
        }
    }
}