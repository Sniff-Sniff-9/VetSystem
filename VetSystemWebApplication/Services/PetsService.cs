using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<List<Species>> GetSpeciesAsync()
        {     
            return await _httpClient.GetFromJsonAsync<List<Species>>("Species") ?? new();
        }

        public async Task<List<Gender>> GetGendersAsync()
        {     
            return await _httpClient.GetFromJsonAsync<List<Gender>>("Genders") ?? new();
        }

        public async Task<PetDto> GetPetAsync(int id)
        {
            var client = await _clientService.GetClientByUserIdAsync() ?? new();      
            return await _httpClient.GetFromJsonAsync<PetDto>($"Pets/{id}") ?? new();
        }
        public async Task DeletePetAsync(int id)
        {
            await _httpClient.DeleteAsync($"Pets/{id}");
        }


        public async Task<PetDto> CreatePetAsync(CreateUpdatePetDto pet)
        {
            var client = await _clientService.GetClientByUserIdAsync() ?? new();
            pet.ClientId = client.ClientId;
            var response = await _httpClient.PostAsJsonAsync("Pets", pet);

            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {error}");
            }

            return await response.Content.ReadFromJsonAsync<PetDto>() ?? new();
        }

        public async Task<PetDto> UpdatePetAsync(int id, CreateUpdatePetDto pet)
        {
            var client = await _clientService.GetClientByUserIdAsync() ?? new();
            pet.ClientId = client.ClientId;
            var response = await _httpClient.PutAsJsonAsync($"Pets/{id}", pet);

            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {error}");
            }

            return await response.Content.ReadFromJsonAsync<PetDto>() ?? new();
        }

    }
}
