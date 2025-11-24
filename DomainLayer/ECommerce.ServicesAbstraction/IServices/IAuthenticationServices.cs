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
        // Take Email, Password Then Return Token, Email, Display Name.
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);

        // Register
        // Take Email, Password, Username, Display Name, Phone Number,
        // Return Token , Email, Display Name.

        Task<UserDTO> RegisterAsync(RegisterDTO registerDTO);
    }
}
