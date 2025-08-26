using core.Enums;
using core.Exceptions;
using core.Models;
using core.Models.Dtos;
using core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> logger;
        private readonly IAuthenticationService authenticationService;
        private readonly IAdministrationService administrationService;

        public AdminController(ILogger<AdminController> logger, IAuthenticationService authenticationService, IAdministrationService administrationService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
            this.administrationService = administrationService;
        }

        [HttpPost("user/create")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult> CreateNewUserAsync(InputUserDto inputUserDto)
        {
            var user = await this.authenticationService.CreateUserAsync(inputUserDto);


            return this.Ok(user);
        }

        [HttpPost("admin/create")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult> CreateNewAdminAsync(InputUserDto inputUserDto)
        {
            var token = await this.authenticationService.CreateAdministratorAsync(inputUserDto);


            return this.Ok(token);
        }

        [HttpDelete("user/delete")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult> DeleteUserAsync(string userMailAddress)
        {
            try
            {
                var ok = await this.authenticationService.DeleteUserAsync(userMailAddress);

                return this.Ok(ok);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(userMailAddress);
            }
            catch (UserDeleteException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("user/update")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> UpdateUserAsync(string userMailAddress, InputUserDto inboundUser)
        {
            var currentUser = this.HttpContext.User;
            var claims = currentUser.Claims;

            var data = HttpContext.User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return Ok();
        }

        [HttpGet("user/list")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult<IEnumerable<OutputUsersWithRolesDto>>> GetAllUsersAsync()
        {
            var usersWithRoles = await this.administrationService.GetAllUsersWithRolesAsync();

            return Ok(usersWithRoles);
        }
    }
}
