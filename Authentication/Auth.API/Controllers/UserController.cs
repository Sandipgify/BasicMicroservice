using Auth.Service.DTO;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserDTO user, string password)
        {
            try
            {
                await _userService.AddUserAsync(user, password);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server errror");
            }

        }
        [HttpPut]
        public async Task<IActionResult> Update(UserDTO userDTO)
        {
            try
            {
                await _userService.UpdateUserAsync(userDTO);
                return NoContent();
            }

            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server errror");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetAllUsersAsync();
            return Ok(result);
        }
    }
}
