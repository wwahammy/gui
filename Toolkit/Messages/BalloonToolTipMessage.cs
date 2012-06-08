using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Gui.Toolkit.Messages
{
    public class BalloonToolTipMessage
    {
        public int TimeToDisplay { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public BalloonToolTipIcon Icon { get; set; }
        
    }

    public enum BalloonToolTipIcon
    {
        None,
        Info,
        Warning,
        Error
    }
}
