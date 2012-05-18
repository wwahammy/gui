using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Configuration;
using CoApp.Packaging.Client;

using CoApp.Toolkit.Tasks;
using CoApp.Toolkit.Win32;
using CoApp.Toolkit.Extensions;

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
        private readonly RegistryView _coAppSystem = RegistryView.CoAppSystem;


        internal readonly PackageManager EPM;


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
            get { return EPM.Feeds.ContinueWith((a) => 
                a.Result.Where(f => !f.IsSession).Select(f => f.Location)); }
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


        public Task BlockPackage(string packageName)
        {
            return EPM.BlockPackage(packageName);
        }

        public Task UnblockPackage(string packageName)
        {
            return EPM.UnBlockPackage(packageName);
        }

        public Task<IEnumerable<Package>> GetPackages()
        {
            return EPM.QueryPackages("*");
        }

        public Task<IEnumerable<Package>> GetPackages(string packageName, FourPartVersion? minVersion = new FourPartVersion?(), FourPartVersion? maxVersion = new FourPartVersion?(), bool? dependencies = new bool?(), bool? installed = new bool?(), bool? active = new bool?(), bool? requested = new bool?(), bool? blocked = new bool?(), bool? latest = new bool?(), string locationFeed = null, bool? updates = new bool?(), bool? upgrades = new bool?(), bool? trimable = new bool?())
        {
            return null;
            //EPM.QU
           // return EPM.QueryPackages(packageName, minVersion, maxVersion, dependencies, installed, active, requested,
           //                        blocked, latest, locationFeed, updates, upgrades, trimable);
        }

        public Task<Package> GetPackage(string packageName)
        {
            return GetPackage(packageName, false);
        }

        public Task<Package> GetPackage(string packageName, bool withDetails)
        {
           

            return EPM.GetPackage(new CanonicalName(packageName)).ContinueAlways((Task<Package> t) =>
                                                            {
                                                                if (t.IsCanceled || t.IsFaulted)
                                                                {
                                                                    throw t.Exception.Unwrap();
                                                                }
                                                                

                                                                if (withDetails)
                                                                {
                                                                    return
                                                                        EPM.GetPackageDetails(t.Result).Result;
                                                                }

                                                                return t.Result;
                                                            }

                );

        }

        public Task<IEnumerable<Package>> GetAllVersionsOfPackage(Package p)
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
            return null;
           // return EPM.GetUpdatablePackages("*");
        }

        public Task<IEnumerable<Package>> GetUpgradablePackages()
        {
            return null;
            //return EPM.GetUpgradablePackages("*");
        }

        public Task<PackageSet> GetPackageSet(string canonicalName)
        {
            return EPM.GetPackageSet(canonicalName);
        }

        public Task<Package> GetPackageDetails(string canonicalName)
        {
            return EPM.GetPackageDetails(canonicalName);
        }

        public Task UpdateExistingPackage(string canonicalName, bool? autoUpgrade = null,
                                          Action<string, int, int> installProgress = null,
                                          Action<string> packageInstalled = null)
        {
            return
                Task.Factory.StartNew(() =>
                                          {
                                              CurrentTask.Events +=
                                                  new PackageInstallProgress((name, progress, overallProgress) => installProgress(name, progress,
                                                                                                                                  overallProgress));
                                              CurrentTask.Events += new PackageInstalled(name => packageInstalled(name));

                                              return EPM.UpdateExistingPackage(canonicalName, autoUpgrade).ContinueWith(
                                                  t => LastTimeInstalled = DateTime.Now);
                                          }
                    );
            
        }

        public Task UpgradeExistingPackage(string canonicalName, bool? autoUpgrade,
                                           Action<string, int, int> installProgress, Action<string> packageInstalled)
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
                                                     EPM.UpgradeExistingPackage(canonicalName, autoUpgrade).
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


        public Task<Package> GetPackageDetails(Package package)
        {
            return EPM.GetPackageDetails(package);
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

        public Task TrimAll()
        {
            return null;
            /*
            return Task.Factory.StartNew<Task>(() => EPM.GetTrimablePackages("*").ContinueWith(t =>
                                                                                             {
                                                                                                 var tasks =
                                                                                                     Enumerable.ToArray<Task>(t.Result.Select(
                                                                                                             p =>
                                                                                                             EPM.
                                                                                                                 RemovePackage
                                                                                                                 (
                                                                                                                     p.
                                                                                                                         CanonicalName,
                                                                                                                     false)));


                                                                                                 Task.WaitAll(tasks);
                                                                                                 var exceptions
                                                                                                     =
                                                                                                     tasks.
                                                                                                         FindAllExceptions
                                                                                                         ();
                                                                                                 if (exceptions.Length >
                                                                                                     0)
                                                                                                     throw new AggregateException
                                                                                                         (exceptions);
                                                                                             }));
             * */
        }

        public Task InstallPackage(string canonicalName, Action<string, int, int> installProgress, Action<string> packageInstalled)
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
                                              return EPM.InstallPackage(canonicalName);
                                          });
        }

        public Task RemovePackage(string canonicalName, Action<string, int> removeProgress, Action<string> packageRemoved)
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
    }
}