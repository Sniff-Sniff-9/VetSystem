using VetSystemModels.Dto.Pet;

namespace VetSystemApi.Services.Interfaces
{
    public interface IPetsService
    {
        public Task<List<PetDto>> GetPetsAsync();
        public Task<PetDto?> GetPetByIdAsync(int id);
        public Task<List<PetDto>> GetPetsByClientIdAsync(int id);
        public Task<PetDto> CreatePetAsync(CreateUpdatePetDto petDto, int clientId);
        public Task<PetDto> UpdatePetAsync(int id, CreateUpdatePetDto petDto);
        public Task DeletePetAsync(int id);
    }
}
