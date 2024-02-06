using ConcurentServer.Services;

namespace ConcurentServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services) => services
        .AddScoped<ScheduledServiceJob>();

    public static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddSingleton<CounterService>();
}