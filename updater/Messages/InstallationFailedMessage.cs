using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Toolkit.Exceptions;
using CoApp.Updater.Model;
using CoApp.Updater.Support;

namespace CoApp.Updater.Messages
{
    public class InstallationFailedMessage
    {
        public IEnumerable<UpdateFailureException> Exceptions { get; set; }
    }
}

