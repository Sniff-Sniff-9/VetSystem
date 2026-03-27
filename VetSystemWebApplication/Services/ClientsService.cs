using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.User;

namespace VetSystemWebApplication.Services
{
    public class ClientsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ClientsService(HttpClient httpClient, IJSRuntime js, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<ClientDto?> GetClientByUserIdAsync()
        {
            var token = _localStorage.GetItem<string>("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.GetFromJsonAsync<ClientDto?>("Users/Client") ?? new();
        }

        public async Task<UserDto?> GetUserAsync()
        {
            var token = _localStorage.GetItem<string>("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.GetFromJsonAsync<UserDto?>("User") ?? new();
        }

        
    }
}
