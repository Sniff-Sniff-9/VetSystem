using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.AppointmentService;
using VetSystemModels.Dto.Service;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class AppointmentServicesService: IAppointmentServicesService
    {
        private readonly AppDbContext _context;
        private ILogger<AppointmentServicesService> _logger;

        public AppointmentServicesService(AppDbContext context, ILogger<AppointmentServicesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AppointmentServiceDto>> GetAppointmentServicesAsync()
        {
            var appointmentServices = await _context.AppointmentServices.ToListAsync();
            return appointmentServices.Select(es => ToAppointmentServiceDto(es)).ToList();
        }

        public async Task<List<ServiceDto>> GetServicesByAppointmentIdAsync(int id)
        {
            var services = await _context.AppointmentServices.Where(es => es.AppointmentId == id).Select(es => es.Service).ToListAsync();
            return services.Select(s => ToServiceDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByServiceIdAsync(int id)
        {
            var фppointments = await _context.AppointmentServices.Where(es => es.ServiceId == id).Select(es => es.Appointment).ToListAsync();
            return фppointments.Select(e => ToAppointmentDto(e)).ToList();
        }

        public async Task<AppointmentServiceDto?> GetAppointmentServiceByIdAsync(int id)
        {
            var appointmentService = await _context.AppointmentServices.FirstOrDefaultAsync(es => es.AppointmentServiceId == id);
            if (appointmentService == null)
            {
                return null;
            }
            return ToAppointmentServiceDto(appointmentService);
        }

        public async Task<AppointmentServiceDto> CreateAppointmentServiceAsync(CreateAppointmentServiceDto AppointmentServiceDto)
        {
            var appointmentExists = await _context.Appointments.AnyAsync(e => e.AppointmentId == AppointmentServiceDto.AppointmentId);
            if (!appointmentExists)
            {
                throw new ArgumentException("Appointment doesn't exist.");
            }

            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == AppointmentServiceDto.ServiceId);
            if (service == null)
            {
                throw new ArgumentException("Service doesn't exist.");
            }

            var relationExists = await _context.AppointmentServices.AnyAsync(es => es.AppointmentId == AppointmentServiceDto.AppointmentId
                 && es.ServiceId == AppointmentServiceDto.ServiceId
                 && !es.IsDeleted);

            if (relationExists)
            {
                throw new ArgumentException("Appointment already has this service.");
            }

            var AppointmentService = new AppointmentService
            {
                AppointmentId = AppointmentServiceDto.AppointmentId,
                ServiceId = service.ServiceId,
                PriceAtMoment = service.Price
            };
            try
            {
                _context.Add(AppointmentService);
                await _context.SaveChangesAsync();
                return ToAppointmentServiceDto(AppointmentService);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Appointment's service can't be created.");
                throw;
            }
        }

        public async Task DeleteAppointmentServiceAsync(int id)
        {
            var AppointmentService = await _context.AppointmentServices.FirstOrDefaultAsync(es => es.AppointmentServiceId == id);
            if (AppointmentService == null)
            {
                throw new ArgumentNullException("Appointment's service not found.");
            }
            AppointmentService.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Appointment's service can't be deleted.");
                throw;
            }
        }

        private ServiceDto ToServiceDto(Service service)
        {
            return new ServiceDto()
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                Price = service.Price,
                DurationMinutes = service.DurationMinutes
            };
        }

        private AppointmentDto ToAppointmentDto(Appointment appointment)
        {
            return new AppointmentDto()
            {
                AppointmentId = appointment.AppointmentId,
                ServiceId = appointment.ServiceId,
                ServiceName = appointment.Service?.ServiceName ?? "undefined",
                ScheduleId = appointment.ScheduleId,
                SсheduleTimeStart = appointment.Schedule?.StartTime ?? TimeOnly.MinValue,
                SсheduleTimeEnd = appointment.Schedule?.EndTime ?? TimeOnly.MinValue,
                PetId = appointment.PetId,
                PetName = appointment.Pet?.Name ?? "undefiend",
                TotalPriceAtMoment = appointment.TotalPriceAtMoment,
                AppointmentStatusId = appointment.AppointmentStatusId,
                AppointmentStatusName = appointment.AppointmentStatus?.AppointmentStatusName ?? "undefined"
            };
        }

        private AppointmentServiceDto ToAppointmentServiceDto(AppointmentService AppointmentService)
        {
            return new AppointmentServiceDto()
            {
                AppointmentServiceId = AppointmentService.AppointmentServiceId,
                AppointmentId = AppointmentService.AppointmentId,
                ServiceId = AppointmentService.ServiceId,
                PriceAtMoment = AppointmentService.PriceAtMoment        
            };
        }
    }
}

