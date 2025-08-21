using core.Enums;
using core.Exceptions;
using core.Models.Dtos;
using core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IAuthenticationService authenticationService;

        public UserController(ILogger<UserController> logger, IAuthenticationService authenticationService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(InputLoginUserDto inputLoginUserDto)
        {
            logger.LogInformation("LoginUser");

            try
            {
                var t = await this.authenticationService.LoginUserAsync(inputLoginUserDto);
                return Ok(new { t });
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
        [HttpGet("currentUser")]
        public async Task<ActionResult> GetLoggedInUser()
        {
            var user = HttpContext.User;

            var claims = user.Claims;

            // get claim nameidentifier -> Id

            var data = HttpContext.User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return Ok(data);
        }
    }
}
