using VetSystemModels.Dto.ScheduleAvailability;
using VetSystemModels.Dto.Workday;

namespace VetSystemApi.Services.Interfaces
{
    public interface IScheduleAvailabilityService
    {
        public Task<List<TimeOnly>> GetAvailableSlotsAsync(ScheduleAvailabilityDto scheduleAvailabilityDto);
        public Task<List<TimeOnly>> GetAllSlotsAsync(ScheduleAvailabilityDto scheduleAvailabilityDto);
    }
}
