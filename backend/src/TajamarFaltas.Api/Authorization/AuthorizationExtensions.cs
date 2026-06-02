using Microsoft.AspNetCore.Authorization;

namespace TajamarFaltas.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddTajamarAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.AlumnoOnly, policy => policy.RequireRole("Alumno"));
            options.AddPolicy(PolicyNames.ProfesorOnly, policy => policy.RequireRole("Profesor"));
            options.AddPolicy(PolicyNames.AdministradorOnly, policy => policy.RequireRole("Administrador"));
        });

        return services;
    }
}