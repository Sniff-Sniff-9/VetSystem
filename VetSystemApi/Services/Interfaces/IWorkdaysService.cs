using VetSystemModels.Dto.Workday;

namespace VetSystemApi.Services.Interfaces
{
    public interface IWorkdaysService
    {
        public Task<List<WorkdayDto>> GetWorkdaysAsync();
        public Task<WorkdayDto?> GetWorkdayByIdAsync(int id);
        public Task<List<WorkdayDto>> GetWorkdaysByEmployeeIdAsync(int id);
        public Task<WorkdayDto> CreateWorkdayAsync(CreateUpdateWorkdayDto workdayDto);
        public Task<WorkdayDto> UpdateWorkdayAsync(int id, CreateUpdateWorkdayDto workdayDto);
        public Task DeleteWorkdayAsync(int id);
    }
}
