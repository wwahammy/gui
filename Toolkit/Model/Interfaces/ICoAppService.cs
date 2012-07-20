using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Linq;
using CollectionFilter = CoApp.Toolkit.Collections.XList<System.Linq.Expressions.Expression<System.Func<System.Collections.Generic.IEnumerable<CoApp.Packaging.Common.IPackage>, System.Collections.Generic.IEnumerable<CoApp.Packaging.Common.IPackage>>>>;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    public interface ICoAppService
    {

        Task<IEnumerable<PolicyProxy>>  Policies { get; } 

        Task AddPrincipalToPolicy(PolicyType type, string principal);

        Task RemovePrincipalFromPolicy(PolicyType type, string principal);

        Task<IEnumerable<Feed>> SystemFeeds { get; }

        Task AddSystemFeed(string feedUrl);

        Task RemoveSystemFeed(string feedUrl);

        Task<Feed> GetSystemFeed(string feedUrl);

        Task<IEnumerable<string>> SessionFeeds { get; }

        Task<bool> OptedIn { get; }

        Task SetOptedIn(bool optedIn);


        Task SetState(CanonicalName packageName, PackageState state);

        Task BlockPackage(CanonicalName packageName);
        
        Task UnblockPackage(CanonicalName packageName);

        Task<IEnumerable<Package>> GetPackages(Expression<Func<IPackage, bool>> pkgFilter = null, CollectionFilter collectionFilter = null, bool withDetails = false, string locationFeed = null);

        Task<IEnumerable<Package>> GetPackages(CanonicalName packageName, Expression<Func<IPackage, bool>> pkgFilter = null, CollectionFilter collectionFilter = null, bool withDetails = false, string locationFeed = null);

        Task<Package> GetPackage(CanonicalName packageName);
        
        Task<Package> GetPackage(CanonicalName packageName, bool withDetails);

        Task<IEnumerable<Package>> GetAllVersionsOfPackage(IPackage p);

        Task<IEnumerable<Package>> GetUpdatablePackages();

        Task<IEnumerable<Package>> GetUpgradablePackages();

        Task<Package> GetPackageDetails(CanonicalName canonicalName);

        Task UpdateExistingPackage(CanonicalName canonicalName, bool? autoUpgrade, Action<string, int, int> installProgress,
                                   Action<string> packageInstalled, CanonicalName packageToUpdateFrom);

        Task UpgradeExistingPackage(CanonicalName canonicalName, bool? autoUpgrade, Action<string, int, int> installProgress,
                                   Action<string> packageInstalled, CanonicalName packageToUpdateFrom);

        Task<UpdateChoice> UpdateChoice { get;}

        Task SetUpdateChoice(UpdateChoice choice);

        Task<bool> TrimOnUpdate { get;}

        Task SetTrimOnUpdate(bool trim);

        DateTime? LastTimeInstalled { get; }

        DateTime? LastTimeChecked { get; }

        Task SetAllFeedsStale();

        Task SetFeedStale(string feedLocation);

        Task<ScheduledTask> GetScheduledTask(string name);

        Task SetScheduledTask(string name, string executable, string commandline, int hour, int minutes, DayOfWeek? dayOfWeek, int intervalInMinutes);
        /*
        Task GetCoAppUpdaterTask();
        Task SetCoAppUpdaterTask(int hour, int minutes, DayOfWeek? dayOfWeek )*/
        Task RemoveScheduledTask(string name);

        Task<int> TrimAll();

        Task InstallPackage(CanonicalName canonicalName, Action<string, int, int> installProgress,
                            Action<string> packageInstalled);

        Task<IEnumerable<Package>> IdentifyPackageAndDependenciesToInstall(Package package, bool withUpgrade = false, bool ignoreThisPackage = true, bool getDetails = true);
        

        Task RemovePackage(CanonicalName canonicalName, Action<string, int> removeProgress, Action<string> packageRemoved);

        Task Elevate();

        event Action Elevated;

        Task<IEnumerable<ScheduledTask>> ScheduledTasks { get; }
    }

    public class PolicyProxy
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public IEnumerable<string> Members { get; set; }
        public bool IsEnabledForUser { get; set; }

        internal static PolicyProxy Convert(Policy p)
        {
            return new PolicyProxy {Description = p.Description, Name = p.Name, Members = p.Members, IsEnabledForUser = p.IsEnabled};
        }
       
    }
}
