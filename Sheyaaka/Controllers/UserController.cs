using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Sheyaaka.Services; // Import the UserService

namespace Sheyaaka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            var result = await _userService.RegisterUser(user);
            if (result.Contains("Please check your email"))
                return Ok(result);

            return BadRequest(result);
        }

        // GET: api/User/confirm-email
        [HttpGet("confirm-email")]
        public IActionResult ConfirmEmail(string token)
        {
            var result = _userService.ConfirmEmail(token);
            if (result.Contains("successfully"))
                return Ok(result);

            return BadRequest(result);
        }

        // PUT: api/User/change-password
        [HttpPut("change-password")]
        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = _userService.ChangePassword(request);
            if (result.Contains("successfully"))
                return Ok(result);

            return BadRequest(result);
        }

        // PUT: api/User/update-profile/{id}
        [HttpPut("update-profile/{id}")]
        public ActionResult UpdateProfile(int id, [FromBody] User updatedUser)
        {
            var result = _userService.UpdateProfile(id, updatedUser);
            if (result.Contains("successfully"))
                return Ok(result);

            return BadRequest(result);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
