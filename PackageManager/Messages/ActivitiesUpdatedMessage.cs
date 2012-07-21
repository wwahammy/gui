using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.PackageManager.Messages
{
    public class ActivitiesUpdatedMessage
    {
        public int NumberOfActivities { get; set; }
        public int NumberOfFailures { get; set; }
        public int NumberFinished { get; set; }
    }
}
