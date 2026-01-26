using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientsService.GetClientsAsync();
            if (clients == null)
            {
                return NotFound();
            }
            return Ok(clients);
        }

        [HttpGet("ClientId/{id}")]
        public async Task<IActionResult> GetClientByClientId(int id)
        {
            var client = await _clientsService.GetClientByClientIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet("UserId/{id}")]
        public async Task<IActionResult> GetClientByUserId(int id)
        {
            var client = await _clientsService.GetClientByUserIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientAsync([FromBody] ClientDto clientDto)
        {
            var client = await _clientsService.CreateClientAsync(clientDto);
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientAsync(int id, [FromBody] UpdateClientDto updateClientDto)
        {

            var client = await _clientsService.UpdateClientAsync(id, updateClientDto);
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            return Ok(client);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _clientsService.DeleteClientAsync(id);
            return NoContent();
        }
    }
}
