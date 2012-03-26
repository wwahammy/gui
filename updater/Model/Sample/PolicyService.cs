using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model.Sample
{
#if SAMPLEDATA
    public class PolicyService : IPolicyService
    {
        private static readonly SecurityIdentifier ADMINSID =
            new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);

        private static readonly SecurityIdentifier WORLDSID = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

        #region Policies

        private PolicyResult _activePolicy = PolicyResult.Other;
        private PolicyResult _blockPolicy = PolicyResult.Other;
        private PolicyResult _freezePolicy = PolicyResult.Other;
        private PolicyResult _installPolicy = PolicyResult.Other;
        private PolicyResult _removePolicy = PolicyResult.Other;
        private PolicyResult _requirePolicy = PolicyResult.Other;
        private PolicyResult _sessionFeedsPolicy = PolicyResult.Other;
        private PolicyResult _sysFeedsPolicy = PolicyResult.Other;
        private PolicyResult _updatePolicy = PolicyResult.Other;

        #endregion

        internal ICoAppService CoApp;

        public PolicyService()
        {
            CoApp = new LocalServiceLocator().CoAppService;
        }

        #region IPolicyService Members

        public Task<PolicyResult> InstallPolicy
        {
            get { return Task.Factory.StartNew(() => _installPolicy); }
        }

        public Task<bool> CanChangeSettings
        {
            get { return Task.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.ModifyPolicy)); }
        }

        public Task SetInstallPolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _installPolicy = result);
        }

        public Task<PolicyResult> UpdatePolicy
        {
            get { return Task.Factory.StartNew(() => _updatePolicy); }
        }

        public Task SetUpdatePolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _updatePolicy = result);
        }

        public Task<PolicyResult> RemovePolicy
        {
            get { return Task.Factory.StartNew(() => _removePolicy); }
        }

        public Task SetRemovePolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _removePolicy = result);
        }

        public Task<PolicyResult> BlockPolicy
        {
            get { return Task.Factory.StartNew(() => _blockPolicy); }
        }

        public Task SetBlockPolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _blockPolicy = result);
        }

        public Task<PolicyResult> FreezePolicy
        {
            get { return Task.Factory.StartNew(() => _freezePolicy); }
        }

        public Task SetFreezePolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _freezePolicy = result);
        }

        public Task<PolicyResult> ActivePolicy
        {
            get { return Task.Factory.StartNew(() => _activePolicy); }
        }

        public Task SetActivePolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _activePolicy = result);
        }

        public Task<PolicyResult> RequirePolicy
        {
            get { return Task.Factory.StartNew(() => _requirePolicy); }
        }

        public Task SetRequirePolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _requirePolicy = result);
        }

        public Task<PolicyResult> SystemFeedsPolicy
        {
            get { return Task.Factory.StartNew(() => _sysFeedsPolicy); }
        }

        public Task SetSystemFeedsPolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _sysFeedsPolicy = result);
        }

        public Task<PolicyResult> SessionFeedsPolicy
        {
            get { return Task.Factory.StartNew(() => _sessionFeedsPolicy); }
        }

        public Task SetSessionFeedsPolicy(PolicyResult result)
        {
            return Task.Factory.StartNew(() => _sysFeedsPolicy = result);
        }

        public string UserName
        {
            get { return Environment.UserName; }
        }

        public Task<bool> CanInstall
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.InstallPackage)); }
        }


        public Task<bool> CanUpdate
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.UpdatePackage)); }
        }

        public Task<bool> CanRemove
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.RemovePackage)); }
        }

        public Task<bool> CanRequire
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.ChangeRequiredState)); }
        }

        public Task<bool> CanSetActive
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.ChangeActivePackage)); }
        }

        public Task<bool> CanFreeze
        {
            get { throw new NotImplementedException(); }
        }

        public Task<bool> CanBlock
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.ChangeBlockedState)); }
        }

        public Task<bool> CanSetSessionFeeds
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.EditSessionFeeds)); }
        }

        public Task<bool> CanSetSystemFeeds
        {
            get { return Task<bool>.Factory.StartNew(() => CanCurrentUserPerformAction(PolicyType.EditSystemFeeds)); }
        }

        #endregion

        private static bool CanCurrentUserPerformAction(PolicyType type)
        {
            if (
                type == PolicyType.InstallPackage || type == PolicyType.RemovePackage  || type == PolicyType.ModifyPolicy)
            {
                WindowsIdentity current = WindowsIdentity.GetCurrent();

                if (current == null)
                {
                    //apparently you can get null?
                    return false;
                }


                var groupsForCurrentUser = current.Groups;


                if (groupsForCurrentUser != null && groupsForCurrentUser.Any((i) => i == ADMINSID))
                {
                    return true;
                }


                return false;
            }
            return true;
        }


        private PolicyResult CreatePolicyResult(IEnumerable<string> accounts)
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();

            if (current == null)
            {
                //apparently you can get null?
                return PolicyResult.Other;
            }
            SecurityIdentifier currentUser = current.User;
            PolicyResult output = PolicyResult.Other;
            foreach (SecurityIdentifier sid in accounts.Select(FindSid))
            {
                if (sid.Equals(WORLDSID))
                {
                    output = PolicyResult.Everyone;
                    break;
                }

                if (sid.Equals(currentUser))

                {
                    output = PolicyResult.CurrentUser;
                }
            }

            return output;
        }

        private SecurityIdentifier FindSid(string account)
        {
            SecurityIdentifier sid = null;
            try
            {
                // first, let's try this as a sid (SDDL) string
                sid = new SecurityIdentifier(account);
                return sid;
            }
            catch
            {
            }

            try
            {
                // maybe it's an account/group name
                var name = new NTAccount(account);
                sid = (SecurityIdentifier) name.Translate(typeof (SecurityIdentifier));
                if (sid != null)
                {
                    return sid;
                }
            }
            catch
            {
            }

            return null;
        }
    }
#endif
}