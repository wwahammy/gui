using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CoApp.Toolkit.Engine.Client;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model
{
    public class PolicyService : IPolicyService
    {
        private static readonly SecurityIdentifier WORLDSID = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        internal ICoAppService CoApp;
        internal IWindowsUserService UserService;

        public PolicyService()
        {
            var loc = new LocalServiceLocator();
            CoApp = loc.CoAppService;
            UserService = loc.WindowsUserService;
        }

        #region IPolicyService Members

        public Task<PolicyResult> InstallPolicy
        {
            get { return CoApp.GetPolicy(PolicyType.InstallPackage).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task<bool> CanChangeSettings
        {
            get { return CoApp.GetPolicy(PolicyType.ModifyPolicy).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task SetInstallPolicy(PolicyResult policyToSet)
        {
            return InstallPolicy.ContinueWith(t => ModifyPolicy(t, policyToSet, PolicyType.InstallPackage));
        }

        public Task<PolicyResult> UpdatePolicy
        {
            get { return CoApp.GetPolicy(PolicyType.UpdatePackage).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetUpdatePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> RemovePolicy
        {
            get
            {
                return CoApp.GetPolicy(PolicyType.RemovePackage).
                    ContinueWith(t =>
                                 CreatePolicyResultTask(t));
            }
        }

        public Task SetRemovePolicy(PolicyResult result)
        {
            return RemovePolicy.ContinueWith(t => ModifyPolicy(t, result, PolicyType.RemovePackage));
        }

        public Task<PolicyResult> BlockPolicy
        {
            get { return CoApp.GetPolicy(PolicyType.ChangeBlockedState).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetBlockPolicy(PolicyResult result)
        {
            return BlockPolicy.ContinueWith(t => ModifyPolicy(t, result, PolicyType.ChangeBlockedState));
        }

        public Task<PolicyResult> FreezePolicy
        {
            get { throw new NotImplementedException(); }
        }

        public Task SetFreezePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> ActivePolicy
        {
            get { return CoApp.GetPolicy(PolicyType.ChangeActivePackage).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetActivePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> RequirePolicy
        {
            get { return CoApp.GetPolicy(PolicyType.ChangeRequiredState).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetRequirePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> SystemFeedsPolicy
        {
            get { return CoApp.GetPolicy(PolicyType.EditSystemFeeds).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetSystemFeedsPolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> SessionFeedsPolicy
        {
            get { return CoApp.GetPolicy(PolicyType.EditSessionFeeds).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetSessionFeedsPolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanInstall
        {
            get { return CoApp.GetPolicy(PolicyType.InstallPackage).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public string UserName
        {
            get { return Environment.UserName; }
        }


        public Task<bool> CanUpdate
        {
            get { return CoApp.GetPolicy(PolicyType.UpdatePackage).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanRemove
        {
            get { return CoApp.GetPolicy(PolicyType.RemovePackage).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanRequire
        {
            get { return CoApp.GetPolicy(PolicyType.ChangeRequiredState).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanSetActive
        {
            get { return CoApp.GetPolicy(PolicyType.ChangeActivePackage).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanFreeze
        {
            get { throw new NotImplementedException(); }
        }

        public Task<bool> CanBlock
        {
            get { return CoApp.GetPolicy(PolicyType.ChangeBlockedState).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanSetSessionFeeds
        {
            get { return CoApp.GetPolicy(PolicyType.EditSessionFeeds).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanSetSystemFeeds
        {
            get { return CoApp.GetPolicy(PolicyType.EditSystemFeeds).ContinueWith(t => VerifyActionPermission(t)); }
        }

        #endregion

        private bool VerifyActionPermission(Task<Policy> taskResult)
        {
            if (taskResult.IsFaulted)
            {
                return false;
            }

            return
                CanCurrentUserPerformAction(
                    taskResult.Result.Members);
        }

        private bool CanCurrentUserPerformAction(IEnumerable<string> accounts)
        {
            IIdentity current = UserService.GetCurrentUser();

            if (current == null)
            {
                //apparently you can get null?
                return false;
            }
            SidWrapper currentUser = UserService.GetUserSid(current);


            foreach (SidWrapper sid in accounts.Select((a) => UserService.FindSid(a)))
            {
                if (sid.Equals(WORLDSID))
                {
                    return true;
                }

                if (sid.Equals(currentUser))
                {
                    return true;
                }


                return UserService.IsSIDInUsersGroup(current, sid);
            }

            return false;
        }


        private PolicyResult CreatePolicyResult(IEnumerable<string> accounts)
        {
            IIdentity current = UserService.GetCurrentUser();

            if (current == null)
            {
                //apparently you can get null?
                return PolicyResult.Other;
            }
            SidWrapper currentUser = UserService.GetUserSid(current);
            PolicyResult output = PolicyResult.Other;
            foreach (SidWrapper sid in accounts.Select(a => UserService.FindSid(a)))
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

        private void ModifyPolicy(Task<PolicyResult> currentPolicyTask, PolicyResult newPolicy, PolicyType policyToSet)
        {
            if (currentPolicyTask.IsFaulted)
                throw new Exception();


            PolicyResult currentPolicy = currentPolicyTask.Result;
            IIdentity currentUser = UserService.GetCurrentUser();
            //get the current install policy
            if (currentPolicy == PolicyResult.Everyone)
            {
                if (newPolicy == PolicyResult.CurrentUser)
                {
                    //remove everyone
                    CoApp.RemovePrincipalFromPolicy(policyToSet, WORLDSID.Value);

                    //add user

                    CoApp.AddPrincipalToPolicy(policyToSet, currentUser.Name);
                }
                else if (newPolicy == PolicyResult.Everyone)
                {
                    //do nothing
                }
                else if (newPolicy == PolicyResult.Other)
                {
                    //remove everyone
                    CoApp.RemovePrincipalFromPolicy(policyToSet, WORLDSID.Value);
                }
            }

            else if (currentPolicy == PolicyResult.CurrentUser)
            {
                if (newPolicy == PolicyResult.CurrentUser)
                {
                    //do nothing
                }
                else if (newPolicy == PolicyResult.Everyone)
                {
                    //remove CurrentUser
                    CoApp.RemovePrincipalFromPolicy(policyToSet, currentUser.Name);
                    //add everyone
                    CoApp.AddPrincipalToPolicy(policyToSet, WORLDSID.Value);
                }
                else if (newPolicy == PolicyResult.Other)
                {
                    //remove CurrentUser
                    CoApp.RemovePrincipalFromPolicy(policyToSet, currentUser.Name);
                }
            }

            else if (currentPolicy == PolicyResult.Other)
            {
                if (newPolicy == PolicyResult.CurrentUser)
                {
                    //add currentuser
                    CoApp.AddPrincipalToPolicy(policyToSet, currentUser.Name);
                }
                else if (newPolicy == PolicyResult.Everyone)
                {
                    //add everyone
                    CoApp.AddPrincipalToPolicy(policyToSet, WORLDSID.Value);
                }
                else if (newPolicy == PolicyResult.Other)
                {
                    //do nothing
                }
            }
        }

        private PolicyResult CreatePolicyResultTask(Task<Policy> taskIn)
        {
            if (taskIn.IsFaulted)
            {
                throw new Exception(); //create properexception
            }
            else
            {
                return
                    CreatePolicyResult(
                        taskIn.Result.Members);
            }
        }
    }
}