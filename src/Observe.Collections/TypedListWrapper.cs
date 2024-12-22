using System.Collections;
using System.Collections.Generic;

namespace Observe.Collections
{
    internal class TypedListWrapper<T> : IReadOnlyList<T>
    {
        public TypedListWrapper(IList list)
        {
            _list = list;
        }

        public T this[int index] => (T)_list[index];
        public int Count => _list.Count;


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            return new TypedListWrapperEnumerator<T>(_list.GetEnumerator());
        }

        private readonly IList _list;
    }

    internal class TypedListWrapperEnumerator<T> : IEnumerator<T>
    {
        public TypedListWrapperEnumerator(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => (T)_enumerator.Current;
        object IEnumerator.Current => _enumerator.Current;

        public void Dispose() { }
        public bool MoveNext() => _enumerator.MoveNext();
        public void Reset() => _enumerator.Reset();

        private IEnumerator _enumerator;
    }
}
