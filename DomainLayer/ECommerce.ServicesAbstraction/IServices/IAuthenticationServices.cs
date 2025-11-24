using ECommerce.Shared.DTOs.IdentityDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction.IServices
{
    public interface IAuthenticationServices
    {
        // Login
        // Takes LoginDTO (Email + Password)
        // Returns UserDTO (Token, Email, DisplayName)
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);

        // Register
        // Takes RegisterDTO (Email, Password, Username, DisplayName, PhoneNumber)
        // Returns UserDTO (Token, Email, DisplayName)
        Task<UserDTO> RegisterAsync(RegisterDTO registerDTO);

        // Check Email
        // Takes email and returns true if email exists, otherwise false
        Task<bool> CheckEmailAsync(string email);

        // Get Current User Address
        // Takes email and returns AddressDTO for that user
        Task<AddressDTO> GetCurrentUserAddressAsync(string email);

        // Update Current User Address
        // Takes email + AddressDTO and updates the user's address
        // Returns the updated AddressDTO
        Task<AddressDTO> UpdateCurrentUserAddressAsync(string email, AddressDTO addressDTO);

        // Get Current User Details
        // Takes email and returns UserDTO (Token, Email, DisplayName)
        Task<UserDTO> GetCurrentUserAsync(string email);
    }
}
