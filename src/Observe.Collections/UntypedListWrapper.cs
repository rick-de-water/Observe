using System.Collections;
using System.Collections.Generic;

namespace Observe.Collections
{
    /// <summary>
    /// Turns a non-generic list into a generic list.
    /// Expects all items in the non-generic list to be of type <typeparamref name="T"/>.
    /// </summary>
    internal class UntypedListWrapper<T> : IReadOnlyList<T>
    {
        public UntypedListWrapper(IList list)
        {
            _list = list;
        }

        public T this[int index] => (T)_list[index];
        public int Count => _list.Count;


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            return new UntypedEnumeratorWrapper<T>(_list.GetEnumerator());
        }

        private readonly IList _list;
    }
}
