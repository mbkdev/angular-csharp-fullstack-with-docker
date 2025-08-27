using core.Enums;
using core.Exceptions;
using core.Extensions;
using core.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace core.Services
{
    public class AuthenticationService(ILogger<AuthenticationService> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager) : IAuthenticationService
    {
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger<AuthenticationService> logger = logger;
        private readonly UserManager<IdentityUser> userManager = userManager;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly SignInManager<IdentityUser> signInManager = signInManager;

        public async Task AddUserRoleIfNotExistsAsync(RoleTypes roleType)
        {
            var roleTypeString = roleType.ToString();

            var userRoleExists = await this.roleManager.RoleExistsAsync(roleTypeString);
            if (!userRoleExists)
            {
                await this.roleManager.CreateAsync(new IdentityRole(roleTypeString));
            }
        }

        public async Task AddUserToRoleAsync(IdentityUser identityUser, RoleTypes roleType)
            => await this.userManager.AddToRoleAsync(identityUser, roleType.ToString());

        public async Task<bool> DeleteUserAsync(string userEmail)
        {
            var identityUser = await this.userManager.FindByEmailAsync(userEmail);
            if (identityUser is null)
            {
                throw new UserNotFoundException();
            }

            var identityResult = await this.userManager.DeleteAsync(identityUser);
            if (!identityResult.Succeeded)
            {
                var error = FormatErrorsToString(identityResult.Errors);
                throw new UserDeleteException(error);
            }

            return true;
        }

        public async Task<string> CreateUserAsync(InputUserDto inputUserDto)
        {
            var user = new IdentityUser { UserName = inputUserDto.Email, Email = inputUserDto.Email };

            await this.AddUserRoleIfNotExistsAsync(RoleTypes.User);

            var result = await this.userManager.CreateAsync(user, inputUserDto.Password);
            await this.AddUserToRoleAsync(user, RoleTypes.User);

            var errors = result.Errors.Select(e => e.Description);

            if (result.Succeeded)
            {
                return await this.BuildToken(inputUserDto);
            }
            else
            {
                throw new CreateUserException(FormatErrorsToString(errors));
            }
        }

        public async Task<string> CreateAdministratorAsync(InputUserDto inputUserDto)
        {
            var user = new IdentityUser { UserName = inputUserDto.Email, Email = inputUserDto.Email };

            var userRoleExists = await this.roleManager.RoleExistsAsync(RoleTypes.User.ToString());

            if (!userRoleExists)
            {
                await this.roleManager.CreateAsync(new IdentityRole("User"));
            }

            var adminRoleExists = await this.roleManager.RoleExistsAsync(RoleTypes.Admin.ToString());

            if (!adminRoleExists)
            {
                await this.roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var result = await this.userManager.CreateAsync(user, inputUserDto.Password);
            await this.userManager.AddToRoleAsync(user, "User");
            await this.userManager.AddToRoleAsync(user, "Admin");

            var errors = result.Errors.Select(e => e.Description);

            if (result.Succeeded)
            {
                var token = await this.BuildToken(inputUserDto);
                return token;
            }
            else
            {
                throw new CreateUserException(FormatErrorsToString(errors));
            }
        }

        public async Task<string> LoginUserAsync(InputLoginUserDto inputLoginUserDto)
        {
            var result = await this.signInManager.PasswordSignInAsync(inputLoginUserDto.Email, inputLoginUserDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var inputUserDto = new InputUserDto { Email = inputLoginUserDto.Email, Password = inputLoginUserDto.Password };

                return await this.BuildToken(inputUserDto);
            }
            else
            {
                throw new InvalidEmailOrPasswordException();
            }
        }

        public async Task LogoutUserAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        private async Task<string> BuildToken(InputUserDto userInfo, RoleTypes[]? roleTypes = null)
        {
            var user = await this.userManager.FindByEmailAsync(userInfo.Email);
            if (user is null)
            {
                throw new UserNotFoundException(userInfo.Email);
            }

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Email, userInfo.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (roleTypes != null)
            {
                foreach (var role in roleTypes)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            var userRoles = await this.userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = this.configuration.GetJwtKeyIfExists();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string FormatErrorsToString(IEnumerable<IdentityError> errors)
        {
            var stringBuilder = new StringBuilder();

            foreach (var error in errors)
            {
                stringBuilder.AppendLine($"- {error.Description}");
            }

            return stringBuilder.ToString();
        }

        private static string FormatErrorsToString(IEnumerable<string> errors)
        {
            var stringBuilder = new StringBuilder();

            foreach (var error in errors)
            {
                stringBuilder.AppendLine($"- {error}");
            }

            return stringBuilder.ToString();
        }
    }
}
