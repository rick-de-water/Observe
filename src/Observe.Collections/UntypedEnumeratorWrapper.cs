using System.Collections;
using System.Collections.Generic;

namespace Observe.Collections
{
    /// <summary>
    /// Turns a non-generic enumerator into a generic enumerator.
    /// Expects all items in the non-generic enumerator to be of type <typeparamref name="T"/>.
    /// </summary>
    internal class UntypedEnumeratorWrapper<T> : IEnumerator<T>
    {
        public UntypedEnumeratorWrapper(IEnumerator enumerator)
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
