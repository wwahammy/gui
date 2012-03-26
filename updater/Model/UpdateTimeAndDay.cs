using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Updater.Model.Interfaces;

namespace CoApp.Updater.Model
{
    public class UpdateTimeAndDay
    {
        public int Time { get; set; }
        public UpdateDayOfWeek DayOfWeek { get; set; }
    }
}
