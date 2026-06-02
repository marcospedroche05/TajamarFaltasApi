using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TajamarFaltas.Domain.Interfaces;
using TajamarFaltas.Infrastructure.Options;

namespace TajamarFaltas.Infrastructure.ExternalServices;

public sealed class TajamarApiClientService : ITajamarApiClientService
{
    private const string ExternalAuthLoginPath = "/api/Auth/Login";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private readonly HttpClient _httpClient;
    private readonly TajamarApiOptions _options;
    private string? _adminAccessToken;
    private DateTimeOffset _adminTokenExpiresAt = DateTimeOffset.MinValue;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    public TajamarApiClientService(HttpClient httpClient, IOptions<TajamarApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_adminAccessToken) && _adminTokenExpiresAt > DateTimeOffset.UtcNow)
        {
            return _adminAccessToken;
        }

        await _tokenLock.WaitAsync(cancellationToken);
        try
        {
            if (!string.IsNullOrWhiteSpace(_adminAccessToken) && _adminTokenExpiresAt > DateTimeOffset.UtcNow)
            {
                return _adminAccessToken;
            }

            using var response = await PostExternalLoginRawJsonAsync(
                ExternalAuthLoginPath,
                _options.AdminUser,
                _options.AdminPassword,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<TokenResponse>(JsonOptions, cancellationToken)
                ?? throw new InvalidOperationException("Tajamar admin login response was empty.");

            _adminAccessToken = payload.AccessToken;
            _adminTokenExpiresAt = DateTimeOffset.UtcNow.AddMinutes(Math.Max(payload.ExpiresIn <= 0 ? 60 : payload.ExpiresIn, 5));

            return _adminAccessToken;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    public async Task<TResponse?> GetAsync<TResponse>(string relativeUrl, CancellationToken cancellationToken = default)
    {
        using var request = await CreateAuthorizedRequestAsync(HttpMethod.Get, relativeUrl, cancellationToken);
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest body, CancellationToken cancellationToken = default)
    {
        using var request = await CreateAuthorizedRequestAsync(HttpMethod.Post, relativeUrl, cancellationToken);
        request.Content = JsonContent.Create(body, options: JsonOptions);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
    }

    public async Task<TResponse?> PostAnonymousAsync<TRequest, TResponse>(string relativeUrl, TRequest body, CancellationToken cancellationToken = default)
    {
        if (string.Equals(relativeUrl, ExternalAuthLoginPath, StringComparison.Ordinal)
            && TryExtractLoginCredentials(body, out var userName, out var password))
        {
            using var loginResponse = await PostExternalLoginRawJsonAsync(relativeUrl, userName, password, cancellationToken);
            loginResponse.EnsureSuccessStatusCode();
            return await loginResponse.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
        }

        using var response = await _httpClient.PostAsJsonAsync(relativeUrl, body, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
    }

    private async Task<HttpRequestMessage> CreateAuthorizedRequestAsync(HttpMethod method, string relativeUrl, CancellationToken cancellationToken)
    {
        var token = await GetAdminAccessTokenAsync(cancellationToken);
        var request = new HttpRequestMessage(method, relativeUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private async Task<HttpResponseMessage> PostExternalLoginRawJsonAsync(string relativeUrl, string userName, string password, CancellationToken cancellationToken)
    {
        // Build raw JSON body explicitly for strict payload casing required by external API.
        string jsonBody = $"{{\"userName\":\"{userName}\",\"password\":\"{password}\"}}";
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var absoluteUrl = _httpClient.BaseAddress is null
            ? relativeUrl
            : new Uri(_httpClient.BaseAddress, relativeUrl).ToString();

        Console.WriteLine($"[TajamarApiClientService] POST URL: {absoluteUrl}");
        Console.WriteLine($"[TajamarApiClientService] POST JSON: {jsonBody}");

        return await _httpClient.PostAsync(relativeUrl, content, cancellationToken);
    }

    private static bool TryExtractLoginCredentials<TRequest>(TRequest body, out string userName, out string password)
    {
        userName = string.Empty;
        password = string.Empty;

        if (body is null)
        {
            return false;
        }

        var bodyType = body.GetType();
        var userNameProperty = bodyType.GetProperty("UserName");
        var passwordProperty = bodyType.GetProperty("Password");

        if (userNameProperty is null || passwordProperty is null)
        {
            return false;
        }

        userName = userNameProperty.GetValue(body)?.ToString() ?? string.Empty;
        password = passwordProperty.GetValue(body)?.ToString() ?? string.Empty;

        return !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password);
    }

    private sealed record TokenResponse(string AccessToken, int ExpiresIn);
}