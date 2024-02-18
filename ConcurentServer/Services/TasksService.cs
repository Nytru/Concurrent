using ConcurentModels.Interfaces;
using ConcurentModels.Wrapper;
using ConcurentServer.Extensions;
using ConcurentServer.Options;
using Microsoft.Extensions.Options;

namespace ConcurentServer.Services;

public class TasksService
{
    public static volatile int Counter;
    private static readonly object Lock = new();
    private readonly SemaphoreSlim _semaphore;
    private volatile Dictionary<Guid, (DateTime Date, ITask Task)> _dictionary = new();

    public TasksService(IOptions<SemaphoreOptions> options)
    {
        _semaphore = new SemaphoreSlim(options.Value.MaxValue, options.Value.MaxValue);
    }

    public OperationResult<Guid> AddTask(ITask cringeTask)
    {
        Guid id;
        lock (Lock)
        {
            id = Guid.NewGuid();
            _dictionary.Add(id, (DateTime.Now, cringeTask));
        }
        Added().Fire();
        return new OperationResult<Guid>(id);
    }

    public OperationResult RemoveTask(Guid id)
    {
        lock (Lock)
        {
            _dictionary.Remove(id);
        }
        return new OperationResult();
    }

    private OperationResult<ITask> GetNext()
    {
        KeyValuePair<Guid, (DateTime Date, ITask Task)> task;
        lock (Lock)
        {
            task = _dictionary.MinBy(pair => pair.Value.Date);
            _dictionary.Remove(task.Key);
        }

        return new OperationResult<ITask>(task.Value.Task);
    }

    private async Task Added()
    {
        try
        {
            await _semaphore.WaitAsync();
            var t = GetNext();
            if (t.Success)
            {
                await t.Value.Complete();
                Console.WriteLine(Interlocked.Increment(ref Counter));
            }
        }
        finally
        {
            _semaphore.Release();
        }

    }
}