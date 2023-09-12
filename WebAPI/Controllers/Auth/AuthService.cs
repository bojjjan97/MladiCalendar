using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.AppDtos;
using WebAPI.AppDtos.ConfigModels;
using WebAPI.AppDtos.Enums;
using WebAPI.Controllers.Auth.Dtos;
using WebAPI.Models;
using WebAPI.Models.Enums;

namespace WebAPI.Controllers.Auth
{
    public class AuthService
    {
        private readonly UserManager<WebAPI.Models.User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AuthConfig _config;

        public AuthService(UserManager<WebAPI.Models.User> userManager, RoleManager<IdentityRole> roleManager, IOptions<AuthConfig> config)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _config = config.Value;
        }

        public async Task<ServiceResponseDto> Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null)
                return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "User doesnt exist" };
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));

                var token = new JwtSecurityToken(
                    //issuer: _configuration["JWT:ValidIssuer"],
                    //audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                UserToken userToken = new UserToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ValidTo = token.ValidTo
                };
                return new ServiceResponseDto { ResponseType = EResponseType.Success,Message = "Successfull login", ResponseObject = userToken };
            }
            else
                return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "Wrong password" };
        }

        public async Task<ServiceResponseDto> Register(RegisterDto model,bool? adminUser)
        {
            var userExists = await userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "User with entered email already exists!" };

            WebAPI.Models.User user = new WebAPI.Models.User()
            {
                UserName = model.Email,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserType = (EUserType) model.UserType,
                CountryId = model.CountryId
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new ServiceResponseDto { ResponseType = EResponseType.Error, Message = "User creation failed! Please check user details and try again." };
            

            if (adminUser == true)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            return new ServiceResponseDto{ ResponseType = EResponseType.Success, Message = "Account created successfully!" };
        }

        public async Task<ServiceResponseDto> ChangeUserData(ChangeUserDto changeUser, string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.FirstName = changeUser.FirstName;
            user.LastName = changeUser.LastName;
            user.PhoneNumber = changeUser.PhoneNumber;
            user.CountryId = changeUser.CountryId;
            user.UserType = (EUserType)changeUser.UserType;
            await this.userManager.UpdateAsync(user);
            return new ServiceResponseDto { ResponseType = EResponseType.Success, Message = "Account updated successfully!" };
        }

    }
}
