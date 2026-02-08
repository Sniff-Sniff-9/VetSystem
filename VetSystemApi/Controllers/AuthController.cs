using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetSystemApi.Services;
using VetSystemModels.Dto.Login;

namespace VetSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

                var token = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);

                if (token == null)
                {
                    return Unauthorized("Email or password is incorrect.");
                }
                return Ok(new { Token = token });
        }
    }
}
