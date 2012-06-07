using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Command;
using LocalServiceLocator = CoApp.Updater.Model.LocalServiceLocator;

namespace CoApp.Updater.ViewModel
{
    public class AskToCreateEventViewModel : ScreenViewModel
    {
        internal IUpdateService UpdateService;
        internal ICoAppService CoAppService;
        public AskToCreateEventViewModel()
        {
            Title = "Automatically update CoApp packages?";
            UpdateService = new LocalServiceLocator().UpdateService;
            CoAppService = new LocalServiceLocator().CoAppService;
            ElevateSetScheduledTask = new RelayCommand(RunElevateSetScheduleTask);
            DontScheduleTask = new RelayCommand(Leave);
            SetScheduledTask = new RelayCommand(() =>
            {
                UpdateService.SetDefaultScheduledTask().ContinueWith(t =>
                {
                    if
                        (
                        t
                            .
                            IsCanceled ||
                        t
                            .
                            IsFaulted)
                    {
                        Logger
                            .
                            Error
                            ("We couldn't set update task. {0} {1}",
                             t
                                 .
                                 Exception
                                 .
                                 Message,
                             t
                                 .
                                 Exception
                                 .
                                 StackTrace);

                        MessengerInstance
                            .
                            Send
                            (new MetroDialogBoxMessage
                            {
                                Title
                                    =
                                    "We couldn't set update task",
                                Content
                                    =
                                    "The update task couldn't be set",
                                Buttons
                                    =
                                    new ObservableCollection
                                    <
                                    ButtonDescription
                                    >
                                                                                                                                              {
                                                                                                                                                  new ButtonDescription
                                                                                                                                                      {
                                                                                                                                                          IsCancel
                                                                                                                                                              =
                                                                                                                                                              true,
                                                                                                                                                          Title
                                                                                                                                                              =
                                                                                                                                                              "Continue"
                                                                                                                                                      }
                                                                                                                                              }
                            });
                    }

                   Leave();
                });


            });
        }

        private void Leave()
        {
          
            var loc = new LocalServiceLocator();
            var vmLoc = new ViewModelLocator();
            loc.NavigationService.GoTo(vmLoc.UpdatingViewModel);

        }


        public ICommand SetScheduledTask { get; set; }
        public ICommand ElevateSetScheduledTask { get; set; }

        public ICommand DontScheduleTask { get; set; }

        private void RunElevateSetScheduleTask()
        {
            var elevate = CoAppService.Elevate();


            elevate.ContinueOnFail(e => RunElevateFailed(e));
            elevate.Continue(() => SetScheduledTask.Execute(null));

        }

        private void RunElevateFailed(Exception e)
        {
            Logger.Warning("Elevation Failed {0}, {1}", e.Message, e.StackTrace);
            MessengerInstance
                .
                Send
                (new MetroDialogBoxMessage
                {
                    Title
                        =
                        "Permission problem",
                    Content
                        =
                        "You don't have permission to set a scheduled task. Sorry :(",
                    Buttons
                        =
                        new ObservableCollection
                        <
                        ButtonDescription
                        >
                                  {
                                      new ButtonDescription
                                          {
                                              IsCancel
                                                  =
                                                  true,
                                              Title
                                                  =
                                                  "Continue"
                                          }
                                     }
                });

            Leave();

        }
    }
}
