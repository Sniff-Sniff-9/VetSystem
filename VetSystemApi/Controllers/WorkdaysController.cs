using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.Workday;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkdaysController : ControllerBase
    {
        private readonly IWorkdaysService _workdaysService;

        public WorkdaysController(IWorkdaysService workdaysService)
        {
            _workdaysService = workdaysService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkdaysAsync()
        {
            var workdays = await _workdaysService.GetWorkdaysAsync();
            return Ok(workdays);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkdayByIdAsync(int id)
        {
            var workday = await _workdaysService.GetWorkdayByIdAsync(id);

            if (workday == null)
            {
                return NotFound();
            }

            return Ok(workday);
        }

        [HttpGet("/api/Employees/{id}/Workdays")]
        public async Task<IActionResult> GetWorkdaysByEmployeeIdAsync(int id)
        {
            var workday = await _workdaysService.GetWorkdaysByEmployeeIdAsync(id);
            return Ok(workday);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkdayAsync([FromBody] CreateUpdateWorkdayDto workdayDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var workday = await _workdaysService.CreateWorkdayAsync(workdayDto);

            return Ok(workday);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkdayAsync(int id, [FromBody] CreateUpdateWorkdayDto workdayDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var workday = await _workdaysService.UpdateWorkdayAsync(id, workdayDto);

            return Ok(workday);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkdayAsync(int id)
        {
            await _workdaysService.DeleteWorkdayAsync(id);
            return NoContent();
        }
    }
}
