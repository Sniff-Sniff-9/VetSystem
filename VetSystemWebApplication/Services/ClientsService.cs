using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Dto.User;
using VetSystemModels.Entities;

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

        public async Task<ClientDto?> UpdateClientAsync(UpdateClientDto clientDto)
        {
            var client = await _httpClient.GetFromJsonAsync<ClientDto?>("Users/Client") ?? new();
            var response = await _httpClient.PutAsJsonAsync($"Clients/{client.ClientId}", clientDto);

            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {error}");
            }

            return await response.Content.ReadFromJsonAsync<ClientDto>() ?? new();
        }

        
    }
}
