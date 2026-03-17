using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VetSystemInfrastructure.Configuration;
using VetSystemInfrastructure.Migrations;
using VetSystemModels.Dto.Workday;
using VetSystemModels.Entities;
using VetSystemApi.Services.Interfaces;

namespace VetSystemApi.Workdays
{
    public class WorkdaysService: IWorkdaysService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WorkdaysService> _logger;

        public WorkdaysService(AppDbContext context, ILogger<WorkdaysService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<WorkdayDto>> GetWorkdaysAsync()
        {
            var workdays = await _context.Workdays.Include(w => w.Employee).ToListAsync();
            return workdays.Select(w => ToWorkdayDto(w)).ToList();
        }

        public async Task<WorkdayDto?> GetWorkdayByIdAsync(int id)
        {
            var workday = await _context.Workdays.Include(w => w.Employee).FirstOrDefaultAsync(s => s.WorkdayId == id);
            if (workday == null)
            {
                return null;
            }
            return ToWorkdayDto(workday);
        }

        public async Task<List<WorkdayDto>> GetWorkdaysByEmployeeIdAsync(int id)
        {
            var workdays = await _context.Workdays.Include(w => w.Employee).Where(s => s.EmployeeId == id).ToListAsync();
            return workdays.Select(w => ToWorkdayDto(w)).ToList();
        }

        public async Task<WorkdayDto> CreateWorkdayAsync(CreateUpdateWorkdayDto workdayDto)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == workdayDto.EmployeeId);
            if (!employeeExists)
            {
                throw new ArgumentException("Employee doesn't exist.");
            }

            var workdayExists = await _context.Workdays
                                .AnyAsync(w => w.EmployeeId == workdayDto.EmployeeId &&
                                w.WorkDate == workdayDto.WorkDate);

            if (workdayExists)
            {
                throw new ArgumentException("Workday for this day already exist.");
            }

            if (!IsWorkdayTimeValid(workdayDto))
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            var workday = new Workday()
            {
                EmployeeId = workdayDto.EmployeeId,
                WorkDate = workdayDto.WorkDate,
                StartTime = workdayDto.StartTime,
                EndTime = workdayDto.EndTime,
                LunchStart = workdayDto.LunchStart,
                LunchEnd = workdayDto.LunchEnd,
                SlotDuration = workdayDto.SlotDuration
            };
            try
            {
                _context.Add(workday);
                await _context.SaveChangesAsync();

                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == workday.EmployeeId);

                return ToWorkdayDto(workday);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Workday can't be created.");
                throw;
            }
        }

        public async Task<WorkdayDto> UpdateWorkdayAsync(int id, CreateUpdateWorkdayDto workdayDto)
        {
            var workday = _context.Workdays.Include(w => w.Employee).FirstOrDefault(s => s.WorkdayId == id);

            if (workday == null)
            {
                throw new ArgumentNullException("Workday not found.");
            }
            workday.EmployeeId = workdayDto.EmployeeId;
            workday.WorkDate = workdayDto.WorkDate;
            workday.StartTime = workdayDto.StartTime;
            workday.EndTime = workdayDto.EndTime;
            workday.LunchEnd = workdayDto.LunchEnd;
            workday.LunchStart = workdayDto.LunchStart;
            workday.SlotDuration = workdayDto.SlotDuration;
            try
            {
                await _context.SaveChangesAsync();
                return ToWorkdayDto(workday);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Workday can't be updated.");
                throw;
            }
        }

        public async Task DeleteWorkdayAsync(int id)
        {
            var workday = _context.Workdays.FirstOrDefault(s => s.WorkdayId == id);
            if (workday == null)
            {
                throw new ArgumentNullException("Workday not found.");
            }
            workday.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Workday can't be deleted.");
                throw;
            }
        }
        private WorkdayDto ToWorkdayDto(Workday workday)
        {
            return new WorkdayDto()
            {
                WorkdayId = workday.WorkdayId,
                EmployeeId = workday.EmployeeId,
                EmployeeName = workday.Employee != null ? $"{workday.Employee.LastName} {workday.Employee.FirstName} {workday.Employee.MiddleName}" : "undefiend",
                WorkDate = workday.WorkDate,
                StartTime = workday.StartTime,
                EndTime = workday.EndTime,
                LunchEnd = workday.LunchEnd,
                LunchStart = workday.LunchStart,
                SlotDuration = workday.SlotDuration
            };
        }

        private bool IsWorkdayTimeValid(CreateUpdateWorkdayDto workdayDto)
        {
            if (workdayDto.StartTime >= workdayDto.EndTime)
            {
                return false;
            }
            if (workdayDto.LunchStart  >= workdayDto.LunchEnd)
            {
                return false;
            }
            if (workdayDto.LunchStart < workdayDto.StartTime)
            {
                return false;
            }
            if (workdayDto.LunchEnd > workdayDto.EndTime)
            {
                return false;
            }
            return true;
        }
    }
}
