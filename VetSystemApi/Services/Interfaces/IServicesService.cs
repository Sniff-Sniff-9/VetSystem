using VetSystemModels.Entities;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Services.Interfaces
{
    public interface IServicesService
    {
        public Task<List<ServiceDto>> GetServicesAsync();
        public Task<ServiceDto?> GetServiceByIdAsync(int id);
        public Task<ServiceDto> CreateServiceAsync(ServiceDto serviceDto);
        public Task<ServiceDto> UpdateServiceAsync(int id, ServiceDto serviceDto);
        public Task DeleteServiceAsync(int id);
    }
}
