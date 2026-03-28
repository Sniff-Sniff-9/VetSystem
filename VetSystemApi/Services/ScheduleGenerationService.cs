using Microsoft.EntityFrameworkCore;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Schedule;
using VetSystemModels.Dto.Workday;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class ScheduleGenerationService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ScheduleGenerationService> _logger;

        public ScheduleGenerationService(AppDbContext context, ILogger<ScheduleGenerationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ScheduleDto>> GenerateScheduleForWorkdayAsync(Workday workday)
        {
            var schedules = new List<Schedule>();
            var current = workday.StartTime;

            while (current < workday.EndTime)
            {
                if (current >= workday.LunchStart && current < workday.LunchEnd)
                {
                    current = workday.LunchEnd;
                    continue;
                }

                var slotEnd = current.AddMinutes(workday.SlotDuration);

                if (slotEnd > workday.EndTime)
                    slotEnd = workday.EndTime;

                schedules.Add(new Schedule
                {
                    WorkdayId = workday.WorkdayId,
                    StartTime = current,
                    EndTime = slotEnd
                });

                current = slotEnd;
            }

            try
            {
                _context.Schedules.AddRange(schedules);
                await _context.SaveChangesAsync();

                var savedSchedules = await _context.Schedules
                    .Include(s => s.Workday)
                    .Where(s => s.WorkdayId == workday.WorkdayId)
                    .ToListAsync();

                return savedSchedules.Select(ToScheduleDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Schedule can't be created.");
                throw;
            }
        }

        public async Task<List<ScheduleDto>> GenerateSchedulesForDateAsync(DateOnly date)
        {
            var workdays = await _context.Workdays
                .Where(w => w.WorkDate == date)
                .ToListAsync();

            var result = new List<ScheduleDto>();

            foreach (var workday in workdays)
            {
                var schedules = await GenerateScheduleForWorkdayAsync(workday);
                result.AddRange(schedules);
            }

            return result;
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

    }
}
