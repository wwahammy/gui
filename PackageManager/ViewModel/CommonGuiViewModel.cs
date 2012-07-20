using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.ViewModels;
using CoApp.Gui.Toolkit.ViewModels.Modal;
using CoApp.PackageManager.Messages;
using CoApp.PackageManager.Model;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.ViewModel
{
    public abstract class CommonGuiViewModel : ScreenViewModel
    {
        internal IActivityService Activity;


        private int _numberOfActivities;

        private int _numberOfActivityFailures;

        protected CommonGuiViewModel()
        {
            var loc = new LocalServiceLocator();

            Activity = loc.ActivityService;
            var nav = loc.NavigationService;
            GoToActivities = new RelayCommand(() => nav.GoTo(new ViewModelLocator().ActivityViewModel));
            Loaded += OnLoaded;
            Messenger.Default.Register<ActivitiesUpdatedMessage>(this, true, ActivitiesUpdated);
        }

        public ICommand GoToActivities { get; set; }

        private void OnLoaded()
        {
            var activities = Activity.Activities;
            var activitiesCount = activities.Count(a => a.State == State.Performing);
            UpdateOnUI(() => NumberOfActivities = activitiesCount);
            var activitiesFailures = activities.Count(a => a.State == State.Failed);
            UpdateOnUI(() => NumberOfActivityFailures = activitiesFailures);
            var activitiesFinished = activities.Count(a => a.State == State.Finished);
            UpdateOnUI(() => NumberOfActivitiesFinished = activitiesFinished);

        }

        public int NumberOfActivities
        {
            get { return _numberOfActivities; }
            set
            {
                _numberOfActivities = value;
                RaisePropertyChanged("NumberOfActivities");
            }
        }

        private int _numberOfActivitiesFinished;

        public int NumberOfActivitiesFinished
        {
            get { return _numberOfActivitiesFinished; }
            set
            {
                _numberOfActivitiesFinished = value;
                RaisePropertyChanged("NumberOfActivitiesFinished");
            }
        }

        

        public int NumberOfActivityFailures
        {
            get { return _numberOfActivityFailures; }
            set
            {
                _numberOfActivityFailures = value;
                RaisePropertyChanged("NumberOfActivityFailures");
            }
        }

        private void ActivitiesUpdated(ActivitiesUpdatedMessage activitiesUpdatedMessage)
        {            
            UpdateOnUI(() => NumberOfActivities = activitiesUpdatedMessage.NumberOfActivities);
            UpdateOnUI(() => NumberOfActivityFailures = activitiesUpdatedMessage.NumberOfFailures);
            UpdateOnUI(() => NumberOfActivitiesFinished = activitiesUpdatedMessage.NumberFinished);
        }

        protected static MetroDialogBoxMessage NotEnoughPermissions(string action)
        {
            var vm = new BasicModalViewModel { Title = "Permissions problem", Content = "You don't have permission to {0}.".format(action) };
            vm.SetViaButtonDescriptions(new List<ButtonDescription>
                                            {
                                                new ButtonDescription
                                                    {
                                                        Title
                                                            =
                                                            "Continue"
                                                    }
                                            });

            return new MetroDialogBoxMessage(vm);
        }
    }
}