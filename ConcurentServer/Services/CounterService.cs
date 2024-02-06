using ConcurentModels.Interfaces;
using ConcurentModels.Models;
using ConcurentModels.Wrapper;

namespace ConcurentServer.Services;

public class CounterService
{
    private CustomConcurrentQueue _tasksQueue = new ();

    public OperationResult<ITask> GetNext()
    {
        var next = _tasksQueue.Next();
        if (next is null)
        {
            return new OperationResult<ITask>(new Exception("No tasks found"));
        }

        return new OperationResult<ITask>(next);
    }

    public OperationResult<int> AddTask(CringeTask cringeTask)
    {
        if (_tasksQueue.TryAdd(cringeTask))
        {
            return cringeTask.Id;
        }

        return new OperationResult<int>(new Exception($"cannot add task with id {cringeTask.Id}"));
    }

    public OperationResult RemoveTask(int id)
    {
        if (_tasksQueue.TryRemove(id))
        {
            return OperationResult.Ok;
        }

        return new Exception($"Cannot remove task with id: {id}");
    }
}