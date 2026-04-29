using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;

namespace VetSystemWpfDesktop.Services
{
    class ClientsService
    {
        private readonly HttpClient _httpClient;

        public ClientsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClientDto>?> GetClientsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ClientDto>>("Clients");
        }

        public async Task<List<PetDto>?> GetPetsByClientIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<List<PetDto>>($"Clients/{id}/Pets");
        }

        public async Task<ClientDto?> GetClientAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ClientDto?>($"Clients/{id}");
        }

        public async Task<ClientDto?> UpdateClientAsync(UpdateClientDto clientDto, int id)
        {
            var client = await _httpClient.GetFromJsonAsync<ClientDto?>($"Clients/{id}") ?? new();
            var response = await _httpClient.PutAsJsonAsync($"Clients/{client.ClientId}", clientDto);

            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {error}");
            }

            return await response.Content.ReadFromJsonAsync<ClientDto>() ?? new();
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientDto client)
        {
            var response = await _httpClient.PostAsJsonAsync("Clients", client);

            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API error: {error}");
            }

            return await response.Content.ReadFromJsonAsync<ClientDto>() ?? new();
        }

        public async Task DeleteClientAsync(int id)
        {
            await _httpClient.DeleteAsync($"Clients/{id}");
        }
    }
}
