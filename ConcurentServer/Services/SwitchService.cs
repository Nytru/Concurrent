using ConcurentModels.Wrapper;

namespace ConcurentServer.Services;

public class SwitchService
{
    private static object _switchLock = new();

    public OperationResult Switch(bool state)
    {
        lock (_switchLock)
        {
            State = state;
        }
        return OperationResult.Ok;
    }

    public OperationResult Switch()
    {
        lock (_switchLock)
        {
            State = !State;
        }
        return OperationResult.Ok;
    }

    public bool State { get; private set; }
}