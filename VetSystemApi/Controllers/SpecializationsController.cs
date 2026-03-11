using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemModels.Entities;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly DictionaryEntityService<Specialization> _specializationsService;
        public SpecializationsController(DictionaryEntityService<Specialization> specializationsService)
        {
            _specializationsService = specializationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializations()
        {
            var specializations = await _specializationsService.GetAllAsync();
            if (specializations == null)
            {
                return NotFound();
            }
            return Ok(specializations);
        }

    }
}
