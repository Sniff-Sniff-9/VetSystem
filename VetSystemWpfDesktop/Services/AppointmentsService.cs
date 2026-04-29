using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.AppointmentService;
using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Dto.Service;
using VetSystemModels.Entities;

namespace VetSystemWpfDesktop.Services
{
    public class AppointmentsService
    {
        private readonly HttpClient _httpClient;

        public AppointmentsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppointmentDto>?> GetAppointmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>("Appointments");
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AppointmentDto>($"Appointments/{id}");
        }

        public async Task<List<AppointmentServiceDto>?> GetServicesByAppointmentIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentServiceDto>>($"Appointments/{id}/Services");
        }

        public async Task<List<AppointmentDto>?> GetAppointmentsByEmployeeIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>($"Employees/{id}/Appointments");
        }

        public async Task<List<TimeOnly>?> GetAvailableSlotsAsync(int id, DateOnly date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");

            return await _httpClient.GetFromJsonAsync<List<TimeOnly>>(
                $"ScheduleAvailability?EmployeeId={id}&ScheduleAvailabilityDate={formattedDate}");
        }

        public async Task<List<TimeOnly>?> GetAllSlotsAsync(int id, DateOnly date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");

            return await _httpClient.GetFromJsonAsync<List<TimeOnly>>(
                $"ScheduleAvailability/All?EmployeeId={id}&ScheduleAvailabilityDate={formattedDate}");
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(int id, CreateUpdateAppointmentDto appointmentDto)
        {
            
            var response = await _httpClient.PutAsJsonAsync($"Appointments/{id}", appointmentDto);

            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {error}");
            }

            return await response.Content.ReadFromJsonAsync<AppointmentDto>() ?? new();
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(DateOnly date, EmployeeDto employee, ServiceDto service, PetDto pet,
            TimeOnly slot, int appointmentStatusId)
        {
            var appointment = new CreateUpdateAppointmentDto
            {
                AppointmentDate = date,
                StartTime = slot,
                AppointmentStatusId = appointmentStatusId,
                PetId = pet.PetId,
                EmployeeId = employee.EmployeeId,
                ServiceId = service.ServiceId,
            };

            var response = await _httpClient.PostAsJsonAsync("Appointments", appointment);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AppointmentDto>() ?? new();

        }

        public async Task<AppointmentDto> CreateAppointmentServiceAsync(CreateAppointmentServiceDto appointment)
        {


            var response = await _httpClient.PostAsJsonAsync("AppointmentServices", appointment);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AppointmentDto>() ?? new();

        }

        public async Task<List<AppointmentStatus>> GetStatusesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentStatus>>("AppointmentStatuses") ?? new();
        }

        public async Task<AppointmentStatus> GetStatusByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AppointmentStatus>($"AppointmentStatuses/{id}") ?? new();
        }

        public async Task DeleteAppointmentServiceAsync(int id)
        {
            await _httpClient.DeleteAsync($"AppointmentServices/{id}");
        }
        public async Task DeleteAppointmentAsync(int id)
        {
            await _httpClient.DeleteAsync($"Appointments/{id}");
        }
    }
}

