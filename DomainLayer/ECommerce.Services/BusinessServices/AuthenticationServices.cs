using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Identity;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.IdentityDTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                    Token = "Token - DTO"
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
                    Token = "Token - DTO"
                };
            }

            else
            {
                // Validation 
                var errors = result.Errors.Select(exception => exception.Description).ToList();
                throw new BadRequestException(errors); 
            }
        }
    }
}
