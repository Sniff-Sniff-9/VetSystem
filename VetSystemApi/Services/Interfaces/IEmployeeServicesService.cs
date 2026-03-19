using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.EmployeeService;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Services.Interfaces
{
    public interface IEmployeeServicesService
    {
        public Task<List<EmployeeServiceDto>> GetAllEmployeeServicesAsync();
        public Task<List<ServiceDto>> GetServicesByEmployeeIdAsync(int id);
        public Task<List<EmployeeDto>> GetEmployeesByServiceIdAsync(int id);
        public Task<EmployeeServiceDto?> GetEmployeeServiceByIdAsync(int id);
        public Task<EmployeeServiceDto> CreateEmployeeServiceAsync(CreateUpdateEmployeeServiceDto employeeServiceDto);
        public Task<EmployeeServiceDto> UpdateEmployeeServiceAsync(int id, CreateUpdateEmployeeServiceDto employeeServiceDto);
        public Task DeleteEmployeeServiceAsync(int id);

    }
}
