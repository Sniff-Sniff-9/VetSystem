using VetSystemModels.Dto.Schedule;
using VetSystemModels.Dto.Workday;

namespace VetSystemApi.Services.Interfaces
{
    public interface IScheduleGenerationService
    {
        public Task<List<ScheduleDto>> GenerateScheduleForWorkdayAsync(int id);
        public Task<Dictionary<int, List<ScheduleDto>>> GenerateSchedulesForWeekAsync(List<WorkdayDto> workdays);
    }
}
