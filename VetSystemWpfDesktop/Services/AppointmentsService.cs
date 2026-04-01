using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;

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

    }
}

