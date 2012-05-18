using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CoApp.Gui.Toolkit.Model.Interfaces;
using CoApp.PackageManager.Messages;
using CoApp.PackageManager.Model.Interfaces;
using CoApp.Packaging.Client;

using CoApp.Toolkit.Extensions;
using GalaSoft.MvvmLight.Messaging;

namespace CoApp.PackageManager.Model
{
    public class ActivityService : IActivityService
    {
        internal ICoAppService CoApp;

        public ActivityService()
        {
            CoApp = new LocalServiceLocator().CoAppService;
            Activities = new ObservableCollection<Activity>();
            ((ObservableCollection<Activity>) Activities).CollectionChanged +=
                (s, a) => Messenger.Default.Send(new ActivitiesUpdatedMessage());
        }

        #region IActivityService Members

        public Task InstallPackage(Package p)
        {
            return
                Task.Factory.StartNew(() =>
                                          {
                                              Activity a = CreateActivity(p, ActivityType.Install);
                                              Activities.Add(a);
                                              Task task = CoApp.InstallPackage(p.CanonicalName,
                                                                               (ignore1, ignore2, progress) =>
                                                                               a.Progress = progress,
                                                                               s =>
                                                                               a.State = State.Finished);

                                              task.Wait();
                                              if (task.Exception != null)
                                              {
                                                  a.State = State.Failed;
                                              }
                                              task.RethrowWhenFaulted();
                                          });
        }

        public Task RemovePackage(Package p)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 Activity a = CreateActivity(p, ActivityType.Remove);
                                                 Activities.Add(a);
                                                 Task task = CoApp.RemovePackage(p.CanonicalName, (i, progress) =>
                                                                                                  a.Progress = progress,
                                                                                 i =>
                                                                                 a.State = State.Finished);
                                                 task.Wait();

                                                 if (task.Exception != null)
                                                 {
                                                     a.State = State.Failed;
                                                 }

                                                 task.RethrowWhenFaulted();
                                             }
                );
        }

        public IList<Activity> Activities { get; private set; }

        #endregion

        private Activity CreateActivity(Package p, ActivityType type)
        {
            var a = new Activity(p, type);
            a.PropertyChanged += (sender, args) => Messenger.Default.Send(new ActivitiesUpdatedMessage());
            return a;
        }
    }
}