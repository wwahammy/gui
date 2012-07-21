using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Gui.Toolkit.ViewModels.Modal;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model;
using CoApp.Updater.Model.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CoApp.Updater.ViewModel
{
    public class AskToCreateEventViewModel : ScreenViewModel
    {
        internal ICoAppService CoAppService;
        internal IUpdateService UpdateService;

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


                                                                                                                         var
                                                                                                                             modalModel
                                                                                                                                 = new BasicModalViewModel
                                                                                                                                       {
                                                                                                                                           Title
                                                                                                                                               =
                                                                                                                                               "We couldn't set update task",
                                                                                                                                           Content
                                                                                                                                               =
                                                                                                                                               "The update task couldn't be set",
                                                                                                                                       };
                                                                                                                         modalModel
                                                                                                                             .
                                                                                                                             SetViaButtonDescriptions
                                                                                                                             (new List
                                                                                                                                  <
                                                                                                                                  ButtonDescription
                                                                                                                                  >
                                                                                                                                  {
                                                                                                                                      new ButtonDescription
                                                                                                                                          {
                                                                                                                                              Title
                                                                                                                                                  =
                                                                                                                                                  "Continue"
                                                                                                                                          }
                                                                                                                                  });


                                                                                                                         MessengerInstance
                                                                                                                             .
                                                                                                                             Send
                                                                                                                             (
                                                                                                                                 new MetroDialogBoxMessage(modalModel)
                                                                                                                             );
                                                                                                                     }

                                                                                                                     Leave
                                                                                                                         ();
                                                                                                                 });
                                                    });
        }


        public ICommand SetScheduledTask { get; set; }
        public ICommand ElevateSetScheduledTask { get; set; }

        public ICommand DontScheduleTask { get; set; }

        private void Leave()
        {
            var loc = new LocalServiceLocator();
            var vmLoc = new ViewModelLocator();
            loc.NavigationService.GoTo(vmLoc.UpdatingViewModel);
        }

        private void RunElevateSetScheduleTask()
        {
            Task elevate = CoAppService.Elevate();


            elevate.ContinueOnFail(e => RunElevateFailed(e));
            elevate.Continue(() => SetScheduledTask.Execute(null));
        }

        private void RunElevateFailed(Exception e)
        {
            Logger.Warning("Elevation Failed {0}, {1}", e.Message, e.StackTrace);
            var modalModel = new BasicModalViewModel
                                 {
                                     Title
                                         =
                                         "Permission problem",
                                     Content
                                         =
                                         "You don't have permission to set a scheduled task. Sorry :(",
                                 };
            modalModel.SetViaButtonDescriptions(new List<ButtonDescription>
                                                    {
                                                        new ButtonDescription
                                                            {
                                                                Title
                                                                    =
                                                                    "Continue"
                                                            }
                                                    });
            MessengerInstance
                .
                Send
                (new MetroDialogBoxMessage(modalModel));

            Leave();
        }
    }
}