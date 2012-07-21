using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.Gui.Toolkit.Model
{
    public class PolicyService : IPolicyService
    {
// ReSharper disable InconsistentNaming
        private static readonly SecurityIdentifier WORLDSID = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        private readonly object _policyTaskLock = new object();
// ReSharper restore InconsistentNaming
        internal ICoAppService CoApp;
        internal IWindowsUserService UserService;

        private IEnumerable<PolicyProxy> _policies; 

      

        public PolicyService()
        {
            var loc = new LocalServiceLocator();
            CoApp = loc.CoAppService;
            CoApp.Elevated += CoAppOnElevated;

            UserService = loc.WindowsUserService;
            ReloadPolicies();
        }

        #region IPolicyService Members

        public void ReloadPolicies()
        {
            lock (_policyTaskLock)
            {
                _policies = CoApp.Policies.Result;
            }
        }


        public Task<PolicyResult> InstallPolicy
        {
            get
            {
                return Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.InstallPackage), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => CreatePolicyResultTask(t));
                    
            }
        }

        public Task<bool> CanChangeSettings
        {
            get
            {
              
                return
                    Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.ModifyPolicy), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => t.Result.IsEnabledForUser);
            }
        }

        public Task SetInstallPolicy(PolicyResult policyToSet)
        {
            return
                InstallPolicy.ContinueWith(t => ModifyPolicy(t, policyToSet, PolicyType.InstallPackage)).ContinueWith(
                    t =>
                        {
                            t.RethrowWhenFaulted();
                            Messenger.Default.Send(new PoliciesUpdatedMessage());
                        }
                    );
        }

        public Task<PolicyResult> UpdatePolicy
        {
            get
            {
               
                return
                   Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.UpdatePackage), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => CreatePolicyResultTask(t));
            }
        }

        public Task SetUpdatePolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> RemovePolicy
        {
            get
            {
                return
                  Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.RemovePackage), TaskCreationOptions.AttachedToParent).
                    ContinueWith(t =>
                                 CreatePolicyResultTask(t));
            }
        }

        public Task SetRemovePolicy(PolicyResult result)
        {
            return RemovePolicy.ContinueWith(t => ModifyPolicy(t, result, PolicyType.RemovePackage)).ContinueWith(
                t =>
                    {
                        t.RethrowWhenFaulted();
                        Messenger.Default.Send(new PoliciesUpdatedMessage());
                    }
                );
        }


        public Task<PolicyResult> SystemFeedsPolicy
        {
            get
            {
                return
                  Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.EditSystemFeeds), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => CreatePolicyResultTask(t));
            }
        }

        public Task SetSystemFeedsPolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<PolicyResult> SessionFeedsPolicy
        {
            get
            {
                return
                  Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.EditSessionFeeds), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => CreatePolicyResultTask(t));
            }
        }

        public Task SetSessionFeedsPolicy(PolicyResult result)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanInstall
        {
            get
            {
                return
                   Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.InstallPackage), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => t.Result.IsEnabledForUser);
            }
        }


        public Task<bool> CanUpdate
        {
            get
            {
                 return
                   Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.UpdatePackage), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => t.Result.IsEnabledForUser);
            }
        }

        public Task<bool> CanRemove
        {
            get
            {
                return
                  Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.RemovePackage), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => t.Result.IsEnabledForUser);
            }
        }


        public Task<bool> CanSetSessionFeeds
        {
            get
            {
                return
                  Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.EditSessionFeeds), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => t.Result.IsEnabledForUser);
            }
        }

        public Task<bool> CanSetSystemFeeds
        {
            get
            {
                return
                  Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.EditSystemFeeds), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => t.Result.IsEnabledForUser);
            }
        }

        public Task<bool> CanSetState
        {
            get
            {
                return
                Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.ChangeState), TaskCreationOptions.AttachedToParent).ContinueWith(t => t.Result.IsEnabledForUser);
            }
        }

        public string UserName
        {
            get { return Environment.UserName; }
        }

        public Task<PolicyResult> SetStatePolicy
        {
            get
            {
                return
                Task.Factory.StartNew(() => GetPolicyFromList(PolicyType.ChangeState), TaskCreationOptions.AttachedToParent).ContinueWith(
                        t => CreatePolicyResultTask(t));
            }

        }

     

        public Task SetSetStatePolicy(PolicyResult result)
        {

            return SetStatePolicy.ContinueWith(t => ModifyPolicy(t, result, PolicyType.ChangeState)).ContinueWith(
                t =>
                {
                    t.RethrowWhenFaulted();
                    Messenger.Default.Send(new PoliciesUpdatedMessage());
                });
        }

        #endregion

        private void CoAppOnElevated()
        {
            ReloadPolicies();
            Messenger.Default.Send(new PoliciesUpdatedMessage());
        }

      
        private PolicyProxy GetPolicyFromList( PolicyType type)
        {
            lock (_policyTaskLock)
            {
                return _policies.Single(p => p.Name == type.ToString());
            }
        }
        


        private PolicyResult CreatePolicyResult(IEnumerable<string> accounts)
        {
            IIdentity current = UserService.GetCurrentUser();

            if (current == null)
            {
                //apparently you can get null?
                return PolicyResult.Other;
            }
            var currentUser = UserService.GetUserSid(current);
            var output = PolicyResult.Other;
            foreach (var sid in accounts.Select(a => UserService.FindSid(a)))
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