using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using CoApp.Gui.Toolkit;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Toolkit.Extensions;
using CoApp.Toolkit.Logging;
using CoApp.Updater.Model;
using CoApp.Updater.Support;
using CoApp.Updater.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro;

namespace CoApp.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : CoAppApplication
    {
        private NotifyIcon _notifyIcon;
        private LocalServiceLocator loc = new LocalServiceLocator();

        public App() : base()
        {
            Messenger.Default.Register<BalloonToolTipMessage>(this, HandleToolTipMessage);
            //load possible assemblies
            var n = new GoToMessage();
            var a = new Accent();
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            
            
        }

        private void HandleToolTipMessage(BalloonToolTipMessage balloonToolTipMessage)
        {
            var realIcon = (ToolTipIcon)Enum.Parse(typeof(ToolTipIcon), balloonToolTipMessage.Icon.ToString());
            _notifyIcon.ShowBalloonTip(balloonToolTipMessage.TimeToDisplay, balloonToolTipMessage.Title, balloonToolTipMessage.Message, realIcon);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            Logger.Error(dispatcherUnhandledExceptionEventArgs.Exception);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = false;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            
            var quiet = false;
            var force = false;

            var options = e.Args.Switches();
            foreach (var arg in options.Keys)
            {
                var argumentParameters = options[arg];
                var last = argumentParameters.LastOrDefault();

                switch (arg)
                {
                    case "quiet":
                        Logger.Message("We're running quiet");
                        quiet = true;
                        break;
                    case "force":
                        Logger.Message("We will force this to run quietly.");
                        force = true;
                        break;
                }
            }

            if (quiet && !force)
            {
                var quietSupport = new QuietSupport();
                
                if (quietSupport.HandleScheduledTaskCall().Result)
                    return;
            }

            var mainWindow = new MainWindow();

            if (quiet)
            {
                mainWindow.Visibility = Visibility.Hidden;
                loc.AutomationService.TurnOnAutomation();
                if (!force)
                {
                    loc.UpdateService.SetScheduledTaskToRunNow();
                }
            }
            else
            {
                mainWindow.Visibility = Visibility.Visible;
            }

            if (!quiet && !loc.UpdateService.IsSchedulerSet.Result)
            {
                loc.NavigationService.GoTo(new ViewModelLocator().AskToCreateEventViewModel);
            }
            else
            {
                loc.NavigationService.GoTo(ViewModelLocator.UpdatingViewModelStatic);
            }


            

            Current.MainWindow = mainWindow;
            SetupNotifyIcon();
        }


        private void SetupNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
                              {
                                  Icon = Updater.Properties.Resources.CoApp,
                                  Visible = true,
                                  BalloonTipText = "CoApp Update"
                              };

            _notifyIcon.DoubleClick +=
                (o, e) =>
                    {
                        Current.MainWindow.Visibility = Visibility.Visible;
                        if (loc.AutomationService.IsAutomated)
                        {
                            loc.AutomationService.TurnOffAutomation();
                        }
                    };
        }
    }
}