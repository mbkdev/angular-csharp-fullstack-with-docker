using core.Enums;
using core.Exceptions;
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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthenticationService> logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AuthenticationService(ILogger<AuthenticationService> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, IConfiguration configuraion)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

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
            => await userManager.AddToRoleAsync(identityUser, roleType.ToString());
        /*
        public async Task<string> CreateUserAsync(InputUserDto inputUserDto)
        {
            try
            {
                var user = new IdentityUser
                {
                    Email = inputUserDto.Email,
                    //UserName = inputUserDto.Username,
                    //PhoneNumber = inputUserDto.PhoneNumber
                };

                await this.AddUserRoleIfNotExistsAsync(RoleTypes.User);

                var result = await userManager.CreateAsync(user, inputUserDto.Password);
                await this.AddUserToRoleIfNotExistsAsync(user, RoleTypes.User);

                var errors = result.Errors.Select(x => x.Description);

                if (result.Succeeded)
                {
                    return await this.BuildTokenAsync(inputUserDto.ConvertToUserModel(), [RoleTypes.User]);
                }
                else
                {
                    throw new CreateUserException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        */
        //public async Task<string> CreateAdministratorAsync(InputUserDto inboundUser)
        //{
        //    try
        //    {
        //        var user = CreateNewIdentityUser(inboundUser.Email, "");

        //        await this.AddUserRoleIfNotExistsAsync(RoleTypes.Admin);

        //        var result = await userManager.CreateAsync(user, inboundUser.Password);
        //        await this.AddUserToRoleIfNotExistsAsync(user, RoleTypes.Admin);

        //        var errors = result.Errors.Select(x => x.Description);

        //        if (result.Succeeded)
        //        {
        //            var token = await this.BuildTokenAsync(inboundUser.ConvertToUserModel(), [RoleTypes.Admin]);
        //            return token;
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception();
        //    }
        //}

        //public async Task<string> LoginUserAsync(InputLoginUserDto inputLoginUserDto)
        //{
        //    /*

        //TUser val = await UserManager.FindByNameAsync(userName);
        //if (val == null)
        //{
        //    return SignInResult.Failed;
        //}

        //return await PasswordSignInAsync(val, password, isPersistent, lockoutOnFailure);
        //     */

        //    //var val = await userManager.FindByNameAsync(inputLoginUserDto.Username);
        //    //if (val != null)
        //    //{
        //    //    var signInResult = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);

        //    //    if (val == null)
        //    //    {
        //    //        throw new ArgumentNullException("user");
        //    //    }

        //    //    SignInResult signInResult = await PreSignInCheck(user);
        //    //    if (signInResult != null)
        //    //    {
        //    //        return signInResult;
        //    //    }

        //    //    if (await UserManager.CheckPasswordAsync(user, password))
        //    //    {
        //    //        bool isEnabled;
        //    //        bool flag = AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess", out isEnabled) && isEnabled;
        //    //        if (!flag)
        //    //        {
        //    //            flag = !(await IsTfaEnabled(user));
        //    //        }

        //    //        if (flag && !(await ResetLockoutWithResult(user)).Succeeded)
        //    //        {
        //    //            return SignInResult.Failed;
        //    //        }

        //    //        return SignInResult.Success;
        //    //    }

        //    //    ILogger logger = Logger;
        //    //    EventId eventId = 2;
        //    //    string text = await UserManager.GetUserIdAsync(user);
        //    //    logger.LogWarning(eventId, "User {userId} failed to provide the correct password.", text);
        //    //    if (UserManager.SupportsUserLockout && lockoutOnFailure)
        //    //    {
        //    //        if (!((await UserManager.AccessFailedAsync(user)) ?? IdentityResult.Success).Succeeded)
        //    //        {
        //    //            return SignInResult.Failed;
        //    //        }

        //    //        if (await UserManager.IsLockedOutAsync(user))
        //    //        {
        //    //            return await LockedOut(user);
        //    //        }
        //    //    }

        //    //    return SignInResult.Failed;

        //    //    return (!signInResult.Succeeded) ? signInResult : (await SignInOrTwoFactorAsync(user, isPersistent));
        //    //}
        //    try
        //    {
        //        var result = await signInManager.PasswordSignInAsync(inputLoginUserDto.Username, inputLoginUserDto.Password, false, false);
        //        if (result.Succeeded)
        //        {
        //            return await this.BuildTokenAsync(inputLoginUserDto.ConvertToUserModel());
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    throw new InvalidEmailOrPasswordException();
        //}


        public async Task<bool> DeleteUserAsync(string userEmail)
        {
            var identityUser = await userManager.FindByEmailAsync(userEmail);
            if (identityUser == null)
            {
                throw new UserNotFoundException();
            }

            var identityResult = await userManager.DeleteAsync(identityUser);
            if (!identityResult.Succeeded)
            {
                var error = FormatErrorsToString(identityResult.Errors);
                throw new UserDeleteException(error);
            }

            return true;
        }

        public async Task<string> CreateUserAsync(InputUserDto inputUserDto)
        {
            try
            {
                var user = new IdentityUser { UserName = inputUserDto.Email, Email = inputUserDto.Email };

                await this.AddUserRoleIfNotExistsAsync(RoleTypes.User);
                //bool userRoleExists = await roleManager.RoleExistsAsync(RoleTypes.User.ToString());

                //if (!userRoleExists)
                //{
                //    await roleManager.CreateAsync(new IdentityRole("User"));
                //}

                var result = await userManager.CreateAsync(user, inputUserDto.Password);
                //await userManager.AddToRoleAsync(user, "User");
                await this.AddUserToRoleAsync(user, RoleTypes.User);

                var errors = result.Errors.Select(e => e.Description);

                if (result.Succeeded)
                {
                    return await BuildToken(inputUserDto);
                }
                else
                {
                    throw new CreateUserException();
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public async Task<string> CreateAdministratorAsync(InputUserDto inputUserDto)
        {
            try
            {
                var user = new IdentityUser { UserName = inputUserDto.Email, Email = inputUserDto.Email };

                bool userRoleExists = await roleManager.RoleExistsAsync(RoleTypes.User.ToString());

                if (!userRoleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                bool adminRoleExists = await roleManager.RoleExistsAsync(RoleTypes.Admin.ToString());

                if (!adminRoleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var result = await userManager.CreateAsync(user, inputUserDto.Password);
                await userManager.AddToRoleAsync(user, "User");
                await userManager.AddToRoleAsync(user, "Admin");

                var errors = result.Errors.Select(e => e.Description);

                if (result.Succeeded)
                {
                    var token = await BuildToken(inputUserDto);
                    return token;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }


        public async Task<string> LoginUserAsync(InputLoginUserDto inputLoginUserDto)
        {
            var result = await this.signInManager.PasswordSignInAsync(inputLoginUserDto.Email, inputLoginUserDto.Password,
                 isPersistent: false, lockoutOnFailure: false);

            logger.LogInformation(result.Succeeded.ToString());

            if (result.Succeeded)
            {
                var inputUserDto = new InputUserDto { Email = inputLoginUserDto.Email, Password = inputLoginUserDto.Password };

                logger.LogInformation("Build Token");

                var token = await BuildToken(inputUserDto);
                return token;
            }
            else
            {
                throw new InvalidEmailOrPasswordException("lalalalalalalala");
            }
        }

        public async Task LogoutUserAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        private async Task<string> BuildToken(InputUserDto userInfo, RoleTypes[]? roleTypes = null)
        {
            var user = await this.userManager.FindByEmailAsync(userInfo.Email);
            if (user == null) return null;

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            logger.LogInformation("Before Write token");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private async Task<string> BuildToken(InputUserDto userInfo, RoleTypes[]? roleTypes)
        //{
        //    var user = await this.userManager.FindByEmailAsync(userInfo.Email);
        //    if (user == null) return null;

        //    var claims = new List<Claim>() {
        //      new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
        //      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    foreach (var role in roleTypes)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        //    }

        //    var userRoles = await this.userManager.GetRolesAsync(user);

        //    foreach (var role in userRoles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:key"]));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var expiration = DateTime.UtcNow.AddHours(1);
        //    JwtSecurityToken token = new JwtSecurityToken(
        //       issuer: null,
        //       audience: null,
        //       claims: claims,
        //       expires: expiration,
        //       signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        private IdentityUser CreateNewIdentityUser(string emailAddress, string username)
            => new() { Email = emailAddress, UserName = username };

        private string FormatErrorsToString(IEnumerable<IdentityError> errors)
        {
            var stringBuilder = new StringBuilder();

            foreach (var error in errors)
            {
                stringBuilder.AppendLine($"- {error.Description}");
            }

            return stringBuilder.ToString();
        }
    }
}
