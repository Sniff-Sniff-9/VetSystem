using Microsoft.EntityFrameworkCore;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.AppointmentService;
using VetSystemModels.Dto.WorkdayOverride;
using VetSystemModels.Entities;
using VetSystemApi.Services.Interfaces;

namespace VetSystemApi.Services
{
    public class WorkdayOverridesService: IWorkdayOverridesService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WorkdayOverridesService> _logger;

        public WorkdayOverridesService(AppDbContext context, ILogger<WorkdayOverridesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<WorkdayOverrideDto> CreateWorkdayOverridesAsync(CreateUpdateWorkdayOverrideDto workdayOverrideDto)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == workdayOverrideDto.EmployeeId);

            if (!employeeExists)
                throw new ArgumentException("Employee doesn't exist.");

            if (workdayOverrideDto.StartTime >= workdayOverrideDto.EndTime)
                throw new ArgumentException("StartTime must be earlier than EndTime");

            var overlap = await _context.WorkdayOverrides.AnyAsync(o =>
                            o.EmployeeId == workdayOverrideDto.EmployeeId &&
                            o.WorkdayOverrideDate == workdayOverrideDto.WorkdayOverrideDate &&
                            workdayOverrideDto.StartTime < o.EndTime &&
                            workdayOverrideDto.EndTime > o.StartTime);

            if (overlap)
                throw new ArgumentException("Override overlaps with existing blocked time.");

            var workdayOverride = new WorkdayOverride
            {
                EmployeeId = workdayOverrideDto.EmployeeId,
                WorkdayOverrideDate = workdayOverrideDto.WorkdayOverrideDate,
                StartTime = workdayOverrideDto.StartTime,
                EndTime = workdayOverrideDto.EndTime
            };

            try
            {
                _context.WorkdayOverrides.Add(workdayOverride);
                await _context.SaveChangesAsync();
                var result = await _context.WorkdayOverrides.Include(wo => wo.Employee).
                    FirstAsync(wo => wo.EmployeeId == workdayOverride.EmployeeId);
                return ToWorkdayOverrideDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Workday override can't be created.");
                throw;
            }
        }

        private WorkdayOverrideDto ToWorkdayOverrideDto(WorkdayOverride workdayOverride)
        {
            return new WorkdayOverrideDto()
            {
                EmployeeId = workdayOverride.EmployeeId,
                EmployeeName = workdayOverride.Employee != null
                ? $"{workdayOverride.Employee.LastName} {workdayOverride.Employee.FirstName} " +
                $"{workdayOverride.Employee.MiddleName}" : "undefined",
                WorkdayOverrideDate = workdayOverride.WorkdayOverrideDate,
                StartTime = workdayOverride.StartTime,
                EndTime = workdayOverride.EndTime
            };
        }
    }
}