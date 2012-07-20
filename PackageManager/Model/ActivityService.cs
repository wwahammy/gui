using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.PackageManager.Messages;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Packaging.Client;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.Model
{
    public class ActivityService : IActivityService
    {
        internal ICoAppService CoApp;

        readonly object _activityListLock = new object();

        public ActivityService()
        {
            CoApp = new LocalServiceLocator().CoAppService;
            Activities = new ObservableCollection<Activity>();

            //Sample Activities
       
            ((ObservableCollection<Activity>) Activities).CollectionChanged +=
                (s, a) => Messenger.Default.Send(CreateActivitiesUpdateMessage());
         
        }

        

        private ActivitiesUpdatedMessage CreateActivitiesUpdateMessage()
        {
            var msg = new ActivitiesUpdatedMessage();
            lock (_activityListLock)
            {
                foreach (var g in Activities.GroupBy(a => a.State))
                {
                    var i = g.Count();
                    switch(g.Key)
                    {
                        case State.Finished:
                            msg.NumberFinished = i;
                            break;
                        case State.Failed:
                            msg.NumberOfFailures = i;
                            break;
                        case State.Performing:
                            msg.NumberOfActivities = i;
                            break;
                    }
                }
                return msg;
            }
        }

        #region IActivityService Members

        public Task InstallPackage(IPackage p)
        {


            Activity a = CreateActivity(p, ActivityType.Install);
            AddActivity(a);
            Task task = CoApp.InstallPackage(p.CanonicalName,
                                             (ignore1, ignore2, progress) =>
                                             a.Progress = progress,
                                             s =>
                                             a.State = State.Finished).ContinueOnFail(e => a.State = State.Failed);


            return task;
        }

        public Task RemovePackage(IPackage p)
        {

            Activity a = CreateActivity(p, ActivityType.Remove);
            AddActivity(a);
            Task task = CoApp.RemovePackage(p.CanonicalName, (i, progress) =>
                                                             a.Progress = progress,
                                            i =>
                                            a.State = State.Finished).ContinueOnFail(e => a.State = State.Failed);
         
            return task;

           
        }

    

    public Task SetState(IPackage p, PackageState state)
        {
       
                                                 var a = CreateActivity(p, ActivityType.SetState);
                                                 AddActivity(a);

        var task = CoApp.SetState(p.CanonicalName, state).ContinueAlways(t =>
                                                                             {
                                                                                 if (t.Exception != null)
                                                                                     a.State = State.Failed;
                                                                                 else
                                                                                     a.State = State.Finished;
                                                                             });


                                               
        return task;

        }

        public void RemoveActivity(Activity a)
        {
            lock (_activityListLock)
            {
                Activities.Remove(a);
            }
            Messenger.Default.Send(CreateActivitiesUpdateMessage());
        }

        private void AddActivity(Activity a)
        {
            lock (_activityListLock)
            {
                Activities.Add(a);
            }
        }

        public IList<Activity> Activities { get; private set; }

        #endregion

        private Activity CreateActivity(IPackage p, ActivityType type)
        {
            var a = new Activity(p, type);
            a.PropertyChanged += (sender, args) => Messenger.Default.Send(CreateActivitiesUpdateMessage());
            return a;
        }
    }
}