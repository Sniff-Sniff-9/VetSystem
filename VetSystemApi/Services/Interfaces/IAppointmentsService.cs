using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Services.Interfaces
{
    public interface IAppointmentsService
    {
        public Task<List<AppointmentDto>> GetAppointmentsAsync();
        public Task<List<AppointmentDto>> GetAppointmentsByPetIdAsync(int id);
        public Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
        public Task<AppointmentDto> CreateAppointmentAsync(CreateUpdateAppointmentDto employeeServiceDto);
        public Task<AppointmentDto> UpdateAppointmentAsync(int id, CreateUpdateAppointmentDto employeeServiceDto);
        public Task DeleteAppointmentAsync(int id);
    }
}
