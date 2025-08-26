using core.Enums;
using core.Exceptions;
using core.Models.Dtos;
using core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, IAuthenticationService authenticationService, IUserService userService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(InputLoginUserDto inputLoginUserDto)
        {
            logger.LogInformation("LoginUser");

            try
            {
                var t = await this.authenticationService.LoginUserAsync(inputLoginUserDto);
                return Ok(t);
            }
            catch(InvalidEmailOrPasswordException e)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogoutUser()
        {
            await this.authenticationService.LogoutUserAsync();

            return Ok();
        }

        [HttpPost("test")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public async Task<ActionResult> Test(InputLoginUserDto inputLoginUserDto)
        {
            return Ok("Teest");
        }

        // Change Password
        // Delete Account
        // Change UserData
        // GetUserData

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<OutputUserDto>> GetCurrentUsersProfileAsync()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userFromService = await this.userService.GetCurrentUserProfileAsync(userId);

            var user = new OutputUserDto
            {
                Email = userFromService.Email,
                Username = userFromService.Username
            };

            return Ok(user);
        }
    }
}
