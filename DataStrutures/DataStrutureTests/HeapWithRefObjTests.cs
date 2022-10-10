using DataStrutures;
using System.Collections.Generic;
using System;

namespace DataStrutureTests;

[TestFixture(8)]
[TestFixture(100)]

public class HeapWithRefObjTests
{
    private Heap<Foo> Heap;
    private Foo[] Samples;
    private readonly int Count;

    public HeapWithRefObjTests(int count)
    {
        Count = count;
    }

    [SetUp]
    public void Setup()
    {
        Heap = new Heap<Foo>(new FooComparer());
        Samples = new Foo[Count];

        for (int i = 0; i < Samples.Length; i++)
        {
            Samples[i] = i;
        }

        var random = new Random(1539);
        for (int i = 0; i < Samples.Length; i++)
        {
            int j = random.Next(Count);
            Foo temp = Samples[i];
            Samples[i] = Samples[j];
            Samples[j] = temp;
        }

        for (int i = 0; i < Samples.Length; i++)
        {
            Heap.Push(Samples[i]);
        }
    }

    [Test]
    public void Root_Is_Equal_To_Minium()
    {
        Assume.That(Heap.Count, Is.EqualTo(Count));

        Assert.That(Heap.TryPeek(out var root), Is.True);
        Assert.That(root, Is.Not.Null);
        Assert.That(root.Value, Is.Zero);
        Assert.Pass();
    }

    [Test]
    public void Pop_All_Items_Out_Is_Increment()
    {
        Assume.That(Heap.Count, Is.EqualTo(Count));
        Assume.That(Heap.TryPop(out var previous), Is.True);

        while (Heap.TryPop(out var value))
        {
            Assert.That(value, Is.GreaterThanOrEqualTo(previous));
            previous = value;
        }
        Assert.Pass();
    }

    public class Foo : IComparable<Foo>
    {
        public int Value { get; init; }

        public int CompareTo(Foo? other) => Value.CompareTo(other?.Value ?? 0);

        public static implicit operator Foo(int value) => new Foo { Value = value };
    }

    public class FooComparer : IComparer<Foo>
    {
        public int Compare(Foo? x, Foo? y) => (x?.Value ?? 0).CompareTo(y?.Value ?? 0);
    }
}