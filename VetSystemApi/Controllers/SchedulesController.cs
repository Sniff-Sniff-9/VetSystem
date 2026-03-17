using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.Schedule;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ISchedulesService _schedulesService;

        public SchedulesController(ISchedulesService schedulesService)
        {
            _schedulesService = schedulesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedulesAsync()
        {
            var schedules = await _schedulesService.GetSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleByIdAsync(int id)
        {
            var schedule = await _schedulesService.GetScheduleByIdAsync(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }

        [HttpGet("/api/Workdays/{id}/Schedules")]
        public async Task<IActionResult> GetSchedulesByWorkdayIdAsync(int id)
        {
            var schedule = await _schedulesService.GetSchedulesByWorkdayIdAsync(id);
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync([FromBody] CreateUpdateScheduleDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var schedule = await _schedulesService.CreateScheduleAsync(scheduleDto);

            return Ok(schedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheduleAsync(int id, [FromBody] CreateUpdateScheduleDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var schedule = await _schedulesService.UpdateScheduleAsync(id, scheduleDto);

            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleAsync(int id)
        {
            await _schedulesService.DeleteScheduleAsync(id);
            return NoContent();
        }
    }
}

