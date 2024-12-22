using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Observe.Collections
{
    public sealed class CollectionDebugView<T>
    {
        public CollectionDebugView(IEnumerable<T> collection)
        {
            _collection = collection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => _collection.ToArray();

        private readonly IEnumerable<T> _collection;
    }
}