using AutoMapper;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Identity;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.IdentityDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public AuthenticationServices(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;           // Used for creating, finding, updating users
            _configuration = configuration;       // Used to read JWT options from appsettings.json
            _mapper = mapper;                     // Used to map Address <-> AddressDTO
        }

        // Login method:
        // 1. Check if user exists using email
        // 2. Validate password
        // 3. Generate JWT token
        // 4. Return UserDTO (Email, DisplayName, Token)
        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email)
                ?? throw new UserNotFoundException(loginDTO.Email);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isPasswordValid)
                throw new UnAuthorizedException();

            return new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
        }

        // Register method:
        // 1. Create ApplicationUser from RegisterDTO
        // 2. Create the user in Identity DB
        // 3. If failed → collect errors & throw BadRequestException
        // 4. If succeeded → return UserDTO (Email, DisplayName, Token)
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

            if (!result.Succeeded)
            {
                // Collect validation errors and throw custom exception
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new BadRequestException(errors);
            }

            return new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
        }

        // CreateTokenAsync:
        // 1. Create claims (Email, UserName, Id)
        // 2. Include user roles
        // 3. Read JWT config from appsettings.json
        // 4. Generate JWT token and return it as string
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

            // Add user roles to token
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                userClaims.Add(new Claim(ClaimTypes.Role, role));

            // Read security key from configuration
            var securityKey = _configuration.GetSection("JWTOptions")["SecurityKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWTOptions")["Issuer"],
                audience: _configuration.GetSection("JWTOptions")["Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Check if email exists in the system
        // Returns true if user exists, otherwise false
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        // Get current user address:
        // 1. Include Address navigation property
        // 2. If no address → throw AddressNotFoundException
        // 3. Map Address -> AddressDTO
        public async Task<AddressDTO> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);

            if (user.Address is null)
                throw new AddressNotFoundException(user.DisplayName);

            return _mapper.Map<AddressDTO>(user.Address);
        }

        // Get current user info:
        // Re-create token and return UserDTO
        public async Task<UserDTO> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await CreateTokenAsync(user)
            };
        }

        // Update current user address:
        // If address exists → update fields
        // If not exist → create new Address entry
        // Save user changes in Identity
        public async Task<AddressDTO> UpdateCurrentUserAddressAsync(string email, AddressDTO addressDTO)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);

            if (user.Address is not null)
            {
                // Update existing address
                user.Address.Street = addressDTO.Street;
                user.Address.City = addressDTO.City;
                user.Address.Country = addressDTO.Country;
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
            }
            else
            {
                // Create new address
                user.Address = _mapper.Map<Address>(addressDTO);
            }

            await _userManager.UpdateAsync(user);

            return _mapper.Map<AddressDTO>(user.Address);
        }
    }
}
