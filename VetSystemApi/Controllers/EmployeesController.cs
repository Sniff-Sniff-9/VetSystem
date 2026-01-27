using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto;

namespace VetSystemApi.Controllers
{

    //Add "IsDeleted" field for clients and employees

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeesService.GetEmployeesAsync();
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpGet("EmployeeId/{id}")]
        public async Task<IActionResult> GetEmployeeByEmployeeId(int id)
        {
            var employee = await _employeesService.GetEmployeeByEmployeeIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet("UserId/{id}")]
        public async Task<IActionResult> GetEmployeeByUserId(int id)
        {
            var employee = await _employeesService.GetEmployeeByUserIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] EmployeeDto EmployeeDto)
        {
            var employee = await _employeesService.CreateEmployeeAsync(EmployeeDto);
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {

            var employee = await _employeesService.UpdateEmployeeAsync(id, updateEmployeeDto);
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            return Ok(employee);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _employeesService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
