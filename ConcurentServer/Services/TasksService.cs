using ConcurentModels.Interfaces;
using ConcurentModels.Wrapper;
using ConcurentServer.Extensions;
using ConcurentServer.Options;
using Microsoft.Extensions.Options;

namespace ConcurentServer.Services;

public class TasksService
{
    public static volatile int Counter;
    private volatile bool _paused = false;

    private static readonly object Lock = new();
    private SemaphoreSlim _semaphore;
    private volatile Dictionary<Guid, (DateTime Date, ITask Task)> _dictionary = new();
    private SemaphoreOptions _options;

    public TasksService(IOptions<SemaphoreOptions> options)
    {
        _options = options.Value;
        _semaphore = new SemaphoreSlim(_options.MaxValue, _options.MaxValue);
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

    private static volatile int _took;
    public void Pause(bool state)
    {
        lock (Lock)
        {
            if (_paused == state)
                return;

            _paused = state;
            if (_paused)
            {
                while (_semaphore.CurrentCount > 0)
                {
                    _semaphore.Wait();
                    Interlocked.Increment(ref _took);
                }
            }
            else
            {
                for (var i = 0; i < _took; i++)
                {
                    _semaphore.Release();
                }

                Interlocked.Add(ref _took, -_took);
            }
        }
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
            var result = GetNext();
            if (result.Success)
            {
                await result.Value.Complete();
                Console.WriteLine(Interlocked.Increment(ref Counter));
            }
        }
        finally
        {
            _semaphore.Release();
        }

    }
}