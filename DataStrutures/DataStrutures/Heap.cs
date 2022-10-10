using System;
using System.Collections.Generic;

namespace DataStrutures;

public class Heap<T>
{
    private T[] Items;
    private readonly IComparer<T> Comparer;
    public int Count { get; private set; }

    public Heap(IComparer<T> comparer)
    {
        Comparer = comparer;
        Items = new T[7];
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
        HeapifyDown(ref Items[--Count], 0);
        return true;
    }

    private void HeapifyDown(ref T item, int index)
    {
        int right = (index << 1) + 2;
        if (right <= Count)
        {
            if (Comparer.Compare(Items[right], item) < 0)
            {
                var temp = item;
                Items[index] = Items[right];
                Items[right] = temp;

                HeapifyDown(ref Items[right], right);
                
                item = Items[index];
            }
        }

        int left = (index << 1) + 1;
        if (left <= Count)
        {
            if (Comparer.Compare(Items[left], item) < 0)
            {
                var temp = item;
                Items[index] = Items[left];
                Items[left] = temp;

                HeapifyDown(ref Items[left], left);
            }
        }
    }

    public void Push(T item)
    {
        EnsureSpace();

        Items[Count] = item;

        HeapifyUp(ref Items[Count], Count);
        Count++;
    }

    private void HeapifyUp(ref T item, int index)
    {
        int parent = (index - 1) >> 1;
        if (parent < 0)
        {
            return;
        }

        if (Comparer.Compare(Items[parent], item) < 0)
        {
            return;
        }
        var temp = item;
        item = Items[parent];
        Items[parent] = temp;
        HeapifyUp(ref Items[parent], parent);
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
