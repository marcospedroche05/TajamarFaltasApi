using Microsoft.Extensions.DependencyInjection;

namespace TajamarFaltas.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}