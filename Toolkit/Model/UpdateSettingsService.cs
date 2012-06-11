using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support.Converters;
using CoApp.Packaging.Client;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;

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
            get { return CoApp.UpdateChoice; }
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
                CoApp.SetScheduledTask("coapp_update", @"c:\programdata\bin\CoApp.Updater.exe", "--quiet", hour, 0, UpdateDayOfWeekConverter.Convert(day), 60);
        }

        public Task<bool> AutoTrim
        {
            get { return CoApp.TrimOnUpdate; }
        }

        public Task SetAutoTrim(bool autotrim)
        {
            return CoApp.SetTrimOnUpdate(autotrim);
        }
        
        public Task SetDefaultScheduledTask()
        {
            return CoApp.SetScheduledTask("coapp_update", @"c:\programdata\bin\CoApp.Updater.exe", "--quiet", 3, 0, null,
                                          60);

        }
        
        private static ScheduledTask DefaultTask()
        {
            return new ScheduledTask { Name="coapp_update", CommandLine = "--quiet", 
                DayOfWeek = null, Executable = @"c:\programdata\bin\CoApp.Updater.exe", Hour = 3, IntervalInMinutes = 60, Minutes = 0, };
        }


        public Task SetTask(int hour, UpdateDayOfWeek day, bool autoTrim, UpdateChoice choice)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 CoApp.SetScheduledTask("coapp_update",
                                                                        @"c:\programdata\bin\CoApp.Updater.exe",
                                                                        "--quiet", 3, 0, null, 60);
                                                 CoApp.SetTrimOnUpdate(autoTrim);
                                                 CoApp.SetUpdateChoice(choice);
                                             }
                );

        }

        public Task SetTaskToDefault()
        {
            return SetTask(3, UpdateDayOfWeek.Everyday, CoAppService.DEFAULT_TRIM_ON_UPDATE,
                           CoAppService.DEFAULT_UPDATE_CHOICE);
        }

        public Task<UpdateTaskDTO> GetTask()
        {
            return Task.Factory.StartNew(() => GetTaskDTO(), TaskCreationOptions.AttachedToParent);
        }

        public Task<bool> IsTaskSet()
        {
            return CoApp.ScheduledTasks.ContinueAlways(t =>
            {
                t.RethrowWhenFaulted();
                return t.Result.Any(s => s.Name == "coapp_update");
            });
        }

        private Task<ScheduledTask> GetCoAppUpdateTask()
        {
            return CoApp.GetScheduledTask("coapp_update");
        }

        private UpdateTaskDTO GetTaskDTO()
        {
            var dto = new UpdateTaskDTO();
            try
            {
                dto.AutoTrim = CoApp.TrimOnUpdate.Result;
                
            }
            catch (Exception e )
            {
                Logger.Warning("Couldn't get TrimOnUpdate, {0}, {1}", e.Message, e.StackTrace);
                dto.AutoTrim = CoAppService.DEFAULT_TRIM_ON_UPDATE;
            }

            try
            {
                dto.UpdateChoice = CoApp.UpdateChoice.Result;
            }
            catch (Exception e )
            {
               
                Logger.Warning("Couldn't get UpdateChoice, {0}, {1}", e.Message, e.StackTrace);
                dto.UpdateChoice = CoAppService.DEFAULT_UPDATE_CHOICE;
            }


            var task = GetCoAppUpdateTask();
            var cont = task.ContinueAlways((task1) =>
                                               {
                                                   if (task1.IsFaulted)
                                                   {
                                                       var t = DefaultTask();

                                                       dto.Hour = t.Hour;
                                                       dto.DayOfWeek = UpdateDayOfWeekConverter.ConvertBack(t.DayOfWeek);
                                                   }
                                                   else
                                                   {
                                                       dto.Hour = task.Result.Hour;
                                                       dto.DayOfWeek =
                                                           UpdateDayOfWeekConverter.ConvertBack(task.Result.DayOfWeek);
                                                   }
                                               });

            cont.Wait();

  

            


            return dto;

        }
    }

  
}
