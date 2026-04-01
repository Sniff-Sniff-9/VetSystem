using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Service;
using VetSystemModels.Dto.Employee;

namespace VetSystemWpfDesktop.Services
{
    public class ServicesService
    {
        private readonly HttpClient _httpClient;

        public ServicesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ServiceDto>?> GetServicesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ServiceDto>>("Services");
        }
        public async Task<ServiceDto?> GetServiceAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ServiceDto?>($"Services/{id}");
        }

        public async Task<List<EmployeeDto>?> GetEmployeesByServiceIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<EmployeeDto>>($"Services/{id}/Employees");
        }
    }
}
