using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataStrutures;

public class Heap<T>
{
    private T?[] Items;
    private readonly IComparer<T> Comparer;
    public int Count { get; private set; }

    public Heap(IComparer<T> comparer)
    {
        Comparer = comparer;
        Items = new T?[7];
        Count = 0;
    }

    public bool TryPeek(out T? item)
    {
        if (Count < 1)
        {
            item = default;
            return false;
        }

        item = Items[0];
        return true;
    }

    public bool TryPop(out T? item)
    {
        if (Count < 1)
        {
            item = default;
            return false;
        }

        item = Items[0];
        HeapifyDown(Items[--Count]!, 0);
        Items[Count] = default;
        return true;
    }

    private void HeapifyDown(T item, int index)
    {
        int right = (index << 1) + 2;
        if (right <= Count)
        {
            HalfHeapifyDown(ref item, right);
        }

        int left = (index << 1) + 1;
        if (left <= Count)
        {
            HalfHeapifyDown(ref item, left);
        }

        Items[index] = item;

        void HalfHeapifyDown(ref T item, int index)
        {
            if (Comparer.Compare(Items[index], item) < 0)
            {
                var temp = item;
                item = Items[index]!;
                HeapifyDown(temp, index);
            }
        }
    }

    public void Push(T item)
    {
        EnsureSpace();

        HeapifyUp(item, Count);
        Count++;
    }

    private void HeapifyUp(T item, int index)
    {
        int parent = (index - 1) >> 1;
        if (parent < 0)
        {
            Items[index] = item;
            return;
        }

        if (Comparer.Compare(Items[parent], item) < 0)
        {
            Items[index] = item;
            return;
        }
        Items[index] = Items[parent];
        HeapifyUp(item, parent);
    }

    private void EnsureSpace()
    {
        if (Count < Items.Length)
        {
            return;
        }

        var newArray = new T[(Items.Length << 1) + 1];
        Array.Copy(Items, 0, newArray, 0, Count);
        Items = newArray;
    }
}
