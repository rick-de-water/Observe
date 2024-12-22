using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Observe.Collections
{
    public sealed class NotifyCollectionChangedEventArgs<T> : NotifyCollectionChangedEventArgs
    {
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) : base(action) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems) : base(action, changedItems) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) { }
        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index) { }

        public static NotifyCollectionChangedEventArgs<T> Add(T item)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item);
        public static NotifyCollectionChangedEventArgs<T> Add(T item, int index)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, item, index);
        public static NotifyCollectionChangedEventArgs<T> Add(IEnumerable<T> items)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, new List<T>(items));
        public static NotifyCollectionChangedEventArgs<T> Add(IEnumerable<T> items, int index)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, new List<T>(items), index);

        public static NotifyCollectionChangedEventArgs<T> Remove(T item)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, item);
        public static NotifyCollectionChangedEventArgs<T> Remove(T item, int index)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, item, index);
        public static NotifyCollectionChangedEventArgs<T> Remove(IEnumerable<T> items)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, new List<T>(items));
        public static NotifyCollectionChangedEventArgs<T> Remove(IEnumerable<T> items, int index)
            => new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, new List<T>(items), index);

        //[MaybleNull]
        public new IReadOnlyList<T> OldItems
        {
            get
            {
                if (base.OldItems != null)
                {
                    return new UntypedListWrapper<T>(base.OldItems);
                }
                else
                {
                    return null;
                }
            }
        }

        //[MaybleNull]
        public new IReadOnlyList<T> NewItems
        {
            get
            {
                if (base.NewItems != null)
                {
                    return new UntypedListWrapper<T>(base.NewItems);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
