using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.Employee;

namespace VetSystemApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        private readonly IEmployeeServicesService _employeeServicesService;
        private readonly IAppointmentsService _appointmentsService;

        public EmployeesController(IEmployeesService employeesService, IEmployeeServicesService employeeServicesService, 
            IAppointmentsService appointmentsService)
        {
            _employeesService = employeesService;
            _employeeServicesService = employeeServicesService;
            _appointmentsService = appointmentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            var employees = await _employeesService.GetEmployeesAsync();
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByEmployeeIdAsync(int id)
        {
            var employee = await _employeesService.GetEmployeeByEmployeeIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet("/api/Users/{id}/Employee")]
        public async Task<IActionResult> GetEmployeeByUserIdAsync(int id)
        {
            var employee = await _employeesService.GetEmployeeByUserIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet("{id}/Services")]
        public async Task<IActionResult> GetServicesByEmployeeIdAsync(int id)
        {
            var services = await _employeeServicesService.GetServicesByEmployeeIdAsync(id);
            return Ok(services);
        }

        [HttpGet("{id}/Appointments")]
        public async Task<IActionResult> GetAppointmentsByEmployeeIdAsync(int id)
        {
            var appointments = await _appointmentsService.GetAppointmentsByEmployeeIdAsync(id);
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeDto createEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var employee = await _employeesService.CreateEmployeeAsync(createEmployeeDto);
            
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var employee = await _employeesService.UpdateEmployeeAsync(id, updateEmployeeDto);

            return Ok(employee);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            await _employeesService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
