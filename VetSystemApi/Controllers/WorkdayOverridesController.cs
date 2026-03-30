using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.WorkdayOverride;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkdayOverridesController : ControllerBase
    {
        private readonly IWorkdayOverridesService _workdayOverridesService;

        public WorkdayOverridesController(IWorkdayOverridesService workdayOverridesService)
        {
            _workdayOverridesService = workdayOverridesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkdayOverrideAsync([FromBody] CreateUpdateWorkdayOverrideDto wprkdayOverrideDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var workdayOverride = await _workdayOverridesService.CreateWorkdayOverridesAsync(wprkdayOverrideDto);

            return Ok(workdayOverride);
        }
    }
}
