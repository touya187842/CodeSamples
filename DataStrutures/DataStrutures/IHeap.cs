namespace DataStrutures;

public interface IHeap<T>
{
    public int Count { get; }
    public bool TryPeek(out T? item);
    public bool TryPop(out T? item);
    public void Push(T item);
}
