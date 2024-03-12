using ConcurentModels.Interfaces;
using ConcurentModels.Wrapper;

namespace ConcurentModels.Models;

public record CringeTask(int Id) : ITask
{
    public async Task<OperationResult> Complete()
    {
        // Console.WriteLine($"Cringe №{Id} start");
        await Task.Delay(500);
        Console.WriteLine($"Cringe №{Id} end");
        return new OperationResult();
    }
}