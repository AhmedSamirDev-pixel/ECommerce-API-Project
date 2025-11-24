using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Identity;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.IdentityDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationServices(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email) ?? throw new UserNotFoundException(loginDTO.Email);

            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (IsPasswordValid)
            {
                return new UserDTO()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }

            else
            {
                throw new UnAuthorizedException();
            }

        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser()
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                return new UserDTO()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }

            else
            {
                // Validation 
                var errors = result.Errors.Select(exception => exception.Description).ToList();
                throw new BadRequestException(errors); 
            }
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = _configuration.GetSection("JWTOptions")["SecurityKey"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: _configuration.GetSection("JWTOptions")["Issuer"],
                    audience: _configuration.GetSection("JWTOptions")["Audience"], 
                    claims: userClaims,
                    expires: DateTime.Now.AddDays(2), 
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
