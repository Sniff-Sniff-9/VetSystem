using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Entities;
using VetSystemModels.Dto;
using VetSystemInfrastructure.Configuration;

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
            return clients.Select(c => new ClientDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                MiddleName = c.MiddleName,
                BirthDate = c.BirthDate,
                Phone = c.Phone,
                UserId = c.UserId
            }).ToList();
        }

        public async Task<ClientDto?> GetClientByClientIdAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == id);
            if (client == null)
            {
                return null!;
            }
            return new ClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate,
                Phone = client.Phone,
                UserId = client.UserId
            };
        }

        public async Task<ClientDto?> GetClientByUserIdAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == id);
            if (client == null)
            {
                return null!;
            }
            return new ClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate,
                Phone = client.Phone,
                UserId = client.UserId
            };
        }

        public async Task<ClientDto> CreateClientAsync(ClientDto clientDto)
        {
            
            if (clientDto.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException($"Birth date can't be larger than {DateOnly.FromDateTime(DateTime.UtcNow)}");
            }

            var client = new Client
            {
                LastName = clientDto.LastName,
                FirstName = clientDto.FirstName,
                MiddleName = clientDto.MiddleName,
                BirthDate = clientDto.BirthDate,
                Phone = clientDto.Phone,
                UserId = clientDto.UserId
            };
            try
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return new ClientDto
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    MiddleName = client.MiddleName,
                    BirthDate = client.BirthDate,
                    Phone = client.Phone,
                    UserId = client.UserId
                };
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
                return new ClientDto
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    MiddleName = client.MiddleName,
                    BirthDate = client.BirthDate,
                    Phone = client.Phone,
                    UserId = client.UserId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User can't be updated.");
                throw;
            }
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(u => u.UserId == id);
            if (client == null)
            {
                throw new ArgumentNullException("User not found.");
            }
            try
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User can't be deleted.");
                throw;
            }

        }
    }
}
