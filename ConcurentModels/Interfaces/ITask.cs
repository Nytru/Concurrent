using ConcurentModels.Wrapper;

namespace ConcurentModels.Interfaces;

public interface ITask
{
    public int Id { get; init; }

    public Task<OperationResult> Complete();
}