using System.Collections.Concurrent;
using ConcurentModels.Interfaces;

namespace ConcurentModels.Models;

public class CustomConcurrentQueue
{
    private ConcurrentDictionary<int, ITask> _dictionary = new();
    // private ConcurrentQueue<int> _queue = new();

    public bool TryAdd(ITask task)
    {
        if (_dictionary.TryAdd(task.Id, task))
        {
            // _queue.Enqueue(task.Id);
            return true;
        }
        return false;
    }

    public ITask? Next()
    {
        lock (this)
        {
            using var enumerator = _dictionary.GetEnumerator();
            if (enumerator.MoveNext())
            {
                if (_dictionary.TryRemove(enumerator.Current.Key, out var value))
                {
                    return value;
                }

                throw new Exception("Cannot take next");
            }

            return null;
        }
        // if (_queue.TryDequeue(out var id))
        // {
        //     if (_dictionary.TryRemove(id, out var task))
        //     {
        //         return task;
        //     }
        // }

        // return null;
    }

    public bool TryRemove(int id)
    {
        return _dictionary.TryRemove(id, out _);
    }
}