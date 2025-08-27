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
    public class AdminController(ILogger<AdminController> logger, IAuthenticationService authenticationService, IAdministrationService administrationService) : ControllerBase
    {
        private readonly ILogger<AdminController> logger = logger;
        private readonly IAuthenticationService authenticationService = authenticationService;
        private readonly IAdministrationService administrationService = administrationService;

        [HttpPost("user/create")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult> CreateNewUserAsync(InputUserDto inputUserDto)
        {
            try
            {
                var user = await this.authenticationService.CreateUserAsync(inputUserDto);

                return this.Ok(user);
            }
            catch (CreateUserException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("admin/create")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult> CreateNewAdminAsync(InputUserDto inputUserDto)
        {
            try
            {
                var token = await this.authenticationService.CreateAdministratorAsync(inputUserDto);

                return this.Ok(token);
            }
            catch (CreateUserException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
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
            catch (UserNotFoundException)
            {
                return this.NotFound(userMailAddress);
            }
            catch (UserDeleteException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }




        [HttpGet("user/list")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.Admin))]
        public async Task<ActionResult<IEnumerable<OutputUsersWithRolesDto>>> GetAllUsersAsync()
        {
            var usersWithRoles = await this.administrationService.GetAllUsersWithRolesAsync();

            return this.Ok(usersWithRoles);
        }
    }
}
