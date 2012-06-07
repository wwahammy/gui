using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Gui.Toolkit.Support
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class CanDisableCommandAttribute : Attribute
    {
    }
}
