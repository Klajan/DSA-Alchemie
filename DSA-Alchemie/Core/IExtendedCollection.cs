using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alchemie.Core
{
    interface IExtendedCollection<T> : ICollection<T>, IList<T>, IEnumerable<T>
    {
        public void AddRange(IEnumerable<T> collection);
        public void ReplaceRange(int startIndex, IEnumerable<T> collection);
        public void RemoveRange(int startIndex, int count);
        public void RebuildWithRange(IEnumerable<T> collection);
    }

    public class ExtendedCollection<T> : Collection<T>, IExtendedCollection<T>
    {
        public ExtendedCollection(IList<T> list) : base(list) { }
        public ExtendedCollection(int capacity) : base(new List<T>(capacity)) { }
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            foreach (T item in collection)
            {
                Items.Add(item);
            }
        }

        public void RebuildWithRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            Items.Clear();
            AddRange(collection);
        }

        public void RemoveRange(int startIndex, int count)
        {
            if (startIndex + count >= Count) throw new ArgumentOutOfRangeException(nameof(count));
            for (int i = startIndex; i <= startIndex + count; i++)
            {
                Items.RemoveAt(i);
            }
        }

        public void ReplaceRange(int startIndex, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (startIndex + collection.Count() > Count) throw new ArgumentOutOfRangeException(nameof(collection));
            for (int i = startIndex; i < Items.Count; i++)
            {
                Items[i] = collection.ElementAt(i - startIndex);
            }
        }
    }
}
