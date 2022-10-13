using System.Collections.Generic;

namespace DataStrutures;

public class HeapNode<T> : IHeap<T>
{
    private T? Value;
    private bool HasValue { get; set; }

    private HeapNode<T>? Left;
    private HeapNode<T>? Right;
    private readonly IComparer<T> Comparer;

    public int Count => (Left?.Count ?? 0) + (Right?.Count ?? 0) + (HasValue ? 1 : 0);

    public HeapNode(IComparer<T> comparer)
    {
        Comparer = comparer;
    }

    public bool TryPeek(out T? item)
    {
        item = HasValue ? Value : default;
        return HasValue;
    }

    public bool TryPop(out T? item)
    {
        if (!HasValue)
        {
            item = default;
            return false;
        }

        item = Value;

        if (Left is null)
        {
            if (Right is null)
            { 
                HasValue = false;
                Value = default;
                return true;
            }

            if (Right.TryPop(out var rr))
            {
                Value = rr;
                return true;
            }

            HasValue = false;
            Value = default;
            return true;
        }

        if (Right is null)
        {
            if (Left.TryPop(out var ll))
            {
                Value = ll;
                return true;
            }

            HasValue = false;
            Value = default;
            return true;
        }

        var hasLeft = Left.TryPeek(out var left);
        var hasRight = Right.TryPeek(out var right);
        if (hasLeft && hasRight)
        {
            if (Comparer.Compare(left, right) < 0)
            {
                Left.TryPop(out _);
                Value = left;
                return true;
            }
            else
            {
                Right.TryPop(out _);
                Value = right;
                return true;
            }
        }

        if (hasLeft)
        {
            Left.TryPop(out _);
            Value = left;
            return true;
        }

        if (hasRight)
        {
            Right.TryPop(out _);
            Value = right;
            return true;
        }

        Value = default;
        HasValue = false;
        return true;
    }

    public void Push(T item)
    {
        if (!HasValue)
        {
            Value = item;
            HasValue = true;
            return;
        }

        if (Comparer.Compare(item, Value) < 0)
        {
            var temp = Value;
            Value = item;
            item = temp!;
        }

        var node = (Right?.Count ?? 0).CompareTo((Left?.Count ?? 0)) < 0
            ? Right ??= new HeapNode<T>(Comparer)
            : Left ??= new HeapNode<T>(Comparer);

        node.Push(item);
    }

    public override string ToString() => $"{{ {nameof(Value)} = {Value}, {nameof(Count)} = {Count} }}";
}