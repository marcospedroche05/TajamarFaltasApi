using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TajamarFaltas.Application.FaltaManagement;
using TajamarFaltas.Application.Usuarios;
using TajamarFaltas.Domain.Interfaces;
using TajamarFaltas.Infrastructure.ExternalServices;
using TajamarFaltas.Infrastructure.FaltaManagement;
using TajamarFaltas.Infrastructure.Options;
using TajamarFaltas.Infrastructure.Persistence;
using TajamarFaltas.Infrastructure.Usuarios;

namespace TajamarFaltas.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TajamarApiOptions>()
            .BindConfiguration(TajamarApiOptions.SectionName);

        var connectionString = configuration.GetConnectionString("Default") ?? string.Empty;

        services.AddDbContext<TajamarDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddHttpClient<ITajamarApiClientService, TajamarApiClientService>((serviceProvider, client) =>
        {
            var apiOptions = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<TajamarApiOptions>>().Value;
            client.BaseAddress = new Uri(apiOptions.BaseUrl, UriKind.Absolute);
        });

        // Auth services
        services.AddScoped<TajamarFaltas.Application.Auth.ITokenService, TajamarFaltas.Infrastructure.Auth.TokenService>();
        services.AddScoped<TajamarFaltas.Application.Auth.IAuthService, TajamarFaltas.Infrastructure.Auth.AuthService>();
        services.AddScoped<IMisFaltasQueryService, MisFaltasQueryService>();
        services.AddScoped<IProfesorFaltasService, ProfesorFaltasService>();
        services.AddScoped<TajamarFaltas.Application.FaltaManagement.IAdminFaltasService, AdminFaltasService>();
        services.AddScoped<TajamarFaltas.Application.FaltaManagement.IAdminCursosService, AdminCursosService>();
        services.AddScoped<IUsuariosQueryService, UsuariosQueryService>();

        return services;
    }
}