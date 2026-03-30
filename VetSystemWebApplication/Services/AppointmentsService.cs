using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VetSystemModels.Dto.Appointment;

namespace VetSystemWebApplication.Services
{
    public class VetServicesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public VetServicesService(HttpClient httpClient, IJSRuntime js, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<AppointmentDto>> GetServicesByClientIdAsync()
        {
            var token = _localStorage.GetItem<string>("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>("Client/Services") ?? new();
        }
    }
}