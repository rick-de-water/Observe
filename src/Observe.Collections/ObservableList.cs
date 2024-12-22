using System.Collections;
using System.Collections.Generic;

namespace Observe.Collections
{
    public sealed class ObservableList<T> : NotifyCollectionChanged<T>, IList<T>
    {
        private const string CountName = nameof(Count);
        private const string IndexerName = "Item[]";

        public ObservableList()
        {
            _values = new List<T>();
        }

        public T this[int index]
        {
            get => _values[index];
            set => throw new System.NotImplementedException();
        }

        public int Count => _values.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Add(item, Count);
            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.Add(item);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Add(items, Count);
            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.AddRange(items);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        private readonly List<T> _values;
    }
}
