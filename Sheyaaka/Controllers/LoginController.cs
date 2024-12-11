using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Sheyaaka.HelperClasses;  // The updated TokenHelper
using Sheyaaka.Repositories;   // Your repository interface for data access
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Sheyaaka.Models;

namespace Sheyaaka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly TokenHelper _tokenHelper;  // Using TokenHelper for token operations

        public LoginController(IRepository<User> userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        // POST: api/Login/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] VMLoginModel loginModel)
        {
            try
            {
                if (loginModel == null)
                    return BadRequest("Invalid request.");

                // Find user by email
                User user = await _userRepository.GetByEmail(loginModel.Email);

                // If user does not exist, return Unauthorized
                if (user == null)
                    return Unauthorized("Invalid credentials.");

                // Verify password 
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash);
                if (!isPasswordCorrect)
                    return Unauthorized("Invalid credentials.");

                // Generate JWT token using email
                var token = _tokenHelper.GenerateEmailConfirmationToken(user.UserID);

                // Return the token to the client
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<User>(ex.Message));
            }
        }        
    }
}
