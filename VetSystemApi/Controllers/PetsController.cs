using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto.Pet;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {

        private readonly IPetsService _petsService;

        public PetsController(IPetsService petsService)
        {
            _petsService = petsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPets()
        {
            var pets = await _petsService.GetPetsAsync();
            if (pets == null)
            {
                return NotFound();
            }
            return Ok(pets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetById(int id)
        {
            var pet = await _petsService.GetPetByIdAsync(id);

            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }

        [HttpGet("Client/{clientId}")]
        public async Task<IActionResult> GetPetsByClientId(int clientId)
        {
            var pet = await _petsService.GetPetsByClientIdAsync(clientId);

            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }

        [HttpPost("Client/{id}")]
        public async Task<IActionResult> CreatePetAsync([FromBody] PetDto petDto, int clientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var pet = await _petsService.CreatePetAsync(petDto, clientId);
            
            return Ok(pet);
        }

        [HttpPut("{id}/Client/{clientId}")]
        public async Task<IActionResult> UpdatePetAsync(int id, [FromBody] PetDto updatePetDto, int clientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var pet = await _petsService.UpdatePetAsync(id, updatePetDto, clientId);
            
            return Ok(pet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetAsync(int id)
        {
            await _petsService.DeletePetAsync(id);
            return NoContent();
        }
    }
}
