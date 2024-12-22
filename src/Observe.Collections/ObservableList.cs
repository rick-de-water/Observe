using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Observe.Collections
{
    [DebuggerDisplay("Count = {Count}")]
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
            set => throw new NotImplementedException();
        }

        public int Count => _values.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item) => _values.IndexOf(item);
        public bool Contains(T item) => _values.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

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
            _values.AddRange(collectionChangeEventArgs.NewItems);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Add(item, index);
            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.Insert(index, item);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Add(items, index);
            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.InsertRange(index, collectionChangeEventArgs.NewItems);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            RemoveAt(index);

            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Remove(_values[index], index);
            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.RemoveAt(index);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (Count - index < count)
            {
                throw new ArgumentException();
            }

            if (count == 0)
            {
                return;
            }

            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Remove(
                Enumerable.Range(index, count).Select(i => _values[i]), index);
            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.RemoveRange(index, count);
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            var collectionChangeEventArgs = NotifyCollectionChangedEventArgs<T>.Remove(_values, 0);

            InvokePropertyChanging(CountName);
            InvokePropertyChanging(IndexerName);
            _values.Clear();
            InvokePropertyChanged(CountName);
            InvokePropertyChanged(IndexerName);
            InvokeCollectionChanged(collectionChangeEventArgs);
        }

        public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private readonly List<T> _values;
    }
}
