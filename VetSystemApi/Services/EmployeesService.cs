using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class EmployeesService: IEmployeesService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeesService> _logger;

        public EmployeesService(AppDbContext context, ILogger<EmployeesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _context.Employees.Include(e => e.Specialization).ToListAsync();
            return employees.Select(e => ToEmployeeDto(e)).ToList();
        }

        public async Task<EmployeeDto?> GetEmployeeByEmployeeIdAsync(int id)
        {
            var employee = await _context.Employees.Include(e => e.Specialization).FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null)
            {
                return null!;
            }
            return ToEmployeeDto(employee);
        }

        public async Task<EmployeeDto?> GetEmployeeByUserIdAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(c => c.UserId == id);
            if (employee == null)
            {
                return null!;
            }
            return ToEmployeeDto(employee);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
        {

            if (employeeDto.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException($"Birth date can't be larger than {DateOnly.FromDateTime(DateTime.UtcNow)}");
            }

            var employee = new Employee
            {
                LastName = employeeDto.LastName,
                FirstName = employeeDto.FirstName,
                MiddleName = employeeDto.MiddleName,
                BirthDate = employeeDto.BirthDate,
                Phone = employeeDto.Phone,
                UserId = employeeDto.UserId
            };
            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return ToEmployeeDto(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee can't be created.");
                throw;
            }
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _context.Employees.Include(e => e.Specialization).FirstOrDefaultAsync(u => u.EmployeeId == id);

            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found.");
            }

            employee.FirstName = updateEmployeeDto.FirstName;
            employee.LastName = updateEmployeeDto.LastName;
            employee.MiddleName = updateEmployeeDto.MiddleName;
            employee.BirthDate = updateEmployeeDto.BirthDate;
            employee.SpecializationId = updateEmployeeDto.SpecializationId; 
            employee.Phone = updateEmployeeDto.Phone;

            try
            {
                await _context.SaveChangesAsync();
                return ToEmployeeDto(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee can't be updated.");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(u => u.UserId == id);
            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found.");
            }
            try
            {
                employee.IsDeleted = true; 
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee can't be deleted.");
                throw;
            }

        }
        private EmployeeDto ToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                BirthDate = employee.BirthDate,
                Phone = employee.Phone,
                SpecializationName = employee.Specialization?.SpecializationName ?? "undefined",
                UserId = employee.UserId
            };
        }
    }
}
