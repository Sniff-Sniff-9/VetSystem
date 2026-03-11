using VetSystemModels.Dto.Employee;

namespace VetSystemApi.Services.Interfaces
{
    public interface IEmployeesService
    {
        public Task<List<EmployeeDto>> GetEmployeesAsync();
        public Task<EmployeeDto?> GetEmployeeByEmployeeIdAsync(int id);
        public Task<EmployeeDto?> GetEmployeeByUserIdAsync(int id);
        public Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        public Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
        public Task DeleteEmployeeAsync(int id);
    }
}
