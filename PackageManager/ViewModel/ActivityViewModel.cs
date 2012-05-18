using System.Collections.ObjectModel;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.PackageManager.Messages;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;

namespace CoApp.PackageManager.ViewModel
{
    public class ActivityViewModel : ScreenViewModel
    {
        internal IActivityService Activity;
        private ObservableCollection<Activity> _activities;

        public ActivityViewModel()
        {
            Title = "Activities";
            Activity = new LocalServiceLocator().ActivityService;
            MessengerInstance.Register<ActivitiesUpdatedMessage>(this, ActivitiesChanged);
            Loaded += OnLoaded;
        }

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