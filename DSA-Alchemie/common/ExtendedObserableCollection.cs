﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alchemie.common
{
    public class ExtendedObserableCollection<T> : ObservableCollection<T>
    {
        public ExtendedObserableCollection() : base() { }
        public ExtendedObserableCollection(IEnumerable<T> collection) : base(collection) { }
        public ExtendedObserableCollection(List<T> list) : base(list) { }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            CheckReentrancy();
            int startIndex = this.Count;
            foreach(var item in collection)
            {
                Items.Add(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(), startIndex));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
        }
        public void ReplaceRange(int startIndex, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (startIndex + collection.Count() > Count)
            {
                throw new IndexOutOfRangeException();
            }
            CheckReentrancy();
            var oldItems = Items.ToList().GetRange(startIndex, collection.Count() - 1);
            for(int i = startIndex ; i < Items.Count; i++)
            {
                Items[i] = collection.ElementAt(i - startIndex);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldItems, collection, startIndex));
            //OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
        public void RemoveRange(int startIndex, int count)
        {
            if (startIndex + count >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            CheckReentrancy();
            var oldItems = Items.ToList().GetRange(startIndex, count);
            for(int i = startIndex; i <= startIndex + count; i++)
            {
                Items.RemoveAt(i);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
        }
    }
}
