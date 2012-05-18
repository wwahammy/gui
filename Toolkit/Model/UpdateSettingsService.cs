using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support.Converters;
using CoApp.Packaging.Client;

namespace CoApp.Gui.Toolkit.Model
{
    public class UpdateSettingsService : IUpdateSettingsService
    {
        internal ICoAppService CoApp;

        public UpdateSettingsService()
        {
            CoApp = new LocalServiceLocator().CoAppService;
        }

        public Task<UpdateChoice> UpdateChoice
        {
            get { return Task.Factory.StartNew(() => CoApp.UpdateChoice, TaskCreationOptions.AttachedToParent); }
        }

        public Task SetUpdateChoice(UpdateChoice choice)
        {
            return Task.Factory.StartNew(() => CoApp.UpdateChoice = choice, TaskCreationOptions.AttachedToParent);
        }

        public Task<UpdateTimeAndDay> UpdateTimeAndDay
        {
            get
            {
                return
                    CoApp.GetScheduledTask("coapp_update").ContinueWith(t => t.IsFaulted || t.IsCanceled || t.Result == null
                                                                                 ? DefaultTask()
                                                                                 : t.Result)
                                                                                 .ContinueWith(
                        t =>

                        new UpdateTimeAndDay { DayOfWeek = UpdateDayOfWeekConverter.ConvertBack(t.Result.DayOfWeek), Time = t.Result.Hour });
            }
        }


        public Task SetUpdateTimeAndDay(int hour, UpdateDayOfWeek day)
        {
            return
                CoApp.SetScheduledTask("coapp_update", "", "--quiet", hour, 0, UpdateDayOfWeekConverter.Convert(day), 60);
        }

        public Task<bool> AutoTrim
        {
            get { return Task.Factory.StartNew(() => CoApp.TrimOnUpdate); }
        }

        public Task SetAutoTrim(bool autotrim)
        {
            return Task.Factory.StartNew(() => CoApp.TrimOnUpdate = autotrim);
        }
        
        
        private static ScheduledTask DefaultTask()
        {
            return new ScheduledTask { CommandLine = "--quiet", DayOfWeek = null, Executable = "Coapp.Update", Hour = 3, IntervalInMinutes = 60, Minutes = 0, Name = "coapp_update" };
        }
    }
}
