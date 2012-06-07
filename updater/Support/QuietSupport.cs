using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.Support.Converters;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model.Interfaces;
using FluentDateTime;
using LocalServiceLocator = CoApp.Updater.Model.LocalServiceLocator;

namespace CoApp.Updater.Support
{
    public class QuietSupport
    {
        internal IUpdateService Update;
        internal IUpdateSettingsService UpdateSettings;
        public QuietSupport()
        {
            var loc = new LocalServiceLocator();

            Update = loc.UpdateService;
            UpdateSettings = loc.UpdateSettingsService;
        }

        public Task<bool> HandleScheduledTaskCall()
        {
            var scheduledTask = UpdateSettings.UpdateTimeAndDay;
            var taskLastRun = Update.LastScheduledTaskRealRun;

            return Task.Factory.ContinueWhenAll(new Task[] {scheduledTask, taskLastRun}, (tasks) =>
                                                                                              {
                                                                                                  if (!scheduledTask.IsFaulted && !scheduledTask.IsFaulted)
                                                                                                  {
                                                                                                      var
                                                                                                          lastrunShouldHaveBeen
                                                                                                              =
                                                                                                              WhenLastRunShouldHaveBeen
                                                                                                                  (scheduledTask
                                                                                                                       .
                                                                                                                       Result,
                                                                                                                   DateTime
                                                                                                                       .
                                                                                                                       Now);
                                                                                                     
                                                                                                    if(!DoWeNeedToRun(taskLastRun.Result, lastrunShouldHaveBeen))
                                                                                                    {
                                                                                                      Logger.Message("Shutting down in quiet mode, it hasn't been long enough, Last run: {0}, Should have been {1}", taskLastRun.Result, lastrunShouldHaveBeen);

                                                                                                        Shutdown();
                                                                                                        return true;
                                                                                                    }
                                                                                                    
                                                                                                  }
                                                                                                  return false;
                                                                                                 
                                                                                              }


                );
        }


        internal bool DoWeNeedToRun(DateTime actualLastRun, DateTime scheduledLastRun)
        {
            return actualLastRun < scheduledLastRun;
        }

        /// <summary>
        /// internal for testing
        /// </summary>
        /// <param name="scheduledTimeAndDay"></param>
        /// <returns></returns>
        internal DateTime WhenLastRunShouldHaveBeen(UpdateTimeAndDay scheduledTimeAndDay, DateTime now)
        {

            var today = now.BeginningOfDay();

            if (scheduledTimeAndDay.DayOfWeek == UpdateDayOfWeek.Everyday)
            {
                if (now.Hour > scheduledTimeAndDay.Time)
                {
                    //it should have ranToday
                    return today.AddHours(scheduledTimeAndDay.Time);
                }
                else
                {
                    return today.Subtract(TimeSpan.FromDays(1)).AddHours(scheduledTimeAndDay.Time);
                    //it should have ran yesterday
                }
            }

            else
            {
                

                //it should run once a week
                var isToday = UpdateDayOfWeekConverter.Convert(scheduledTimeAndDay.DayOfWeek) == now.DayOfWeek;
                if (isToday)
                {
                    if (now.Hour < scheduledTimeAndDay.Time)
                    {
                        return today.Subtract(TimeSpan.FromDays(7)).AddHours(scheduledTimeAndDay.Time);
                    }
                    else
                    {
                        return today.AddHours(scheduledTimeAndDay.Time);    
                    }
                    
                }
                else
                {
                    var lastTime = today.Previous((DayOfWeek)UpdateDayOfWeekConverter.Convert(scheduledTimeAndDay.DayOfWeek));
                    lastTime = lastTime.SetHour(scheduledTimeAndDay.Time);
                    return lastTime;
                }
            }

        }


        //override this when unit testing HandleScheduledTaskCall()
        internal virtual void Shutdown()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => Application.Current.Shutdown()));
        }

        
    }


   
    
    

}
