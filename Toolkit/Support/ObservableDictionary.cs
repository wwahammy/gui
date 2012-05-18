using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CoApp.Gui.Toolkit.Support
{
   /// <summary>
   /// A Dictionary that implements the INotifyCOllectionChanged interface.
   /// </summary>
   /// <typeparam name="K">the type of the dictionary key</typeparam>
   /// <typeparam name="V">the type of the dictionary value</typeparam>
    public class ObservableDictionary<K,V> : IDictionary<K,V>, INotifyCollectionChanged
    {
        private Dictionary<K,V> _dict = new Dictionary<K,V>(); 

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<K, V> item)
        {
            _dict.Add(item.Key, item.Value);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

        }

        public void Clear()
        {
            var old = _dict.ToArray();
            _dict.Clear();
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, old));
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return _dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(K key)
        {
            return _dict.ContainsKey(key);
        }

        public void Add(K key, V value)
        {
            _dict.Add(key, value);
        }

        public bool Remove(K key)
        {
            var val = _dict[key];
            var worked = _dict.Remove(key);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<K, V>(key, val)));
            return worked;
        }

        public bool TryGetValue(K key, out V value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public V this[K key]
        {
            get { return _dict[key]; }
            set
            {
                V old = default(V);
                bool foundOld = false;
                if (_dict.ContainsKey(key))
                {
                    foundOld = true;
                    old = _dict[key];
                }

                NotifyCollectionChangedEventArgs args = null;
                if (foundOld)
                {
                   args =  new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                                                                new KeyValuePair<K, V>(key, value),  new KeyValuePair<K, V>(key, old));
                    
                }

                else
                {
                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                                                                new KeyValuePair<K, V>(key, value));
                }

                CollectionChanged(this, args);

            }
        }

        public ICollection<K> Keys
        {
            get { return _dict.Keys; }
        }

        public ICollection<V> Values
        {
            get { return _dict.Values; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };
    }
}
