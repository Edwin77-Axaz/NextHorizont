using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Storage;
using NextHorizont.Models;

namespace NextHorizont.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string TokenKey = "jwt_token";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await SecureStorage.Default.GetAsync(TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync(endpoint);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await LogoutAsync();
            throw new UnauthorizedAccessException("Sesión expirada");
        }
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Podría ser un error de credenciales si es el login, así que leemos el ErrorResponse
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new UnauthorizedAccessException(error?.Detail ?? "No autorizado");
        }

        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))!;
    }

    public async Task SaveTokenAsync(string token)
    {
        await SecureStorage.Default.SetAsync(TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.Default.GetAsync(TokenKey);
    }

    public void RemoveToken()
    {
        SecureStorage.Default.Remove(TokenKey);
    }

    public async Task LogoutAsync()
    {
        RemoveToken();
        // Disparar evento global de sesión cerrada o recargar app
    }
}
