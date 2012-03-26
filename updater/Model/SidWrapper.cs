using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace CoApp.Updater.Model
{
    public class SidWrapper
    {
        public SecurityIdentifier Sid;

        public virtual bool AreSame(SidWrapper other)
        {
            return Sid == other.Sid;
        }
    }
}
