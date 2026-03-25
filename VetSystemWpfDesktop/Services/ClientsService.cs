using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VetSystemModels.Dto.Client;

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
        public async Task<ClientDto?> GetClientAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ClientDto?>($"Clients/{id}");
        }
    }
}
