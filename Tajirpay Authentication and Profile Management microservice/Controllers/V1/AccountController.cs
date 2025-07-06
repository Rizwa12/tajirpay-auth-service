using Application.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tajirpay_Authentication_and_Profile_Management_microservice.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _userService.RegisterAsync(request);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {

            var token = await _userService.LoginAsync(request);
            return Ok(new
            {
                success = true,
                message = "Login successful",
                token = token
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _userService.RefreshTokenAsync(request);
            return Ok(new
            {
                success = true,
                message = "Token refreshed successfully",
                data = response
            });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            await _userService.VerifyEmailAsync(request.Email!, request.Token!);
            return Ok(new
            {
                success = true,
                message = "Email verified successfully."
            });
        }


    }
}
