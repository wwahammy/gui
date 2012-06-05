using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using CoApp.Toolkit.Extensions;

namespace CoApp.Gui.Toolkit.Model
{
    public class PolicyService : IPolicyService
    {
// ReSharper disable InconsistentNaming
        private static readonly SecurityIdentifier WORLDSID = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
// ReSharper restore InconsistentNaming
        internal ICoAppService CoApp;
        internal IWindowsUserService UserService;

        private readonly Task<IEnumerable<PolicyProxy>> _policyTask;

        public PolicyService()
        {
            var loc = new LocalServiceLocator();
            CoApp = loc.CoAppService;
            UserService = loc.WindowsUserService;
            _policyTask = CoApp.Policies;
        }

        

        #region IPolicyService Members

        public Task<PolicyResult> InstallPolicy
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.InstallPackage)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        private PolicyProxy GetPolicyFromTask(IEnumerable<PolicyProxy> policies , PolicyType type)
        {
            return policies.Single(p => p.Name == type.ToString());
        }

        public Task<bool> CanChangeSettings
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ModifyPolicy)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task SetInstallPolicy(PolicyResult policyToSet)
        {
            return
                InstallPolicy.ContinueWith(t => ModifyPolicy(t, policyToSet, PolicyType.InstallPackage)).ContinueWith(
                    t => {
                             t.RethrowWhenFaulted();
                        Messenger.Default.Send(new PoliciesUpdatedMessage());
                    }


    );
        }

        public Task<PolicyResult> UpdatePolicy
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.UpdatePackage)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetUpdatePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> RemovePolicy
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.RemovePackage)).
                    ContinueWith(t =>
                                 CreatePolicyResultTask(t)); }
        }

        public Task SetRemovePolicy(PolicyResult result)
        {
            return RemovePolicy.ContinueWith(t => ModifyPolicy(t, result, PolicyType.RemovePackage)).ContinueWith(
                    t =>
                    {
                        t.RethrowWhenFaulted();
                        Messenger.Default.Send(new PoliciesUpdatedMessage());
                    }


    ); ;
        }

        public Task<PolicyResult> BlockPolicy
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ChangeBlockedState)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetBlockPolicy(PolicyResult result)
        {
            return BlockPolicy.ContinueWith(t => ModifyPolicy(t, result, PolicyType.ChangeBlockedState))
                .ContinueWith(
                    t =>
                    {
                        t.RethrowWhenFaulted();
                        Messenger.Default.Send(new PoliciesUpdatedMessage());
                    }


    ); ;
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
            get { return  _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ChangeActivePackage)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetActivePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> RequirePolicy
        {
            get { return  _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ChangeRequiredState)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetRequirePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> SystemFeedsPolicy
        {
            get { return  _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.EditSystemFeeds)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetSystemFeedsPolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> SessionFeedsPolicy
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.EditSessionFeeds)).ContinueWith(t => CreatePolicyResultTask(t)); }
        }

        public Task SetSessionFeedsPolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanInstall
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.InstallPackage)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public string UserName
        {
            get { return Environment.UserName; }
        }


        public Task<bool> CanUpdate
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.UpdatePackage)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanRemove
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.RemovePackage)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanRequire
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ChangeRequiredState)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanSetActive
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ChangeActivePackage)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanFreeze
        {
            get { throw new NotImplementedException(); }
        }

        public Task<bool> CanBlock
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.ChangeBlockedState)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanSetSessionFeeds
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.EditSessionFeeds)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        public Task<bool> CanSetSystemFeeds
        {
            get { return _policyTask.ContinueWith(t => GetPolicyFromTask(t.Result, PolicyType.EditSystemFeeds)).ContinueWith(t => VerifyActionPermission(t)); }
        }

        #endregion

        private bool VerifyActionPermission(Task<PolicyProxy> taskResult)
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


            foreach (SidWrapper sid in accounts.Select(a => UserService.FindSid(a)))
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
                if (sid.AreSame(WORLDSID))
                {
                    output = PolicyResult.Everyone;
                    break;
                }

                if (sid.AreSame(currentUser))
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


            var currentPolicy = currentPolicyTask.Result;
            var currentUser = UserService.GetCurrentUser();
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

        private PolicyResult CreatePolicyResultTask(Task<PolicyProxy> taskIn)
        {
            if (taskIn.IsFaulted)
            {
                throw new Exception(); //create properexception
            }
            return
                CreatePolicyResult(
                    taskIn.Result.Members);
        }
    }
}