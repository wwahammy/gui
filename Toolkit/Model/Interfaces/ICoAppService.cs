using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoApp.Toolkit.Engine.Client;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    public interface ICoAppService
    {
        Task<PolicyProxy> GetPolicy(PolicyType type);

        Task AddPrincipalToPolicy(PolicyType type, string principal);

        Task RemovePrincipalFromPolicy(PolicyType type, string principal);

        Task<IEnumerable<string>> SystemFeeds { get; }

        Task AddSystemFeed(string feedUrl);

        Task RemoveSystemFeed(string feedUrl);

        Task<IEnumerable<string>> SessionFeeds { get; }

        Task<bool> OptedIn { get; }

        Task SetOptedIn(bool optedIn);

        Task BlockPackage(string packageName);
        
        Task UnblockPackage(string packageName);

        Task<IEnumerable<Package>> GetPackages();

        Task<IEnumerable<Package>> GetUpdatablePackages();

        Task<IEnumerable<Package>> GetUpgradablePackages();

        Task<PackageSet> GetPackageSet(string canonicalName);

        Task<Package> GetPackageDetails(Package package);

        Task<Package> GetPackageDetails(string canonicalName);

        Task UpdateExistingPackage(string canonicalName, bool? autoUpgrade, Action<string, int, int> installProgress,
                                   Action<string> packageInstalled);

        Task UpgradeExistingPackage(string canonicalName, bool? autoUpgrade, Action<string, int, int> installProgress,
                                   Action<string> packageInstalled);

        UpdateChoice UpdateChoice { get; set; }

        bool TrimOnUpdate { get; set; }

        DateTime? LastTimeInstalled { get; }

        DateTime? LastTimeChecked { get; }

        Task SetAllFeedsStale();

        Task<ScheduledTask> GetScheduledTask(string name);

        Task SetScheduledTask(string name, string executable, string commandline, int hour, int minutes, DayOfWeek? dayOfWeek, int intervalInMinutes);


        Task RemoveScheduledTask(string name);

        Task TrimAll();
    }

    public class PolicyProxy
    {
        public string Description { get; set; }
        public string Name { get; set; }

        public IEnumerable<string> Members { get; set; }

        internal static PolicyProxy Convert(Policy p)
        {
            return new PolicyProxy {Description = p.Description, Name = p.Name, Members = p.Members};
        }
       
    }
}
