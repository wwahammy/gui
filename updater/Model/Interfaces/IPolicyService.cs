using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoApp.Updater.Model.Interfaces
{
    public interface IPolicyService
    {
        Task<PolicyResult> InstallPolicy { get; }

        Task<PolicyResult> UpdatePolicy { get; }

        Task<PolicyResult> RemovePolicy { get; }

        Task<PolicyResult> BlockPolicy { get; }

        Task<PolicyResult> FreezePolicy { get; }

        Task<PolicyResult> ActivePolicy { get; }

        Task<PolicyResult> RequirePolicy { get; }

        Task<PolicyResult> SystemFeedsPolicy { get; }

        Task<PolicyResult> SessionFeedsPolicy { get; }

        Task<bool> CanInstall { get; }
        Task<bool> CanUpdate { get; }
        Task<bool> CanRemove { get; }
        Task<bool> CanRequire { get; }
        Task<bool> CanSetActive { get; }
        Task<bool> CanFreeze { get; }
        Task<bool> CanBlock { get; }

        Task<bool> CanSetSessionFeeds { get; }
        Task<bool> CanSetSystemFeeds { get; }

        Task<bool> CanChangeSettings { get; }

        Task SetInstallPolicy(PolicyResult result);
        Task SetUpdatePolicy(PolicyResult result);
        Task SetRemovePolicy(PolicyResult result);
        Task SetBlockPolicy(PolicyResult result);
        Task SetFreezePolicy(PolicyResult result);
        Task SetActivePolicy(PolicyResult result);
        Task SetRequirePolicy(PolicyResult result);
        Task SetSystemFeedsPolicy(PolicyResult result);
        Task SetSessionFeedsPolicy(PolicyResult result);

        string UserName { get; }

    }

    public enum PolicyResult
    {
        Other,
        CurrentUser,
        Everyone
    }

    public enum PolicyType
    {
        Connect,
        EnumeratePackages,
        InstallPackage,
        UpdatePackage,
        RemovePackage,
        FreezePackage,
        ChangeActivePackage,
        ChangeRequiredState,
        ChangeBlockedState,
        EditSystemFeeds,
        EditSessionFeeds,
        ModifyPolicy
    }

    
}