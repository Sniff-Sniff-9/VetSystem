using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.ScheduleAvailability;
using VetSystemModels.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VetSystemApi.Services
{
    public class ScheduleAvailabilityService: IScheduleAvailabilityService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ScheduleAvailabilityService> _logger;

        public ScheduleAvailabilityService(AppDbContext context, ILogger<ScheduleAvailabilityService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TimeOnly>> GetAvailableSlotsAsync(ScheduleAvailabilityDto scheduleAvailabilityDto)
        {
            var dayOfWeek = scheduleAvailabilityDto.ScheduleAvailabilityDate.DayOfWeek.ToString();
            var workday = await _context.Workdays
                .Where(w => w.EmployeeId == scheduleAvailabilityDto.EmployeeId && w.DayOfWeek == dayOfWeek)
                .FirstOrDefaultAsync();

            if (workday == null)
                return new List<TimeOnly>(); 


            var slots = new List<TimeOnly>();
            var current = workday.StartTime;
            while (current.Add(TimeSpan.FromMinutes(workday.SlotDuration)) <= workday.EndTime)
            {
              
                if (!(current >= workday.LunchStart && current < workday.LunchEnd))
                    slots.Add(current);

                current = current.AddMinutes(workday.SlotDuration);
            }

  
            var overrides = await _context.WorkdayOverrides
                .Where(o => o.EmployeeId == scheduleAvailabilityDto.EmployeeId && o.WorkdayOverrideDate == scheduleAvailabilityDto.ScheduleAvailabilityDate)
                .ToListAsync();

            foreach (var ov in overrides)
            {
                slots = slots.Where(s => s < ov.StartTime || s >= ov.EndTime).ToList();
            }

   
            var occupiedSlots = await _context.Appointments
                .Where(a => a.EmployeeId == scheduleAvailabilityDto.EmployeeId &&
                            a.AppointmentDate == scheduleAvailabilityDto.ScheduleAvailabilityDate &&
                            !a.IsDeleted)
                .Select(a => a.StartTime)
                .ToListAsync();

            slots = slots.Where(s => !occupiedSlots.Contains(s)).ToList();

            return slots;
        }
    }
}