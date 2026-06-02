namespace TajamarFaltas.Domain.Interfaces;

public interface ITajamarApiClientService
{
    Task<string> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default);

    Task<TResponse?> GetAsync<TResponse>(string relativeUrl, CancellationToken cancellationToken = default);

    Task<TResponse?> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest body, CancellationToken cancellationToken = default);

    // Post without adding the admin bearer header (useful for authenticating end users)
    Task<TResponse?> PostAnonymousAsync<TRequest, TResponse>(string relativeUrl, TRequest body, CancellationToken cancellationToken = default);
}