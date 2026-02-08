using VetSystemModels.Dto.Client;

namespace VetSystemApi.Services.Interfaces
{
    public interface IEntityService<TEntityDto>
    {
        public Task<List<TEntityDto>> GetAllAsync();
        public Task<TEntityDto> GetByIdAsync(int id);
        public Task<TEntityDto> CreateAsync(TEntityDto entityDto);
        public Task<TEntityDto> UpdateAsync(int id, TEntityDto entityDto);
        public Task DeleteAsync(int id);
    }
}
