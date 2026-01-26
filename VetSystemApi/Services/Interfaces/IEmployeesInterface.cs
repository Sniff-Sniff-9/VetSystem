using VetSystemModels.Dto;

namespace VetSystemApi.Services.Interfaces
{
    public interface IEmployeesInterface
    {
        public Task<List<EmployeeDto>> GetEmployeesAsync();
        public Task<EmployeeDto?> GetEmployeeByEmployeeIdAsync(int id);
        public Task<EmployeeDto?> GetEmployeeByUserIdAsync(int id);
        public Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto EmployeeDto);
        public Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
        public Task DeleteEmployeeAsync(int id);
    }
}
