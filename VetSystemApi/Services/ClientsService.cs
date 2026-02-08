using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Employee;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class ClientsService: IClientsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClientsService> _logger;

        public ClientsService(AppDbContext context, ILogger<ClientsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ClientDto>> GetClientsAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            return clients.Select(c => ToClientDto(c)).ToList();
        }

        public async Task<ClientDto?> GetClientByClientIdAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == id);
            if (client == null)
            {
                return null!;
            }
            return ToClientDto(client);
        }

        public async Task<ClientDto?> GetClientByUserIdAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == id);
            if (client == null)
            {
                return null!;
            }
            return ToClientDto(client);
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientDto createClientDto)
        {
            
            if (createClientDto.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException($"Birth date can't be larger than {DateOnly.FromDateTime(DateTime.UtcNow)}");
            }

            var client = new Client
            {
                LastName = createClientDto.LastName,
                FirstName = createClientDto.FirstName,
                MiddleName = createClientDto.MiddleName,
                BirthDate = createClientDto.BirthDate,
                Phone = createClientDto.Phone,
                UserId = createClientDto.UserId
            };
            try
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return ToClientDto(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Client can't be created.");
                throw;
            }
        }

        public async Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto updateClientDto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(u => u.ClientId == id);

            if (client == null)
            {
                throw new ArgumentNullException("Client not found.");
            }

            client.FirstName = updateClientDto.FirstName;
            client.LastName = updateClientDto.LastName;
            client.MiddleName = updateClientDto.MiddleName;
            client.BirthDate = updateClientDto.BirthDate;
            client.Phone = updateClientDto.Phone;   

            try
            {
                await _context.SaveChangesAsync();
                return ToClientDto(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Client can't be updated.");
                throw;
            }
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == id);
            if (client == null)
            {
                throw new ArgumentNullException("Client not found.");
            }
            try
            {
                client.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Client can't be deleted.");
                throw;
            }
        }

        private ClientDto ToClientDto(Client client)
        {
            return new ClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate,
                Phone = client.Phone
            };
        }
    }
}
