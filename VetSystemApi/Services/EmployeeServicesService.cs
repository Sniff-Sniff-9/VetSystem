using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.EmployeeService;
using VetSystemModels.Dto.Service;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class EmployeeServicesService: IEmployeeServicesService
    {
        private readonly AppDbContext _context;
        private ILogger<EmployeeServicesService> _logger;

        public EmployeeServicesService(AppDbContext context, ILogger<EmployeeServicesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<EmployeeServiceDto>> GetAllEmployeeServicesAsync()
        {
            var services = await _context.EmployeeServices.ToListAsync();
            return services.Select(es => ToEmployeeServiceDto(es)).ToList();
        }

        public async Task<List<ServiceDto>> GetServicesByEmployeeIdAsync(int id)
        {
            var services = await _context.EmployeeServices.Where(es => es.EmployeeId == id).Include(es => es.Service).Select(es => es.Service).ToListAsync();
            return services.Select(s => ToServiceDto(s)).ToList();
        }

        public async Task<List<EmployeeDto>> GetEmployeesByServiceIdAsync(int id)
        {
            var employees = await _context.EmployeeServices.Where(es => es.ServiceId == id).Include(es => es.Employee).Include(es => es.Employee.Specialization).Select(es => es.Employee).ToListAsync();
            return employees.Select(e => ToEmployeeDto(e)).ToList();
        }

        public async Task<EmployeeServiceDto?> GetEmployeeServiceByIdAsync(int id)
        {
            var employeeService = await _context.EmployeeServices.FirstOrDefaultAsync(es => es.EmployeeServiceId == id);
            if (employeeService == null)
            {
                return null;
            }
            return ToEmployeeServiceDto(employeeService);
        }

        public async Task<EmployeeServiceDto> CreateEmployeeServiceAsync(CreateUpdateEmployeeServiceDto employeeServiceDto)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == employeeServiceDto.EmployeeId);
            if (!employeeExists)
            {
                throw new ArgumentException("Employee doesn't exist.");
            }

            var serviceExists = await _context.Services.AnyAsync(s => s.ServiceId == employeeServiceDto.ServiceId);
            if (!serviceExists)
            {
                throw new ArgumentException("Service doesn't exist.");
            }

            var relationExists = await _context.EmployeeServices.AnyAsync(es => es.EmployeeId == employeeServiceDto.EmployeeId
                 && es.ServiceId == employeeServiceDto.ServiceId
                 && !es.IsDeleted);

            if (relationExists)
            {
                throw new ArgumentException("Employee already has this service.");
            }

            var employeeService = new EmployeeService
            {
                EmployeeId = employeeServiceDto.EmployeeId,
                ServiceId = employeeServiceDto.ServiceId
            };
            try
            {
                _context.Add(employeeService);
                await _context.SaveChangesAsync();
                return ToEmployeeServiceDto(employeeService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee's service can't be created.");
                throw;
            }
        }

        public async Task<EmployeeServiceDto> UpdateEmployeeServiceAsync(int id, CreateUpdateEmployeeServiceDto employeeServiceDto)
        {
            var employeeService = await _context.EmployeeServices.FirstOrDefaultAsync(es => es.EmployeeServiceId == id);
            if (employeeService == null)
            {
                throw new ArgumentException("Employee's service not found.");
            }

            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == employeeServiceDto.EmployeeId);
            if (!employeeExists)
            {
                throw new ArgumentException("Employee  doesn't exist.");
            }

            var serviceExists = await _context.Services.AnyAsync(e => e.ServiceId == employeeServiceDto.ServiceId);
            if (!serviceExists)
            {
                throw new ArgumentException("Service doesn't exist.");
            }

            var relationExists = await _context.EmployeeServices.AnyAsync(es => es.EmployeeId == employeeServiceDto.EmployeeId
                 && es.ServiceId == employeeServiceDto.ServiceId
                 && !es.IsDeleted);

            if (relationExists)
            {
                throw new ArgumentException("Employee already has this service.");
            }

            employeeService.ServiceId = employeeServiceDto.ServiceId;
            employeeService.EmployeeId = employeeServiceDto.EmployeeId;

            try
            {
                await _context.SaveChangesAsync();
                return ToEmployeeServiceDto(employeeService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee's service can't be updated.");
                throw;
            }
        }

        public async Task DeleteEmployeeServiceAsync(int id)
        {
            var employeeService = await _context.EmployeeServices.FirstOrDefaultAsync(es => es.EmployeeServiceId == id);
            if (employeeService == null)
            {
                throw new ArgumentNullException("Employee's service not found.");
            }
            employeeService.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Employee's service can't be deleted.");
                throw;
            }
        }

        private ServiceDto ToServiceDto(Service service)
        {
            return new ServiceDto()
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes
            };
        }

        private EmployeeDto ToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                BirthDate = employee.BirthDate,
                Phone = employee.Phone,
                SpecializationName = employee.Specialization?.SpecializationName ?? "undefined",
                SpecializationId = employee.SpecializationId,
                UserId = employee.UserId
            };
        }

        private EmployeeServiceDto ToEmployeeServiceDto(EmployeeService employeeService)
        {
            return new EmployeeServiceDto()
            {
                EmployeeServiceId = employeeService.EmployeeServiceId,
                EmployeeId = employeeService.EmployeeId,
                ServiceId = employeeService.ServiceId
            };
        }
    }
}
