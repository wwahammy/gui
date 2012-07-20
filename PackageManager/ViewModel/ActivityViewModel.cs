using System.Collections.ObjectModel;
using System.Windows.Input;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Messages;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CoApp.PackageManager.ViewModel
{
    public class ActivityViewModel : CommonGuiViewModel
    {
        
        private ObservableCollection<Activity> _activities;

        public ActivityViewModel()
        {
            Title = "Activities";
            //Activity = new LocalServiceLocator().ActivityService;
            MessengerInstance.Register<ActivitiesUpdatedMessage>(this, ActivitiesChanged);
            Remove = new RelayCommand<Activity>(ExecuteRemove);

            
            Loaded += OnLoaded;

        }

        private void ExecuteRemove(Activity activityToRemove)
        {
            Activity.RemoveActivity(activityToRemove);
        }

        public ICommand Remove { get; set; }

        public ObservableCollection<Activity> Activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
                RaisePropertyChanged("Activities");
            }
        }

        private void ActivitiesChanged(ActivitiesUpdatedMessage activitiesUpdatedMessage)
        {
            UpdateOnUI(() => Activities = new ObservableCollection<Activity>(Activity.Activities));
        }

        private void OnLoaded()
        {
            UpdateOnUI(() => Activities = new ObservableCollection<Activity>(Activity.Activities));
        }
    }
}