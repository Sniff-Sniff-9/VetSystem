using VetSystemModels.Dto.WorkdayOverride;

namespace VetSystemApi.Services.Interfaces
{
    public interface IWorkdayOverridesService
    {
        public Task<WorkdayOverrideDto> CreateWorkdayOverridesAsync(CreateUpdateWorkdayOverrideDto workdayOverrideDto);
    }
}
