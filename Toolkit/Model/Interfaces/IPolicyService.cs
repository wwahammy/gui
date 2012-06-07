using System.Threading.Tasks;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    public interface IPolicyService
    {
        Task<PolicyResult> InstallPolicy { get; }

        Task<PolicyResult> UpdatePolicy { get; }

        Task<PolicyResult> RemovePolicy { get; }

        Task<PolicyResult> SetStatePolicy { get; }

        Task<PolicyResult> SystemFeedsPolicy { get; }

        Task<PolicyResult> SessionFeedsPolicy { get; }

        Task<bool> CanInstall { get; }
        Task<bool> CanUpdate { get; }
        Task<bool> CanRemove { get; }

        Task<bool> CanSetState { get; }

        Task<bool> CanSetSessionFeeds { get; }
        Task<bool> CanSetSystemFeeds { get; }

        Task<bool> CanChangeSettings { get; }

        Task SetInstallPolicy(PolicyResult result);
        Task SetUpdatePolicy(PolicyResult result);
        Task SetRemovePolicy(PolicyResult result);

        Task SetSetStatePolicy(PolicyResult result);
        Task SetSystemFeedsPolicy(PolicyResult result);
        Task SetSessionFeedsPolicy(PolicyResult result);


        void ReloadPolicies();
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
        ChangeState,
        EditSystemFeeds,
        EditSessionFeeds,
        ModifyPolicy
    }

    
}