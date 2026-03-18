using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.EmployeeService;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeServicesController : ControllerBase
    {
        private readonly IEmployeeServicesService _employeeServicesService;

        public EmployeeServicesController(IServicesService servicesService, IEmployeeServicesService employeeServicesService)
        {
            _employeeServicesService = employeeServicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeServicesAsync()
        {
            var employeeServices = await _employeeServicesService.GetAllEmployeeServicesAsync();
            return Ok(employeeServices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeServiceByIdAsync(int id)
        {
            var employeeService = await _employeeServicesService.GetEmployeeServiceByIdAsync(id);
            if (employeeService == null)
            {
                return NotFound();
            }
            return Ok(employeeService);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeServiceAsync([FromBody] CreateUpdateEmployeeServiceDto employeeServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var employeeService = await _employeeServicesService.CreateEmployeeServiceAsync(employeeServiceDto);
            return Ok(employeeService);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeServiceAsync(int id, [FromBody] CreateUpdateEmployeeServiceDto employeeServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var employeeService = await _employeeServicesService.CreateEmployeeServiceAsync(employeeServiceDto);
            return Ok(employeeService);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeServiceAsync(int id)
        {
            await _employeeServicesService.DeleteEmployeeServiceAsync(id);
            return NoContent();
        }
    }
}
