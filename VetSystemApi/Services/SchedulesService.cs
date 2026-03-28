using Microsoft.EntityFrameworkCore;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Schedule;
using VetSystemModels.Entities;
using VetSystemApi.Services;
using VetSystemApi.Services.Interfaces;

namespace VetSystemApi.Services
{
    public class SchedulesService: ISchedulesService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SchedulesService> _logger;

        public SchedulesService(AppDbContext context, ILogger<SchedulesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ScheduleDto>> GetSchedulesAsync()
        {
            var schedules = await _context.Schedules.Include(s => s.Workday).ToListAsync();
            return schedules.Select(c => ToScheduleDto(c)).ToList();
        }

        public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var schedule = await _context.Schedules.Include(s => s.Workday).FirstOrDefaultAsync(c => c.ScheduleId == id);
            if (schedule == null)
            {
                return null!;
            }
            return ToScheduleDto(schedule);
        }

        public async Task<List<ScheduleDto>> GetSchedulesByWorkdayIdAsync(int id)
        {
            var schedules = await _context.Schedules.Include(s => s.Workday).Where(c => c.WorkdayId == id).ToListAsync();
            return schedules.Select(s => ToScheduleDto(s)).ToList();
        }

        public async Task<ScheduleDto> CreateScheduleAsync(CreateUpdateScheduleDto scheduleDto)
        {

            if (!await IsScheduleTimeValidAsync(scheduleDto))
            {
                throw new ArgumentException("Schedule times aren't valid.");
            }

            var schedule = new Schedule
            {
                StartTime = scheduleDto.StartTime,
                EndTime = scheduleDto.EndTime,
                WorkdayId = scheduleDto.WorkdayId
            };
            try
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                var result = await _context.Schedules.Include(s => s.Workday).FirstAsync(c => c.ScheduleId == schedule.ScheduleId);
                return ToScheduleDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Schedule can't be created.");
                throw;
            }
        }

        public async Task<ScheduleDto> UpdateScheduleAsync(int id, CreateUpdateScheduleDto scheduleDto)
        {
            var schedule = await _context.Schedules.Include(s => s.Workday).FirstOrDefaultAsync(u => u.ScheduleId == id);

            if (schedule == null)
            {
                throw new ArgumentNullException("Schedule not found.");
            }

            if (!await IsScheduleTimeValidAsync(scheduleDto))
            {
                throw new ArgumentException("Schedule times aren't valid.");
            }

            schedule.StartTime = scheduleDto.StartTime;
            schedule.EndTime = scheduleDto.EndTime;
            schedule.WorkdayId = scheduleDto.WorkdayId;

            try
            {
                await _context.SaveChangesAsync();
                return ToScheduleDto(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Schedule can't be updated.");
                throw;
            }
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _context.Schedules.Include(s => s.Workday).FirstOrDefaultAsync(c => c.ScheduleId == id);
            if (schedule == null)
            {
                throw new ArgumentNullException("Schedule not found.");
            }
            schedule.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Schedule can't be deleted.");
                throw;
            }
        }

        private ScheduleDto ToScheduleDto(Schedule schedule)
        {
            return new ScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                WorkdayId = schedule.WorkdayId,
                WorkDate = schedule.Workday != null ? schedule.Workday.WorkDate : DateOnly.MinValue
            };
        }

        private async Task<bool> IsScheduleTimeValidAsync(CreateUpdateScheduleDto scheduleDto)
        {
            var workday = await _context.Workdays.FirstOrDefaultAsync(w => w.WorkdayId == scheduleDto.WorkdayId);

            if (workday == null)
            {
                throw new ArgumentException("Workday doesn't exist.");
            }
            if(scheduleDto.StartTime > scheduleDto.EndTime)
            {
                return false;
            }
            if (scheduleDto.StartTime < workday.StartTime)
            {
                return false;
            }
            if (scheduleDto.EndTime > workday.EndTime)
            {
                return false;
            }
            if (scheduleDto.StartTime >= workday.LunchStart && scheduleDto.StartTime <= workday.LunchEnd)
            {
                return false;
            }
            if (scheduleDto.EndTime >= workday.LunchStart && scheduleDto.EndTime <= workday.LunchEnd)
            {
                return false;
            }

            return true;
        }
    }
}
