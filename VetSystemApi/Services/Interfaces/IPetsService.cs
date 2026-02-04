using VetSystemModels.Dto;

namespace VetSystemApi.Services.Interfaces
{
    public interface IPetsService
    {
        public Task<List<PetDto>> GetPetsAsync();
        public Task<PetDto?> GetPetByIdIdAsync(int id);
        public Task<List<PetDto>> GetPetsByClientIdAsync(int id);
        public Task<PetDto> CreatePetAsync(PetDto petDto);
        public Task<PetDto> UpdatePetAsync(int id, PetDto petDto);
        public Task DeletePetAsync(int id);
    }
}
