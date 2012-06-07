using System.Security.Principal;

namespace CoApp.Gui.Toolkit.Model.Interfaces
{
    public interface IWindowsUserService
    {
        SidWrapper FindSid(string account);
        
        IIdentity GetCurrentUser();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">If this isn't really a Windows Principal, and you're not mocking this out, it won't work</param>
        /// <returns></returns>
        SidWrapper GetUserSid(IIdentity user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">If this isn't really a Windows Principal, and you're not mocking this out, it won't work</param>
        /// <returns></returns>
        object GroupsForUser(IIdentity user);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">If this isn't really a Windows Principal, and you're not mocking this out, it won't work</param>
        /// <param name="sidToCheck"></param>
        bool IsSIDInUsersGroup(IIdentity user, SidWrapper sidToCheck);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">If this isn't really a Windows Principal, and you're not mocking this out, it won't work</param>
        /// <param name="sidToCheck"></param>
        bool IsSIDInUsersGroup(IIdentity user, string sidToCheck);

        


    }
}