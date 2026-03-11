using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemModels.Entities;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly DictionaryEntityService<Gender> _gendersService;
        public GendersController(DictionaryEntityService<Gender> gendersService)
        {
            _gendersService = gendersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenders()
        {
            var genders = await _gendersService.GetAllAsync();
            if (genders == null)
            {
                return NotFound();
            }
            return Ok(genders);
        }
    }
}
