using Microsoft.JSInterop;
using System.Net.Http.Json;
using VetSystemModels.Dto.Service;
using VetSystemModels.Dto.Employee;

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
            return await _httpClient.GetFromJsonAsync<List<ServiceDto>?>("Services") ?? new();
        }

        public async Task<ServiceDto> GetServiceAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ServiceDto>($"Services/{id}") ?? new();
        }

        public async Task<List<EmployeeDto>?> GetEmployeesByServiceIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<EmployeeDto>>($"Services/{id}/Employees") ?? new();
        }
    }
}
