using System.Collections.Specialized;
using System.ComponentModel;

namespace Observe.Collections;

public class ObservableListTests
{
    public ObservableListTests()
    {
        List = new();
        Events = new();
        List.PropertyChanging += (s, e) =>
        {
            s.ShouldBe(List);
            Events.Add(e);
        };
        List.PropertyChanged += (s, e) =>
        {
            s.ShouldBe(List);
            Events.Add(e);
        };
        List.CollectionChanged += (s, e) =>
        {
            s.ShouldBe(List);
            Events.Add(e);
        };
    }

    ObservableList<int> List { get; }
    List<EventArgs> Events { get; }

    [Fact]
    public void Add()
    {
        for (int i = 0; i < 1000; ++i)
        {
            List.Count.ShouldBe(i);
            Events.Count.ShouldBe(i * 5);

            List.Add(i);

            List.Count.ShouldBe(i + 1);
            Events.Count.ShouldBe((i + 1) * 5);

            Events[i * 5 + 0].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Count");
            Events[i * 5 + 1].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Item[]");
            Events[i * 5 + 2].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Count");
            Events[i * 5 + 3].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Item[]");

            var e = Events[i * 5 + 4].ShouldBeOfType<NotifyCollectionChangedEventArgs<int>>();
            e.Action.ShouldBe(NotifyCollectionChangedAction.Add);
            e.OldItems.ShouldBeNull();
            e.OldStartingIndex.ShouldBe(-1);
            e.NewItems.ShouldBe([i]);
            e.NewStartingIndex.ShouldBe(i);
        }
    }

    [Fact]
    public void AddRange()
    {
        for (int i = 0; i < 1000; ++i)
        {
            List.Count.ShouldBe(i * 3);
            Events.Count.ShouldBe(i * 5);

            List.AddRange([i * 3 + 0, i * 3 + 1, i * 3 + 2]);

            List.Count.ShouldBe((i + 1) * 3);
            Events.Count.ShouldBe((i + 1) * 5);

            Events[i * 5 + 0].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Count");
            Events[i * 5 + 1].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Item[]");
            Events[i * 5 + 2].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Count");
            Events[i * 5 + 3].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Item[]");

            var e = Events[i * 5 + 4].ShouldBeOfType<NotifyCollectionChangedEventArgs<int>>();
            e.Action.ShouldBe(NotifyCollectionChangedAction.Add);
            e.OldItems.ShouldBeNull();
            e.OldStartingIndex.ShouldBe(-1);
            e.NewItems.ShouldBe([i * 3 + 0, i * 3 + 1, i * 3 + 2]);
            e.NewStartingIndex.ShouldBe(i * 3);
        }
    }
}
