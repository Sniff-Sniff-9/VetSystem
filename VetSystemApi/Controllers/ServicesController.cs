using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services.Interfaces;
using VetSystemModels.Dto;
using VetSystemModels.Dto.Service;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        
        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServicesAsync()
        {
            var services = await _servicesService.GetServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceByIdAsync(int id)
        {
            var service = await _servicesService.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceAsync([FromBody] CreateUpdateServiceDto serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var service = await _servicesService.CreateServiceAsync(serviceDto);
            return Ok(service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceAsync(int id, [FromBody] CreateUpdateServiceDto serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Field is incorrect.");
            }
            var service = await _servicesService.UpdateServiceAsync(id, serviceDto);
            return Ok(service);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceAsync(int id)
        {
            await _servicesService.DeleteServiceAsync(id);
            return NoContent();
        }
    }
}
