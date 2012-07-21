using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CoApp.Gui.Toolkit.Controls;
using CoApp.Gui.Toolkit.Messages;
using CoApp.Gui.Toolkit.Support;
using CoApp.Gui.Toolkit.ViewModels.Modal;
using CoApp.PackageManager.Messages;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IActivityService
    {
        Task InstallPackage(IPackage p);
        Task RemovePackage(IPackage p);

        Task SetState(IPackage p, PackageState state);

        void RemoveActivity(Activity a);

        IList<Activity> Activities { get; }
    }

    public class Activity : ObservableObject
    {
        public Activity(IPackage p, ActivityType a)
        {
            Package = p;
            PackageName = p.GetNicestName();
            ActivityType = a;
            State = State.Performing;
            SeeError = new RelayCommand(ExecuteSeeError, () => State == State.Failed);
        }

        private void ExecuteSeeError()
        {
            var vm = new BasicModalViewModel {Title = "Error",
                //TODO: better error?!
                Content = "Something is wrong"};
           


            vm.SetViaButtonDescriptions(new[]
                                            {
                                                new ButtonDescription
                                                    {
                                                        Title = "Continue"
                                                    }
                                            });

             Messenger.Default.Send(new MetroDialogBoxMessage(vm));
        }


        private string _packageName;

        public string PackageName
        {
            get { return _packageName; }
            set
            {
                _packageName = value;
                RaisePropertyChanged("PackageName");
            }
        }

        

        public IPackage Package { get; private set; }

        public ActivityType ActivityType { get; private set; }
       
        private double _progress;

        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }



        private State _state;

        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged("State");
            }
        }


        private string _error;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged("Error");
            }
        }


        public ICommand SeeError { get; set; }

     

    }

    public enum State
    {
        Performing,
        Finished,
        Failed

    }

    public enum ActivityType
    {
        Install,
        Remove,
        SetState

    }

    
}