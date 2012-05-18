using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using CoApp.Gui.Toolkit.Support;

namespace CoApp.PackageManager.Support.Filters
{
    
    public class EasyCollectionViewSource : CollectionViewSource
    {
        readonly ISet<FilterEventHandler> _filterList = new HashSet<FilterEventHandler>(); 

        public EasyCollectionViewSource()
        {
            Filters = new ObservableDictionary<string, FilterEventHandler>();
            Filters.CollectionChanged += DictionaryChanged;
        }

        private void DictionaryChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    HandleAdd(notifyCollectionChangedEventArgs);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    HandleRemove(notifyCollectionChangedEventArgs);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    HandleReplace(notifyCollectionChangedEventArgs);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    HandleReset(notifyCollectionChangedEventArgs);
                    break;
                default:
                    throw new Exception("we don't handle NotifyCOllectionChangedAction.Moved because it doesn't make a lick of sense.");
            }
        }

        private void HandleReset(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            lock (_filterList)
            {
                _filterList.Clear();
            }
        }

        private void HandleAdd(NotifyCollectionChangedEventArgs  notifyCollectionChangedEventArgs)
        {
            var newItem =
                       notifyCollectionChangedEventArgs.NewItems.Cast<KeyValuePair<string, FilterEventHandler>>().First
                           ();
            Filter += newItem.Value;
        }

        private void HandleRemove(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var oldItem =
                        notifyCollectionChangedEventArgs.OldItems.Cast<KeyValuePair<string, FilterEventHandler>>().First
                            ();
            Filter -= oldItem.Value;
        }

        private void HandleReplace(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var oldItem =
                        notifyCollectionChangedEventArgs.OldItems.Cast<KeyValuePair<string, FilterEventHandler>>().First
                            ();
            var newItem = notifyCollectionChangedEventArgs.NewItems.Cast<KeyValuePair<string, FilterEventHandler>>().First
                    ();

            Filter -= oldItem.Value;
            Filter += newItem.Value;
        }

        public ObservableDictionary<string,FilterEventHandler> Filters { get; private set; }


        internal new event FilterEventHandler Filter
        {
            add
            {
                lock (_filterList)
                {
                    _filterList.Add(value);
                }
            }

            remove
            {
                lock (_filterList)
                {
                    _filterList.Remove(value);
                }
            }
        }


    }
}
