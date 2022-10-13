using DataStrutures;
using System;

namespace DataStrutureTests;

[TestFixtureSource(typeof(HeapTestsSource),nameof(HeapTestsSource.HeapTestsArgs))]
public class HeapTests
{
    private IHeap<int> Heap;
    private int[] Samples;
    private readonly int Count;

    public HeapTests(IHeap<int> heap, int count)
    {
        Heap = heap;
        Count = count;
    }

    [SetUp]
    public void Setup()
    {
        Samples = new int[Count];

        for (int i = 0; i < Samples.Length; i++)
        {
            Samples[i] = i;
        }

        var random = new Random(1539);
        for (int i = 0; i < Samples.Length; i++)
        {
            int j = random.Next(Count);
            int temp = Samples[i];
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
        Assert.That(root, Is.Zero);
        Assert.Pass();
    }

    [Test]
    public void Pop_All_Items_Out_Is_Increment()
    {
        Assume.That(Heap.Count, Is.EqualTo(Count));
        Assume.That(Heap.TryPop(out var previous), Is.True);

        while (Heap.TryPop(out var value))
        {
            Assert.That(value, Is.GreaterThan(previous));
            previous = value;
        }
        Assert.Pass();
    }
}
