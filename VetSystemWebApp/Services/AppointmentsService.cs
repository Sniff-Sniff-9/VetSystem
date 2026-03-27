using Microsoft.JSInterop;
using System.Net.Http.Headers;
using VetSystemModels.Dto.Appointment;


namespace VetSystemWebApp.Services
{
    public class AppointmentsService
    {
        private readonly HttpClient _httpClient;

        public AppointmentsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByClientIdAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>("Client/Appointments") ?? new();
        }
    }
}

