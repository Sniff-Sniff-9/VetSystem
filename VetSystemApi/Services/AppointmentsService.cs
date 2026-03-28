using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class AppointmentsService: IAppointmentsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentsService> _logger;

        public AppointmentsService(AppDbContext context, ILogger<AppointmentsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsAsync()
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Schedule.Workday)
                .Include(a => a.AppointmentStatus).Include(a => a.Schedule).Include(a => a.Schedule.Workday.Employee)
                .Include(a => a.Service).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPetIdAsync(int id)
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Schedule.Workday)
                .Include(a => a.Pet.Client).Include(a => a.AppointmentStatus).Include(a => a.Schedule)
                .Include(a => a.Service).Include(a => a.Schedule.Workday.Employee)
                .Where(s => s.PetId == id).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByClientIdAsync(int id)
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Schedule.Workday)
                .Include(a => a.Pet.Client).Include(a => a.AppointmentStatus).Include(a => a.Schedule)
                .Include(a => a.Service).Include(a => a.Schedule.Workday.Employee)
                .Where(s => s.Pet.Client.UserId == id).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByEmployeeIdAsync(int id)
        {
            var appointments = await _context.Appointments.Include(a => a.Pet).Include(a => a.Schedule.Workday)
                .Include(a => a.Pet.Client).Include(a => a.AppointmentStatus).Include(a => a.Schedule)
                .Include(a => a.Service).Include(a => a.Schedule.Workday.Employee)
                .Where(s => s.Schedule.Workday.EmployeeId == id).ToListAsync();
            return appointments.Select(s => ToAppointmentDto(s)).ToList();
        }


        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Schedule.Workday)
                .Include(a => a.AppointmentStatus).Include(a => a.Schedule).Include(a => a.Schedule.Workday.Employee)
                .Include(a => a.Service).FirstOrDefaultAsync(s => s.AppointmentId == id);
            if (appointment == null)
            {
                return null;
            }
            return ToAppointmentDto(appointment);
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(CreateUpdateAppointmentDto appointmentDto)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == appointmentDto.ServiceId);
            var schedule = await _context.Schedules.Include(sch => sch.Workday).FirstOrDefaultAsync(sch => sch.ScheduleId == appointmentDto.ScheduleId);
            var petExists = await _context.Pets.AnyAsync(p => p.PetId == appointmentDto.PetId);
            var appointmentStatusExists = await _context.AppointmentStatuses.AnyAsync(aps => aps.AppointmentStatusId == appointmentDto.AppointmentStatusId);

            if (service == null)
            {
                throw new ArgumentException("Service doesn't exist.");
            }

            if (schedule == null)
            {
                throw new ArgumentException("Schedule doesn't exist.");
            }

            var employeeCanDoService = await _context.EmployeeServices.AnyAsync(es => es.EmployeeId == schedule.Workday.EmployeeId &&
                                       es.ServiceId == appointmentDto.ServiceId);

            if (!employeeCanDoService)
            {
                throw new ArgumentException("Selected employee doesn't provide this service.");
            }

            var availabilitySchedule = await _context.Appointments.AnyAsync(a => a.ScheduleId == appointmentDto.ScheduleId);
            if (availabilitySchedule)
            {
                throw new ArgumentException("Schedule doesn't available.");
            }

            if (!petExists)
            {
                throw new ArgumentException("Pet doesn't exist.");
            }
            if (!appointmentStatusExists)
            {
                throw new ArgumentException("Appointment's status doesn't exist.");
            }

            var appointment = new Appointment()
            {
                ServiceId = appointmentDto.ServiceId,
                ScheduleId = appointmentDto.ScheduleId,   
                PetId = appointmentDto.PetId,
                TotalPriceAtMoment = service.Price,
                AppointmentStatusId = appointmentDto.AppointmentStatusId,
            };
            try
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                var result = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Schedule.Workday)
                    .Include(a => a.AppointmentStatus).Include(a => a.Schedule).Include(a => a.Schedule.Workday.Employee)
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
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == appointmentDto.ServiceId);
            var scheduleExists = await _context.Schedules.AnyAsync(sch => sch.ScheduleId == appointmentDto.ScheduleId);
            var petExists = await _context.Pets.AnyAsync(p => p.PetId == appointmentDto.PetId);
            var appointmentStatusExists = await _context.AppointmentStatuses.AnyAsync(aps => aps.AppointmentStatusId == appointmentDto.AppointmentStatusId);

            if (service == null)
            {
                throw new ArgumentException("Service doesn't exist.");
            }
            if (!scheduleExists)
            {
                throw new ArgumentException("Schedule doesn't exist.");
            }
            if (!petExists)
            {
                throw new ArgumentException("Pet doesn't exist.");
            }
            if (!appointmentStatusExists)
            {
                throw new ArgumentException("Appointment's status doesn't exist.");
            }
            var appointment = _context.Appointments.FirstOrDefault(s => s.AppointmentId == id);
            if (appointment == null)
            {
                throw new ArgumentNullException("Appointment not found.");
            }
            appointment.ServiceId = appointmentDto.ServiceId;
            appointment.ScheduleId = appointmentDto.ScheduleId;
            appointment.PetId = appointmentDto.PetId;
            appointment.AppointmentStatusId = appointmentDto.AppointmentStatusId;
            try
            {
                await _context.SaveChangesAsync();
                var result = await _context.Appointments.Include(a => a.Pet).Include(a => a.Pet.Client).Include(a => a.Schedule.Workday)
                    .Include(a => a.AppointmentStatus).Include(a => a.Schedule).Include(a => a.Schedule.Workday.Employee)
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
                ServiceId = appointment.ServiceId,
                ServiceName = appointment.Service?.ServiceName ?? "undefined",
                ScheduleId = appointment.ScheduleId,
                SсheduleTimeStart = appointment.Schedule?.StartTime ?? TimeOnly.MinValue,
                SсheduleTimeEnd = appointment.Schedule?.EndTime ?? TimeOnly.MinValue,
                PetId = appointment.PetId,
                PetName = appointment.Pet?.Name ?? "undefiend",
                ClientId = appointment.Pet?.ClientId ?? 0,
                ClientName = appointment.Pet?.Client != null
                ? $"{appointment.Pet.Client.LastName} {appointment.Pet.Client.FirstName} {appointment.Pet.Client.MiddleName}" : "undefined",
                EmployeeId = appointment.Schedule?.Workday?.EmployeeId ?? 0,
                EmployeeName = appointment.Schedule?.Workday?.Employee != null
                ? $"{appointment.Schedule.Workday.Employee.LastName} {appointment.Schedule.Workday.Employee.FirstName} " +
                $"{appointment.Schedule.Workday.Employee.MiddleName}" : "undefined",
                TotalPriceAtMoment = appointment.TotalPriceAtMoment,
                AppointmentStatusId = appointment.AppointmentStatusId,
                AppointmentStatusName = appointment.AppointmentStatus?.AppointmentStatusName ?? "undefined",
                AppointmentDate = appointment.Schedule?.Workday != null ? appointment.Schedule.Workday.WorkDate : DateOnly.MinValue
            };
        }
    }
}
