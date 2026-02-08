using VetSystemModels.Dto.Client;

namespace VetSystemApi.Services.Interfaces
{
    public interface IClientsService
    {
        public Task<List<ClientDto>> GetClientsAsync();
        public Task<ClientDto?> GetClientByClientIdAsync(int id);
        public Task<ClientDto?> GetClientByUserIdAsync(int id);
        public Task<ClientDto> CreateClientAsync(CreateClientDto createClientDto);
        public Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto updateClientDto);
        public Task DeleteClientAsync(int id);
    }
}
