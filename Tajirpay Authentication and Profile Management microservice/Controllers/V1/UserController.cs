using Application.DTOS;
using Application.Interfaces;
using Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tajirpay_Authentication_and_Profile_Management_microservice.Controllers.V1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { error = "Invalid or missing token." });
            var userId = Guid.Parse(userIdClaim);
            var profile = await _userService.GetProfileAsync(userId);
            return Ok(profile);
        }

        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateName(Guid id, [FromBody] UpdateNameRequest request)
        {

            await _userService.UpdateNameAsync(id, request);
            return Ok(new { success = true, message = "Name updated successfully." });
        }

    }
}
