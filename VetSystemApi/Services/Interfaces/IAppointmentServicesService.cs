using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.AppointmentService;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Services.Interfaces
{
    public interface IAppointmentServicesService
    {
        public Task<List<AppointmentServiceDto>> GetAppointmentServicesAsync();
        public Task<List<ServiceDto>> GetServicesByAppointmentIdAsync(int id);
        public Task<List<AppointmentDto>> GetAppointmentsByServiceIdAsync(int id);
        public Task<AppointmentServiceDto?> GetAppointmentServiceByIdAsync(int id);
        public Task<AppointmentServiceDto> CreateAppointmentServiceAsync(CreateAppointmentServiceDto AppointmentServiceDto);
        public Task DeleteAppointmentServiceAsync(int id);
    }
}
