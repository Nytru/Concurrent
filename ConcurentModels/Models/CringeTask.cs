using ConcurentModels.Interfaces;
using ConcurentModels.Wrapper;

namespace ConcurentModels.Models;

public record CringeTask(int Id) : ITask
{
    public async Task<OperationResult> Complete()
    {
        Console.WriteLine("Cringe");
        await Task.CompletedTask;
        return new OperationResult();
    }
}