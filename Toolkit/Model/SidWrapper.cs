using System.Security.Principal;

namespace CoApp.Gui.Toolkit.Model
{
    public class SidWrapper
    {
        public SecurityIdentifier Sid;

        public virtual bool AreSame(SidWrapper other)
        {
            return Sid == other.Sid;
        }

        public virtual bool AreSame(SecurityIdentifier sid)
        {
            return Sid == sid;
        }
    }
}
