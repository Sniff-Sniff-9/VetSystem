using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        private readonly IEmployeeServicesService _employeeServicesService;
        private readonly IAppointmentServicesService _appointmentServicesService;

        public ServicesController(IServicesService servicesService, IEmployeeServicesService employeeServicesService, IAppointmentServicesService appointmentServicesService)
        {
            _servicesService = servicesService;
            _employeeServicesService = employeeServicesService;
            _appointmentServicesService = appointmentServicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServicesAsync()
        {
            var services = await _servicesService.GetServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}/Appointments")]
        public async Task<IActionResult> GetAppointmentsByServiceIdAsync(int id)
        {
            var appointments = await _appointmentServicesService.GetAppointmentsByServiceIdAsync(id);
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceByIdAsync(int id)
        {
            var service = await _servicesService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }

        [HttpGet("{id}/Employees")]
        public async Task<IActionResult> GetEmployeesByServiceIdAsync(int id)
        {
            var employees = await _employeeServicesService.GetEmployeesByServiceIdAsync(id);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceAsync([FromBody] CreateUpdateServiceDto serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var service = await _servicesService.CreateServiceAsync(serviceDto);
            return Ok(service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceAsync(int id, [FromBody] CreateUpdateServiceDto serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var service = await _servicesService.UpdateServiceAsync(id, serviceDto);
            return Ok(service);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceAsync(int id)
        {
            await _servicesService.DeleteServiceAsync(id);
            return NoContent();
        }
    }
}
