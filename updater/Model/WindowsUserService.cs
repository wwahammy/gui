using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model
{
    public class WindowsUserService : IWindowsUserService
    {
        public SidWrapper FindSid(string account)
        {
            SecurityIdentifier sid = null;
            try
            {
                // first, let's try this as a sid (SDDL) string
                sid = new SecurityIdentifier(account);

                return new SidWrapper { Sid = sid};
            }
            catch
            {
            }

            try
            {
                // maybe it's an account/group name
                var name = new NTAccount(account);
                sid = (SecurityIdentifier)name.Translate(typeof(SecurityIdentifier));
                if (sid != null)
                {
                    return new SidWrapper { Sid = sid };
                }
            }
            catch
            {
            }

            return null;
        }

        public IIdentity GetCurrentUser()
        {
            return WindowsIdentity.GetCurrent();
        }

        public SidWrapper GetUserSid(IIdentity user)
        {
            var winId = (WindowsIdentity) user;

            return new SidWrapper { Sid = winId.User};
        }

        public object GroupsForUser(IIdentity user)
        {
            var winId = (WindowsIdentity) user;
            return winId.Groups;
        }

        public bool IsSIDInUsersGroup(IIdentity user, SidWrapper sidToCheck)
        {
            var winId = (WindowsIdentity) user;
            return winId.Groups.Any(g => g == sidToCheck.Sid);
        }

        public bool IsSIDInUsersGroup(IIdentity user, string sidToCheck)
        {
            throw new NotImplementedException();
        }
    }
}
