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
        private readonly IAppointmentsService _appointmentsService;

        public PetsController(IPetsService petsService, IAppointmentsService appointmentsService)
        {
            _petsService = petsService;
            _appointmentsService = appointmentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPetsAsync()
        {
            var pets = await _petsService.GetPetsAsync();
            return Ok(pets);
        }

        [HttpGet("{id}/Appointments")]
        public async Task<IActionResult> GetAppointmentsByPetIdAsync(int id)
        {
            var appointments = await _appointmentsService.GetAppointmentsByPetIdAsync(id);
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetByIdAsync(int id)
        {
            var pet = await _petsService.GetPetByIdAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            return Ok(pet);
        }

        [HttpGet("/api/Clients/{id}/Pets")]
        public async Task<IActionResult> GetPetsByClientIdAsync(int id)
        {
            var pet = await _petsService.GetPetsByClientIdAsync(id);
            return Ok(pet);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePetAsync([FromBody] CreateUpdatePetDto petDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }

            var pet = await _petsService.CreatePetAsync(petDto);
            
            return Ok(pet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePetAsync(int id, [FromBody] CreateUpdatePetDto petDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var pet = await _petsService.UpdatePetAsync(id, petDto);
            
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
