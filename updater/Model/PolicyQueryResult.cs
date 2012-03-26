using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Updater.Model
{
    public class PolicyQueryResult
    {
        public string NameOfPolicy;
        public IEnumerable<string> GroupsInPolicy;
    }
}
