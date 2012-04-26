using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using CoApp.Toolkit.Extensions;
using CoApp.Updater.Model;
using CoApp.Updater.Support;
using CoApp.Updater.ViewModel;
using GalaSoft.MvvmLight.Threading;

namespace CoApp.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private NotifyIcon _notifyIcon;
        private LocalServiceLocator loc = new LocalServiceLocator();

        public App() : base()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (!args.Name.Contains(".resources,"))
            {
                AssemblyName name = new AssemblyName(args.Name);
                name.SetPublicKeyToken(null);
                //return Assembly.Load(name);
                //Assembly.Load()
            }
            return null;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            throw new NotImplementedException();
        }

        static App()
        {
            
            DispatcherHelper.Initialize();
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
                        quiet = true;
                        break;
                    case "force":
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


            loc.NavigationService.GoTo(ViewModelLocator.UpdatingViewModelStatic);

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