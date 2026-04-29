using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Entities;

namespace VetSystemWpfDesktop.Services
{
    public class PetsService
    {
        private readonly HttpClient _httpClient;

        public PetsService(HttpClient httpClient)
        {
            _httpClient = httpClient;  
        }

        public async Task<List<PetDto>?> GetPetsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PetDto>>("Pets");
        }

        public async Task<List<Species>?> GetSpeciesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Species>>("Species");
        }

        public async Task<List<Gender>?> GetGendersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Gender>>("Genders");
        }

        public async Task<PetDto> CreatePetAsync(CreateUpdatePetDto pet)
        {
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
