using Auth.Service.DTO;
using Auth.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> Auth(CredentialDTO credential)
        {
            try
            {
              return Ok(await _authService.VerifyPasswordAsync(credential));
            }
           
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
                
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
}
