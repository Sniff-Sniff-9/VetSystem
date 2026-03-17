using VetSystemModels.Dto.Schedule;

namespace VetSystemApi.Services.Interfaces
{
    public interface ISchedulesService
    {
        public Task<List<ScheduleDto>> GetSchedulesAsync();
        public Task<ScheduleDto?> GetScheduleByIdAsync(int id);
        public Task<List<ScheduleDto>> GetSchedulesByWorkdayIdAsync(int id);
        public Task<ScheduleDto> CreateScheduleAsync(CreateUpdateScheduleDto ScheduleDto);
        public Task<ScheduleDto> UpdateScheduleAsync(int id, CreateUpdateScheduleDto ScheduleDto);
        public Task DeleteScheduleAsync(int id);
    }
}
