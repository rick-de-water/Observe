using Newtonsoft.Json.Linq;
using System;
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

    private int PseudoRandomIndex()
    {
        if (List.Count == 0)
        {
            return 0;
        }

        var index = List.Count;
        index = ((index >> 16) ^ index) * 0x45d9f3b;
        index = ((index >> 16) ^ index) * 0x45d9f3b;
        index = (index >> 16) ^ index;
        return index % List.Count;
    }

    [Fact]
    public void IndexOf()
    {
        const int total_count = 1000;
        List.AddRange(Enumerable.Range(0, total_count));
        for (int i = 0; i < total_count; ++i)
        {
            List.IndexOf(i).ShouldBe(i);
        }
    }

    [Fact]
    public void Contains()
    {
        const int total_count = 1000;
        List.AddRange(Enumerable.Range(0, total_count));
        for (int i = -total_count; i < total_count * 2; ++i)
        {
            if (i >= 0 && i < total_count)
            {
                List.Contains(i).ShouldBeTrue();
            }
            else
            {
                List.Contains(i).ShouldBeFalse();
            }
        }
    }

    [Fact]
    public void Add()
    {
        for (int i = 0; i < 1000; ++i)
        {
            List.Count.ShouldBe(i);
            Events.Count.ShouldBe(i * 5);

            List.Add(i);

            List.Count.ShouldBe(i + 1);
            List.ShouldBe(Enumerable.Range(0, i + 1));

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
            List.ShouldBe(Enumerable.Range(0, (i + 1) * 3));

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

    [Fact]
    public void Insert()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => List.Insert(-1, 12));
        Should.Throw<ArgumentOutOfRangeException>(() => List.Insert(1, 12));
        Events.ShouldBeEmpty();

        for (int i = 0; i < 1000; ++i)
        {
            List.Count.ShouldBe(i);
            Events.Count.ShouldBe(i * 5);

            var index = PseudoRandomIndex();
            List.Insert(index, i);

            List.Count.ShouldBe(i + 1);
            List[index].ShouldBe(i);

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
            e.NewStartingIndex.ShouldBe(index);
        }
    }

    [Fact]
    public void InsertRange()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => List.InsertRange(-1, [12, 13, 14]));
        Should.Throw<ArgumentOutOfRangeException>(() => List.InsertRange(1, [12, 13, 14]));
        Events.ShouldBeEmpty();

        for (int i = 0; i < 1000; ++i)
        {
            List.Count.ShouldBe(i * 3);
            Events.Count.ShouldBe(i * 5);

            var index = PseudoRandomIndex();
            List.InsertRange(index, [i * 3 + 0, i * 3 + 1, i * 3 + 2]);

            List.Count.ShouldBe((i + 1) * 3);
            List[index + 0].ShouldBe(i * 3 + 0);
            List[index + 1].ShouldBe(i * 3 + 1);
            List[index + 2].ShouldBe(i * 3 + 2);

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
            e.NewStartingIndex.ShouldBe(index);
        }
    }

    [Fact]
    public void Remove()
    {
        const int total_count = 1000;
        List.AddRange(Enumerable.Range(0, total_count));
        Events.Clear();

        int removed_count = 0;

        while (List.Count > 0)
        {
            var index = PseudoRandomIndex();
            var value = List[index];

            List.ShouldContain(value);
            List.Remove(value).ShouldBeTrue();
            removed_count += 1;
            List.ShouldNotContain(value);
            List.Count.ShouldBe(total_count - removed_count);

            Events.Count.ShouldBe(removed_count * 5);
            Events[(removed_count - 1) * 5 + 0].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Count");
            Events[(removed_count - 1) * 5 + 1].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Item[]");
            Events[(removed_count - 1) * 5 + 2].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Count");
            Events[(removed_count - 1) * 5 + 3].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Item[]");

            var e = Events[(removed_count - 1) * 5 + 4].ShouldBeOfType<NotifyCollectionChangedEventArgs<int>>();
            e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
            e.OldItems.ShouldBe([value]);
            e.OldStartingIndex.ShouldBe(index);
            e.NewItems.ShouldBeNull();
            e.NewStartingIndex.ShouldBe(-1);

            List.Remove(value).ShouldBeFalse();
            List.Count.ShouldBe(total_count - removed_count);
            Events.Count.ShouldBe(removed_count * 5);
        }
    }

    [Fact]
    public void RemoveAt()
    {
        const int total_count = 1000;
        List.AddRange(Enumerable.Range(0, total_count));
        Events.Clear();

        Should.Throw<ArgumentOutOfRangeException>(() => List.RemoveAt(-1));
        Should.Throw<ArgumentOutOfRangeException>(() => List.RemoveAt(List.Count));

        int removed_count = 0;

        while (List.Count > 0)
        {
            var index = PseudoRandomIndex();
            var value = List[index];

            List.ShouldContain(value);
            List.RemoveAt(index);
            removed_count += 1;
            List.ShouldNotContain(value);
            List.Count.ShouldBe(total_count - removed_count);

            Events.Count.ShouldBe(removed_count * 5);
            Events[(removed_count - 1) * 5 + 0].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Count");
            Events[(removed_count - 1) * 5 + 1].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Item[]");
            Events[(removed_count - 1) * 5 + 2].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Count");
            Events[(removed_count - 1) * 5 + 3].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Item[]");

            var e = Events[(removed_count - 1) * 5 + 4].ShouldBeOfType<NotifyCollectionChangedEventArgs<int>>();
            e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
            e.OldItems.ShouldBe([value]);
            e.OldStartingIndex.ShouldBe(index);
            e.NewItems.ShouldBeNull();
            e.NewStartingIndex.ShouldBe(-1);
        }
    }

    [Fact]
    public void RemoveRange()
    {
        const int total_count = 1000;
        List.AddRange(Enumerable.Range(0, total_count));
        Events.Clear();

        Should.Throw<ArgumentOutOfRangeException>(() => List.RemoveRange(-1, 1));
        Should.Throw<ArgumentOutOfRangeException>(() => List.RemoveRange(0, -1));
        Should.Throw<ArgumentException>(() => List.RemoveRange(List.Count, 1));
        Should.Throw<ArgumentException>(() => List.RemoveRange(0, List.Count + 1));

        Events.Count.ShouldBe(0);
        List.RemoveRange(0, 0);
        Events.Count.ShouldBe(0);

        int removed_count = 0;
        int operation_count = 0;

        while (List.Count > 0)
        {
            var index = PseudoRandomIndex();
            var count = Math.Min(3, List.Count - index);

            var values = Enumerable
                .Range(index, count)
                .Select(i => List[i])
                .ToArray();

            foreach (var value in values)
            {
                List.ShouldContain(value);
            }

            List.RemoveRange(index, count);
            removed_count += count;
            operation_count += 1;
            List.Count.ShouldBe(total_count - removed_count);

            foreach (var value in values)
            {
                List.ShouldNotContain(value);
            }

            Events.Count.ShouldBe(operation_count * 5);
            Events[(operation_count - 1) * 5 + 0].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Count");
            Events[(operation_count - 1) * 5 + 1].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Item[]");
            Events[(operation_count - 1) * 5 + 2].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Count");
            Events[(operation_count - 1) * 5 + 3].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Item[]");

            var e = Events[(operation_count - 1) * 5 + 4].ShouldBeOfType<NotifyCollectionChangedEventArgs<int>>();
            e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
            e.OldItems.ShouldBe(values);
            e.OldStartingIndex.ShouldBe(index);
            e.NewItems.ShouldBeNull();
            e.NewStartingIndex.ShouldBe(-1);
        }
    }

    [Fact]
    public void Clear()
    {
        const int total_count = 1000;
        List.AddRange(Enumerable.Range(0, total_count));
        Events.Clear();

        List.Clear();
        List.Count.ShouldBe(0);
        List.ShouldBeEmpty();

        Events.Count.ShouldBe(5);
        Events[0].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Count");
        Events[1].ShouldBeOfType<PropertyChangingEventArgs>().PropertyName.ShouldBe("Item[]");
        Events[2].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Count");
        Events[3].ShouldBeOfType<PropertyChangedEventArgs>().PropertyName.ShouldBe("Item[]");

        var e = Events[4].ShouldBeOfType<NotifyCollectionChangedEventArgs<int>>();
        e.Action.ShouldBe(NotifyCollectionChangedAction.Remove);
        e.OldItems.ShouldBe(Enumerable.Range(0, total_count));
        e.OldStartingIndex.ShouldBe(0);
        e.NewItems.ShouldBeNull();
        e.NewStartingIndex.ShouldBe(-1);
    }
}
