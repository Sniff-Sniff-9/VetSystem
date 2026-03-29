using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.ScheduleAvailability;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleAvailabilityController : ControllerBase
    {
        private readonly IScheduleAvailabilityService _scheduleAvailabilityService;

        public ScheduleAvailabilityController(IScheduleAvailabilityService scheduleAvailabilityService)
        {
            _scheduleAvailabilityService = scheduleAvailabilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServicesAsync([FromQuery] ScheduleAvailabilityDto scheduleAvailabilityDto)
        {
            var slots = await _scheduleAvailabilityService.GetAvailableSlotsAsync(scheduleAvailabilityDto);
            return Ok(slots);
        }
    }
}
