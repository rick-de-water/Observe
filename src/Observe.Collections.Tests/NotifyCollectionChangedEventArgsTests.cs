using System.Collections.Specialized;

namespace Observe.Collections;

public class NotifyCollectionChangedEventArgsTests
{
    [Fact]
    public void AddWithoutIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Add(12);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Add);
        e.OldItems.ShouldBeNull();
        e.OldStartingIndex.ShouldBe(-1);
        e.NewItems.ShouldBe([12]);
        e.NewStartingIndex.ShouldBe(-1);
    }

    [Fact]
    public void AddWithIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Add(12, 3);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Add);
        e.OldItems.ShouldBeNull();
        e.OldStartingIndex.ShouldBe(-1);
        e.NewItems.ShouldBe([12]);
        e.NewStartingIndex.ShouldBe(3);
    }

    [Fact]
    public void AddRangeWithoutIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Add([12, 13, 14]);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Add);
        e.OldItems.ShouldBeNull();
        e.OldStartingIndex.ShouldBe(-1);
        e.NewItems.ShouldBe([12, 13, 14]);
        e.NewStartingIndex.ShouldBe(-1);
    }

    [Fact]
    public void AddRangeWithIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Add([12, 13, 14], 3);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Add);
        e.OldItems.ShouldBeNull();
        e.OldStartingIndex.ShouldBe(-1);
        e.NewItems.ShouldBe([12, 13, 14]);
        e.NewStartingIndex.ShouldBe(3);
    }

    [Fact]
    public void RemoveWithoutIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Remove(12);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
        e.OldItems.ShouldBe([12]);
        e.OldStartingIndex.ShouldBe(-1);
        e.NewItems.ShouldBeNull();
        e.NewStartingIndex.ShouldBe(-1);
    }

    [Fact]
    public void RemoveWithIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Remove(12, 3);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
        e.OldItems.ShouldBe([12]);
        e.OldStartingIndex.ShouldBe(3);
        e.NewItems.ShouldBeNull();
        e.NewStartingIndex.ShouldBe(-1);
    }

    [Fact]
    public void RemoveRangeWithoutIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Remove([12, 13, 14]);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
        e.OldItems.ShouldBe([12, 13, 14]);
        e.OldStartingIndex.ShouldBe(-1);
        e.NewItems.ShouldBeNull();
        e.NewStartingIndex.ShouldBe(-1);
    }

    [Fact]
    public void RemoveRangeWithIndex()
    {
        var e = NotifyCollectionChangedEventArgs<int>.Remove([12, 13, 14], 3);
        e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
        e.OldItems.ShouldBe([12, 13, 14]);
        e.OldStartingIndex.ShouldBe(3);
        e.NewItems.ShouldBeNull();
        e.NewStartingIndex.ShouldBe(-1);
    }
}
