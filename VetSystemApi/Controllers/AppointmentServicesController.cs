using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.AppointmentService;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentServicesController : ControllerBase
    {
        private readonly IAppointmentServicesService _appointmentServicesService;

        public AppointmentServicesController(IServicesService servicesService, IAppointmentServicesService appointmentServicesService)
        {
            _appointmentServicesService = appointmentServicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentServicesAsync()
        {
            var appointmentServices = await _appointmentServicesService.GetAppointmentServicesAsync();
            return Ok(appointmentServices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentServiceByIdAsync(int id)
        {
            var appointmentService = await _appointmentServicesService.GetAppointmentServiceByIdAsync(id);
            if (appointmentService == null)
            {
                return NotFound();
            }
            return Ok(appointmentService);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointmentServiceAsync([FromBody] CreateAppointmentServiceDto appointmentServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var appointmentService = await _appointmentServicesService.CreateAppointmentServiceAsync(appointmentServiceDto);
            return Ok(appointmentService);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentServiceAsync(int id)
        {
            await _appointmentServicesService.DeleteAppointmentServiceAsync(id);
            return NoContent();
        }
    }
}
