using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Observe.Collections
{
    public interface INotifyCollectionChanged<T> : INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        new event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged;
    }
}
