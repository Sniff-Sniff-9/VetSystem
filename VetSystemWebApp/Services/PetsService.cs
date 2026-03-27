using VetSystemModels.Dto.Pet;

namespace VetSystemWebApp.Services
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
            return await _httpClient.GetFromJsonAsync<List<PetDto>>("Pets") ?? new();
        }

        //public async Task<List<Species>?> GetSpeciesAsync()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<Species>>("Species");
        //}

        //public async Task<List<Gender>?> GetGendersAsync()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<Gender>>("Genders");
        //}


    }
}
