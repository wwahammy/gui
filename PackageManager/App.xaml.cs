using System.Windows;
using CoApp.Gui.Toolkit;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Model;
using CoApp.Gui.Toolkit.Support;
using CoApp.PackageManager.ViewModel;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro;

namespace CoApp.PackageManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : CoAppApplication
    {

        private LocalServiceLocator _loc = new LocalServiceLocator();
        private ViewModelLocator _vmLoc = new ViewModelLocator();

        public App()
        {
            
        }

        static App()
        {
       
            DispatcherHelper.Initialize();
            var n = new GoToMessage();
            var a = new Accent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            
            var mainWindow = new MainWindow();

            _loc.NavigationService.GoTo(_vmLoc.HomeViewModel);

            

            Current.MainWindow = mainWindow;
            Current.MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

    }
}
