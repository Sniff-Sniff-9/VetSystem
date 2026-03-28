using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemApi.Services.Interfaces;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleGenerationController : ControllerBase
    {
        private readonly IScheduleGenerationService _scheduleGenerationService;

        public ScheduleGenerationController(IScheduleGenerationService scheduleGenerationService)
        {
            _scheduleGenerationService = scheduleGenerationService;
        }

    }
}
