using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemModels.Entities;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DictionaryEntityService<Role> _rolesService;
        public RolesController(DictionaryEntityService<Role> rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _rolesService.GetAllAsync();
            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }
    }
}
