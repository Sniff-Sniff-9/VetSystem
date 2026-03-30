using Microsoft.JSInterop;
using System.Net.Http.Json;
using VetSystemModels.Dto.Service;

namespace VetSystemWebApplication.Services
{
    public class ServicesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ServicesService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<ServiceDto>> GetAllServicesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ServiceDto>?>("/Services") ?? new();
        }
    }
}
