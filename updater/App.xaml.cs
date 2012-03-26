using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using CoApp.Toolkit.Extensions;
using CoApp.Updater.Model;
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

        static App()
        {
            DispatcherHelper.Initialize();
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _notifyIcon.Visible = false;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            SetupNotifyIcon();
            var quiet = false;

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
                }
            }


            var mainWindow = new MainWindow();

            if (quiet)
            {
                mainWindow.Visibility = Visibility.Hidden;
                loc.AutomationService.TurnOnAutomation();
            }
            else
            {
                mainWindow.Visibility = Visibility.Visible;
                loc.InitializeService.SetInitializeAction(
                    () => loc.NavigationService.GoTo(ViewModelLocator.PrimaryViewModelStatic));
            }


            loc.NavigationService.GoTo(ViewModelLocator.UpdatingViewModelStatic);

            Current.MainWindow = mainWindow;
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