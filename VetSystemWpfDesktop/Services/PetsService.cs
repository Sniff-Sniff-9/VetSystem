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


    }
}
