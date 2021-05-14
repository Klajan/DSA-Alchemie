using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Alchemie.Core
{
    public class ExtendedObserableCollection<T> : ObservableCollection<T>, IExtendedCollection<T>
    {
        public ExtendedObserableCollection() : base()
        {
        }

        public ExtendedObserableCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        public ExtendedObserableCollection(IList<T> list) : base(list)
        {
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            CheckReentrancy();
            foreach (T item in collection)
            {
                Items.Add(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
        }

        public void ReplaceRange(int startIndex, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (startIndex + collection.Count() > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(collection));
            }
            CheckReentrancy();
            List<T> oldItems = Items.ToList().GetRange(startIndex, collection.Count() - 1);
            for (int i = startIndex; i < Items.Count; i++)
            {
                Items[i] = collection.ElementAt(i - startIndex);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldItems, collection, startIndex));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        public void RemoveRange(int startIndex, int count)
        {
            if (startIndex + count >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            CheckReentrancy();
            var oldItems = Items.ToList().GetRange(startIndex, count);
            for (int i = startIndex; i <= startIndex + count; i++)
            {
                Items.RemoveAt(i);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
        }

        public void RebuildWithRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            CheckReentrancy();
            Items.Clear();
            AddRange(collection);
        }
    }
}