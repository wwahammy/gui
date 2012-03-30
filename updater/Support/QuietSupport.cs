using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using CoApp.Updater.Support.Converters;
using FluentDateTime;

namespace CoApp.Updater.Support
{
    public class QuietSupport
    {
        internal IUpdateService Update;
        public QuietSupport()
        {
            var loc = new LocalServiceLocator();

            Update = loc.UpdateService;
        }

        public Task<bool> HandleScheduledTaskCall()
        {
            var scheduledTask = Update.UpdateTimeAndDay;
            var taskLastRun = Update.LastScheduledTaskRealRun;

            return Task.Factory.ContinueWhenAll(new Task[] {scheduledTask, taskLastRun}, (tasks) =>
                                                                                              {
                                                                                                  if (!scheduledTask.IsFaulted && !scheduledTask.IsFaulted)
                                                                                                  {
                                                                                                    if(!DoWeNeedToRun(taskLastRun.Result, WhenLastRunShouldHaveBeen(scheduledTask.Result, DateTime.Now)))
                                                                                                    {
                                                                                                      Logger.Message("Shutting down in quiet mode, it hasn't been long enough");

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
