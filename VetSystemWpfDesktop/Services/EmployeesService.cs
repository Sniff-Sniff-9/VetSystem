using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.Workday;

namespace VetSystemWpfDesktop.Services
{
    public class EmployeesService
    {
        private readonly HttpClient _httpClient;

        public EmployeesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmployeeDto>?> GetEmployeessAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EmployeeDto>>("Employees");
        }
        public async Task<EmployeeDto?> GetClientAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<EmployeeDto?>($"Employees/{id}");
        }

        public async Task<List<WorkdayDto>?> GetWorkdaysByEmployeeIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<WorkdayDto>>($"Employees/{id}/Workdays");
        }
    }
}
