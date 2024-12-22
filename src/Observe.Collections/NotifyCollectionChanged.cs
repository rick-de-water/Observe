using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Observe.Collections
{
    public abstract class NotifyCollectionChanged<T> : INotifyCollectionChanged<T>
    {
        public event EventHandler<NotifyCollectionChangedEventArgs<T>> CollectionChanged
        {
            add
            {
                _collectionChanged = Delegate.Combine(_collectionChanged, value);
            }
            remove
            {
                _collectionChanged = Delegate.Remove(_collectionChanged, value);
            }
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                _collectionChanged = Delegate.Combine(_collectionChanged, value);
            }

            remove
            {
                _collectionChanged = Delegate.Remove(_collectionChanged, value);
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void InvokeCollectionChanged(NotifyCollectionChangedEventArgs<T> eventArgs)
        {
            if (_collectionChanged == null)
            {
                return;
            }

            var list = _collectionChanged.GetInvocationList();
            foreach (var handler in list)
            {
                handler.DynamicInvoke(this, eventArgs);
            }
        }

        protected void InvokePropertyChanging(string propertyName)
        {
            InvokePropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        protected void InvokePropertyChanging(PropertyChangingEventArgs eventArgs)
        {
            PropertyChanging?.Invoke(this, eventArgs);
        }

        protected void InvokePropertyChanged(string propertyName)
        {
            InvokePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void InvokePropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

        private Delegate _collectionChanged;
    }
}
