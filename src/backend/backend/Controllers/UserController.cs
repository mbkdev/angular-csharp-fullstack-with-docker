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
    public class UserController(ILogger<UserController> logger, IAuthenticationService authenticationService, IUserService userService) : ControllerBase
    {
        private readonly ILogger<UserController> logger = logger;
        private readonly IAuthenticationService authenticationService = authenticationService;
        private readonly IUserService userService = userService;

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(InputLoginUserDto inputLoginUserDto)
        {

            try
            {
                var loginToken = await this.authenticationService.LoginUserAsync(inputLoginUserDto);

                return this.Ok(loginToken);
            }
            catch (InvalidEmailOrPasswordException)
            {
                return this.Unauthorized();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogoutUser()
        {
            await this.authenticationService.LogoutUserAsync();

            return this.Ok();
        }

        // Change Password
        // Delete Account
        // Change UserData
        // GetUserData

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<OutputUserDto>> GetCurrentUsersProfileAsync()
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId is null)
                {
                    // Kann das überhaupt sein? Ist ja Authorize Attribut da
                }

                var userFromService = await this.userService.GetCurrentUserProfileAsync(userId);
                if (userFromService is null)
                {
                    throw new UserNotFoundException();
                }

                var user = new OutputUserDto
                {
                    Email = userFromService.Email,
                    Username = userFromService.Username
                };

                return this.Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (UserNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
