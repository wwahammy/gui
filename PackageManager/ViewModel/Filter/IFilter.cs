using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.PackageManager.ViewModel.Filter
{
    public interface IFilter
    {
        FrictionlessFilter Create();
    }
}
