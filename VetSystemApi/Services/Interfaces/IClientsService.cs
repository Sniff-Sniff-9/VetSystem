using VetSystemModels.Dto;

namespace VetSystemApi.Services.Interfaces
{
    public interface IClientsService
    {
        public Task<List<ClientDto>> GetClientsAsync();
        public Task<ClientDto?> GetClientByClientIdAsync(int id);
        public Task<ClientDto?> GetClientByUserIdAsync(int id);
        public Task<ClientDto> CreateClientAsync(ClientDto ClientDto);
        public Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto updateClientDto);
        public Task DeleteClientAsync(int id);
    }
}
