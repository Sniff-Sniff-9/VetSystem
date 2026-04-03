using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Service;
using VetSystemModels.Dto.Employee ;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VetSystemModels.Dto.Pet;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace VetSystemWebApplication.Services
{
    public class AppointmentsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ServicesService _servicesService;

        public AppointmentsService(HttpClient httpClient, ILocalStorageService localStorage, ServicesService servicesService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _servicesService = servicesService;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByClientIdAsync()
        {
            var token = _localStorage.GetItem<string>("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>("Client/Appointments") ?? new();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            await _httpClient.DeleteAsync($"Appointments/{id}");
        }

        public async Task<List<TimeOnly>?> GetAvailableSlotsAsync(int id, DateOnly date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");

            return await _httpClient.GetFromJsonAsync<List<TimeOnly>>(
                $"ScheduleAvailability?EmployeeId={id}&ScheduleAvailabilityDate={formattedDate}");
        }

        public async Task<List<TimeOnly>> GetAvailableSlotsForServiceAsync(int id, DateOnly date)
        {
            var employees = await _servicesService.GetEmployeesByServiceIdAsync(id) ?? new();
            var slots = new List<TimeOnly>();

            foreach (EmployeeDto e in employees)
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var employeeSlots = await _httpClient.GetFromJsonAsync<List<TimeOnly>>(
                    $"ScheduleAvailability?EmployeeId={e.EmployeeId}&ScheduleAvailabilityDate={formattedDate}") ?? new();

                slots.AddRange(employeeSlots);
            }

            return slots.Distinct().OrderBy(t => t).ToList();
        }

        public async Task<List<EmployeeDto>> GetEmployeesBySlotAsync(int id, DateOnly date, TimeOnly slot)
        {
            var employees = await _servicesService.GetEmployeesByServiceIdAsync(id) ?? new();
            var availableEmployees = new List<EmployeeDto>();

            var formattedDate = date.ToString("yyyy-MM-dd");

            foreach (var e in employees)
            {
                var employeeSlots = await _httpClient.GetFromJsonAsync<List<TimeOnly>>(
                    $"ScheduleAvailability?EmployeeId={e.EmployeeId}&ScheduleAvailabilityDate={formattedDate}") ?? new();

                if (employeeSlots.Contains(slot))
                {
                    availableEmployees.Add(e);
                }
            }

            return availableEmployees;
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(DateOnly date, EmployeeDto employee, ServiceDto service, PetDto pet,
            TimeOnly slot)
        {
            var appointment = new CreateUpdateAppointmentDto
            {
                AppointmentDate = date,
                StartTime = slot,
                AppointmentStatusId = 1,
                PetId = pet.PetId,
                EmployeeId = employee.EmployeeId,
                ServiceId = service.ServiceId
            };

            var response = await _httpClient.PostAsJsonAsync("Appointments", appointment);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AppointmentDto>() ?? new();

        }

    }
}