using VetSystemModels.Entities;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Services.Interfaces
{
    public interface IServicesService
    {
        public Task<List<ServiceDto>> GetServicesAsync();
        public Task<ServiceDto?> GetServiceByIdAsync(int id);
        public Task<ServiceDto> CreateServiceAsync(CreateUpdateServiceDto serviceDto);
        public Task<ServiceDto> UpdateServiceAsync(int id, CreateUpdateServiceDto serviceDto);
        public Task DeleteServiceAsync(int id);
    }
}
