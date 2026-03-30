using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;

namespace VetSystemWebApplication.Services
{
    public class PetsService
    {
        private readonly HttpClient _httpClient;
        private readonly ClientsService _clientService;
        private readonly ILocalStorageService _localStorage;

        public PetsService(HttpClient httpClient, ILocalStorageService localStorage, ClientsService clientService)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _clientService = clientService;
        }

        public async Task<List<PetDto>> GetPetsAsync()
        {
            var client = await _clientService.GetClientByUserIdAsync() ?? new();      
            return await _httpClient.GetFromJsonAsync<List<PetDto>>($"Clients/{client.ClientId}/Pets") ?? new();
        }

        public async Task<PetDto> GetPetAsync(int id)
        {
            var client = await _clientService.GetClientByUserIdAsync() ?? new();      
            return await _httpClient.GetFromJsonAsync<PetDto>($"Pets/{id}") ?? new();
        }

    }
}
