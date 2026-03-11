using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemModels.Entities;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentStatusesController : ControllerBase
    {
        private readonly DictionaryEntityService<AppointmentStatus> _appointmentStatusesService;
        public AppointmentStatusesController(DictionaryEntityService<AppointmentStatus> appointmentStatusesService)
        {
            _appointmentStatusesService = appointmentStatusesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentStatuses()
        {
            var appointmentStatuses = await _appointmentStatusesService.GetAllAsync();
            if (appointmentStatuses == null)
            {
                return NotFound();
            }
            return Ok(appointmentStatuses);
        }
    }
}
