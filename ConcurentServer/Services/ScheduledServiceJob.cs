using System.Linq.Expressions;
using Coravel.Invocable;

namespace ConcurentServer.Services;

public class ScheduledServiceJob : IInvocable
{
    private readonly ILogger<ScheduledServiceJob> _logger;
    private readonly CounterService _counterService;

    public ScheduledServiceJob(
        ILogger<ScheduledServiceJob> logger,
        CounterService counterService)
    {
        _logger = logger;
        _counterService = counterService;
    }

    public async Task Invoke()
    {
        _logger.LogInformation("{name} job invoked", nameof(ScheduledServiceJob));
        var nextTask = _counterService.GetNext();
        if (nextTask.Success)
        {
            var result = await nextTask.Value.Complete();
            if (result.Success is false)
            {
                _logger.LogError(
                    exception: result.Exception,
                    message: result.Exception.Message);
            }
        }
        else
        {
            _logger.LogInformation("{message}", nextTask.PublicExceptionMessage);
        }
    }
}