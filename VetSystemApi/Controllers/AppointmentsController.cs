using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.Appointment;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentsService;

        public AppointmentsController(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsAsync()
        {
            var appointments = await _appointmentsService.GetAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _appointmentsService.GetAppointmentByIdAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateUpdateAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var appointment = await _appointmentsService.CreateAppointmentAsync(appointmentDto);

            return Ok(appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointmentAsync(int id, [FromBody] CreateUpdateAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var appointment = await _appointmentsService.UpdateAppointmentAsync(id, appointmentDto);

            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentAsync(int id)
        {
            await _appointmentsService.DeleteAppointmentAsync(id);
            return NoContent();
        }
    }
}
