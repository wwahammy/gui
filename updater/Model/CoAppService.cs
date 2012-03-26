using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoApp.Toolkit.Configuration;
using CoApp.Toolkit.Engine.Client;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.Support;

namespace CoApp.Updater.Model
{
    public class CoAppService : ICoAppService
    {
        public const string UPDATECHOICE_KEYNAME = "preferences#updatechoice";
        public const string TRIM_ON_UPDATE_KEYNAME = "preferences#trim_on_update";

        public const string LASTTIMECHECKED_KEYNAME = "preferences#last_time_checked_for_updates";
        public const string LASTTIMEINSTALLED_KEYNAME = "preferences#last_time_installed_updates";

        public const UpdateChoice DEFAULT_UPDATE_CHOICE = UpdateChoice.AutoInstallAll;
        public const bool DEFAULT_TRIM_ON_UPDATE = false;
        private readonly RegistryView CoAppSystem = RegistryView.CoAppSystem;


        internal readonly EasyPackageManager EPM;


        public CoAppService()
        {
            //TODO downloadprogress, downloadcompleted
            EPM = new EasyPackageManager();
        }

        #region ICoAppService Members

        public Task<Policy> GetPolicy(PolicyType type)
        {
            return EPM.GetPolicy(type.ToString());
        }

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
            get { return EPM.Feeds.ContinueWith((a) => a.Result.Where(f => !f.IsSession).Select(f => f.Location)); }
        }

        public Task AddSystemFeed(string feedUrl)
        {
            return EPM.AddSystemFeed(feedUrl);
        }

        public Task RemoveSystemFeed(string feedUrl)
        {
            return null;
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
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Package>> GetUpdatablePackages()
        {
            return EPM.GetUpdatablePackages("*");
        }

        public Task<IEnumerable<Package>> GetUpgradablePackages()
        {
            return EPM.GetUpgradablePackages("*");
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
                EPM.UpdateExistingPackage(canonicalName, autoUpgrade, installProgress, packageInstalled).ContinueWith(
                    t => LastTimeInstalled = DateTime.Now);
        }

        public Task UpgradeExistingPackage(string canonicalName, bool? autoUpgrade,
                                           Action<string, int, int> installProgress, Action<string> packageInstalled)
        {
            return
                EPM.UpgradeExistingPackage(canonicalName, autoUpgrade, installProgress, packageInstalled).ContinueWith(
                    t => LastTimeInstalled = DateTime.Now);
        }

        public UpdateChoice UpdateChoice
        {
            get
            {
                if (!CoAppSystem[UPDATECHOICE_KEYNAME].HasValue)
                {
                    return DEFAULT_UPDATE_CHOICE;
                }
                return (UpdateChoice) Enum.Parse(typeof (UpdateChoice), CoAppSystem[UPDATECHOICE_KEYNAME].StringValue);
            }
            set { CoAppSystem[UPDATECHOICE_KEYNAME].StringValue = value.ToString(); }
        }


        public bool TrimOnUpdate
        {
            get
            {
                if (!CoAppSystem[TRIM_ON_UPDATE_KEYNAME].HasValue)
                {
                    return DEFAULT_TRIM_ON_UPDATE;
                }
                return CoAppSystem[UPDATECHOICE_KEYNAME].BoolValue;
            }
            set { CoAppSystem[UPDATECHOICE_KEYNAME].BoolValue = value; }
        }

        public DateTime? LastTimeInstalled
        {
            get
            {
                if (CoAppSystem[LASTTIMEINSTALLED_KEYNAME].HasValue)
                {
                    try
                    {
                        return DateTime.Parse(CoAppSystem[LASTTIMEINSTALLED_KEYNAME].StringValue);
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
                    CoAppSystem[LASTTIMEINSTALLED_KEYNAME].StringValue = ((DateTime) value).ToString("en-us");
                }
            }
        }

        public DateTime? LastTimeChecked
        {
            get
            {
                if (CoAppSystem[LASTTIMECHECKED_KEYNAME].HasValue)
                {
                    try
                    {
                        return DateTime.Parse(CoAppSystem[LASTTIMECHECKED_KEYNAME].StringValue);
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
                    CoAppSystem[LASTTIMECHECKED_KEYNAME].StringValue = ((DateTime) value).ToString("en-us");
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
            return Task.Factory.StartNew(() => EPM.GetTrimablePackages("*").ContinueWith(t =>
                                                                                             {
                                                                                                 var tasks =
                                                                                                     t.Result.Select(
                                                                                                         p =>
                                                                                                         EPM.
                                                                                                             RemovePackage
                                                                                                             (
                                                                                                                 p.
                                                                                                                     CanonicalName,
                                                                                                                 false))
                                                                                                         .ToArray();


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
        }

        #endregion
    }
}