using ConcurentServer.Options;
using ConcurentServer.Services;

namespace ConcurentServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServiceOptions(this IServiceCollection services, IConfiguration configuration) => services
        .ConfigureByName<SemaphoreOptions>(configuration);

    public static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddSingleton<TasksService>()
        .AddSingleton<SwitchService>();

    private static IServiceCollection ConfigureByName<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class
    {
        var section = configuration.GetRequiredSection(typeof(T).Name);
        services.Configure<T>(section);
        return services;
    }
}