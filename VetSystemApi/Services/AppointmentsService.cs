using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.AppointmentService;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Dto.ScheduleAvailability;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class AppointmentsService: IAppointmentsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentsService> _logger;
        private readonly IScheduleAvailabilityService _scheduleAvailabilityService;
        private readonly IAppointmentServicesService _appointmentServicesService;

        public AppointmentsService(AppDbContext context, ILogger<AppointmentsService> logger, 
            IScheduleAvailabilityService scheduleAvailabilityService, IAppointmentServicesService appointmentServicesService)
        {
            _context = context;
            _logger = logger;
            _scheduleAvailabilityService = scheduleAvailabilityService;
            _appointmentServicesService = appointmentServicesService;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsAsync()
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Employee)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).ThenInclude(aps => aps.Service).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPetIdAsync(int id)
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Employee)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).ThenInclude(aps => aps.Service).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByClientIdAsync(int id)
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Employee)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).ThenInclude(aps => aps.Service)
                .Where(a => a.Pet.ClientId == id).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByEmployeeIdAsync(int id)
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Employee)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).ThenInclude(aps => aps.Service)
                 .Where(a => a.EmployeeId == id).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }


        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Employee)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).ThenInclude(aps => aps.Service)
                .FirstOrDefaultAsync(s => s.AppointmentId == id);
            if (appointment == null)
            {
                return null;
            }
            return ToAppointmentDto(appointment);
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(CreateUpdateAppointmentDto appointmentDto)
        {
            var petExists = await _context.Pets.AnyAsync(p => p.PetId == appointmentDto.PetId);
            if (!petExists)
                throw new ArgumentException("Pet doesn't exist.");

            var employeeService = await _context.EmployeeServices
                .Include(es => es.Service)
                .FirstOrDefaultAsync(es => es.EmployeeServiceId == appointmentDto.EmployeeServiceId);

            if (employeeService == null)
                throw new ArgumentException("Invalid EmployeeService.");

            var scheduleAvailabilityDto = new ScheduleAvailabilityDto
            {
                EmployeeId = employeeService.EmployeeId,
                ScheduleAvailabilityDate = appointmentDto.AppointmentDate
            };

            var availableSlots = await _scheduleAvailabilityService.GetAvailableSlotsAsync(scheduleAvailabilityDto);

            if (!availableSlots.Contains(appointmentDto.StartTime))
                throw new ArgumentException("Selected time is not available.");

            var appointment = new Appointment
            {
                EmployeeId = employeeService.EmployeeId,
                PetId = appointmentDto.PetId,
                AppointmentDate = appointmentDto.AppointmentDate,
                StartTime = appointmentDto.StartTime,
                EndTime = appointmentDto.StartTime.Add(
                    TimeSpan.FromMinutes(employeeService.Service.DurationMinutes)),
                AppointmentStatusId = appointmentDto.AppointmentStatusId
            };

            try
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                var appointmentServiceDto = new CreateAppointmentServiceDto
                {
                    AppointmentId = appointment.AppointmentId,
                    ServiceId = employeeService.ServiceId,
                    IsMain = true
                };

                var appointmentService = await _appointmentServicesService.CreateAppointmentServiceAsync(appointmentServiceDto);
                appointment.TotalPriceAtMoment = appointmentService.PriceAtMoment;

                var result = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).Include(a => a.Employee)
                    .FirstAsync(a => a.AppointmentId == appointment.AppointmentId);
                return ToAppointmentDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Appointment can't be created.");
                throw;
            }
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(int id, CreateUpdateAppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments.Include(a => a.AppointmentServices)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                throw new ArgumentException("Appointment not found.");

            var employeeService = await _context.EmployeeServices
                .Include(es => es.Service)
                .FirstOrDefaultAsync(es => es.EmployeeServiceId == appointmentDto.EmployeeServiceId);

            if (employeeService == null)
                throw new ArgumentException("Invalid EmployeeService.");

            var scheduleAvailabilityDto = new ScheduleAvailabilityDto
            {
                EmployeeId = employeeService.EmployeeId,
                ScheduleAvailabilityDate = appointmentDto.AppointmentDate
            };

            var availableSlots = await _scheduleAvailabilityService.GetAvailableSlotsAsync(scheduleAvailabilityDto);

            if (!availableSlots.Contains(appointmentDto.StartTime))
                throw new ArgumentException("Selected time is not available.");

            appointment.PetId = appointmentDto.PetId;
            appointment.EmployeeId = employeeService.EmployeeId;
            appointment.AppointmentDate = appointmentDto.AppointmentDate;
            appointment.StartTime = appointmentDto.StartTime;
            appointment.EndTime = appointmentDto.StartTime.Add(
                TimeSpan.FromMinutes(employeeService.Service.DurationMinutes));
            appointment.AppointmentStatusId = appointmentDto.AppointmentStatusId;

            appointment.TotalPriceAtMoment = appointment.AppointmentServices
                .Sum(s => s.PriceAtMoment);

            try
            {
                await _context.SaveChangesAsync();
                var result = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client)
                .Include(a => a.AppointmentStatus).Include(a => a.AppointmentServices).Include(a => a.Employee)
                    .FirstAsync(a => a.AppointmentId == appointment.AppointmentId);
                return ToAppointmentDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Appointment can't be updated.");
                throw;
            }
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(s => s.AppointmentId == id);
            if (appointment == null)
            {
                throw new ArgumentNullException("Appointment not found.");
            }
            appointment.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Appointment can't be deleted.");
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
    }
}
