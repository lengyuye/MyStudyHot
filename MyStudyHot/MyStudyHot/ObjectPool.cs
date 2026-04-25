using System.Collections.Concurrent;

namespace MyStudyHot;

public class ObjectPool<T> where T : new()
{
    private ConcurrentBag<T> _objects = new ConcurrentBag<T>();
    public T Get() => _objects.TryTake(out T obj) ? obj : new T();
    public void Return(T obj) => _objects.Add(obj);
}