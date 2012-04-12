using System.Windows;
using CoApp.Gui.Toolkit.Model;
using CoApp.PackageManager.ViewModel;
using GalaSoft.MvvmLight.Threading;

namespace CoApp.PackageManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private LocalServiceLocator _loc = new LocalServiceLocator();
        private ViewModelLocator _vmLoc = new ViewModelLocator();
        static App()
        {
            DispatcherHelper.Initialize();
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
