using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Service;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class ServicesService: IServicesService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ServicesService> _logger;

        public ServicesService(AppDbContext context, ILogger<ServicesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ServiceDto>> GetServicesAsync()
        {
            var services = await _context.Services.ToListAsync();
            return services.Select(s => ToServiceDto(s)).ToList();
        }

        public async Task<ServiceDto?> GetServiceByIdAsync(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
            if (service == null)
            {
                return null;
            }
            return ToServiceDto(service);
        }

        public async Task<ServiceDto> CreateServiceAsync(ServiceDto serviceDto)
        {
            var service = new Service()
            {
                ServiceName = serviceDto.ServiceName,
                Price = serviceDto.Price
            };
            try
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return ToServiceDto(service);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Service can't be created.");
                throw;
            }
        }

        public async Task<ServiceDto> UpdateServiceAsync(int id, ServiceDto serviceDto)
        {
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null)
            {
                throw new ArgumentNullException("Service not found.");
            }
            service.ServiceName = serviceDto.ServiceName ?? "undefiend";
            service.Price = serviceDto.Price;
            try
            {
                await _context.SaveChangesAsync();
                return ToServiceDto(service);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Service can't be updated.");
                throw;
            }
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = _context.Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null)
            {
                throw new ArgumentNullException("Service not found.");
            }
            service.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service can't be deleted.");
                throw;
            }
        }
        private ServiceDto ToServiceDto(Service service)
        {
            return new ServiceDto()
            {
                ServiceName = service.ServiceName,
                Price = service.Price
            };
        }
    }
}
