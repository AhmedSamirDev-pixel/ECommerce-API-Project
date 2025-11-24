using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.IdentityDTOS
{
    public class LoginDTO
    {
        [EmailAddress]
       public string Email { get; set; } = null!;
       public string Password { get; set; } = null!;
    }
}
