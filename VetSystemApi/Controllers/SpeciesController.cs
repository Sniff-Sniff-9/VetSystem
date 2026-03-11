using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemModels.Entities;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly DictionaryEntityService<Species> _speciesService;
        public SpeciesController(DictionaryEntityService<Species> speciesService)
        {
            _speciesService = speciesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecies()
        {
            var species = await _speciesService.GetAllAsync();
            if (species == null)
            {
                return NotFound();
            }
            return Ok(species);
        }

    }
}
