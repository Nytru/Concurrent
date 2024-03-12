namespace ConcurentServer.Options;

public record SemaphoreOptions
{
    public int MaxValue { get; set; }
}