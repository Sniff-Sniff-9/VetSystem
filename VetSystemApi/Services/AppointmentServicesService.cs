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
            var appointmentServices = await _context.AppointmentServices.Include(es => es.Service).ToListAsync();
            return appointmentServices.Select(es => ToAppointmentServiceDto(es)).ToList();
        }

        public async Task<List<AppointmentServiceDto>> GetServicesByAppointmentIdAsync(int id)
        {
            var services = await _context.AppointmentServices.Include(es => es.Service).Where(es => es.AppointmentId == id).ToListAsync();
            return services.Select(s => ToAppointmentServiceDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByServiceIdAsync(int id)
        {
            var appointments = await _context.AppointmentServices.Where(es => es.ServiceId == id).Select(es => es.Appointment).ToListAsync();
            return appointments.Select(e => ToAppointmentDto(e)).ToList();
        }

        public async Task<AppointmentServiceDto?> GetAppointmentServiceByIdAsync(int id)
        {
            var appointmentService = await _context.AppointmentServices.Include(es => es.Service).FirstOrDefaultAsync(es => es.AppointmentServiceId == id);
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
                PriceAtMoment = service.Price,
                IsMain = AppointmentServiceDto.IsMain
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

        private AppointmentDto ToAppointmentDto(Appointment appointment)
        {
            return new AppointmentDto()
            {
                AppointmentId = appointment.AppointmentId,
                ServiceId = appointment.AppointmentServices?.FirstOrDefault(s => s.IsMain)?.ServiceId ?? 0,
                ServiceName = appointment.AppointmentServices?.FirstOrDefault(s => s.IsMain)?.Service?.ServiceName ?? "undefined",
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                PetId = appointment.PetId,
                PetName = appointment.Pet?.Name ?? "undefiend",
                ClientId = appointment.Pet?.ClientId ?? 0,
                ClientName = appointment.Pet?.Client != null
                ? $"{appointment.Pet.Client.LastName} {appointment.Pet.Client.FirstName} {appointment.Pet.Client.MiddleName}" : "undefined",
                EmployeeId = appointment.EmployeeId,
                EmployeeName = appointment.Employee != null
                ? $"{appointment.Employee.LastName} {appointment.Employee.FirstName} " +
                $"{appointment.Employee.MiddleName}" : "undefined",
                TotalPriceAtMoment = appointment.TotalPriceAtMoment,
                AppointmentStatusId = appointment.AppointmentStatusId,
                AppointmentStatusName = appointment.AppointmentStatus?.AppointmentStatusName ?? "undefined",
                AppointmentDate = appointment.AppointmentDate
            };
        }

        private AppointmentServiceDto ToAppointmentServiceDto(AppointmentService appointmentService)
        {
            return new AppointmentServiceDto()
            {
                AppointmentServiceId = appointmentService.AppointmentServiceId,
                AppointmentId = appointmentService.AppointmentId,
                ServiceId = appointmentService.ServiceId,
                PriceAtMoment = appointmentService.PriceAtMoment,
                ServiceName = appointmentService.Service?.ServiceName ?? "undefined",
                IsMain = appointmentService.IsMain
            };
        }
    }
}

