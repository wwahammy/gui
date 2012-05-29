using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoApp.PackageManager.Messages;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.Model.Interfaces
{
    public interface IActivityService
    {
        Task InstallPackage(IPackage p);
        Task RemovePackage(IPackage p);

        IList<Activity> Activities { get; }
    }

    public class Activity : ObservableObject
    {
        public Activity(IPackage p, ActivityType a)
        {
            Package = p;
            ActivityType = a;
            State = State.Performing;
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
        Block,
        Unblock,
        Activate

    }

    
}