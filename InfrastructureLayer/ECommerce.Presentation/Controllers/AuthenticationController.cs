using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.IdentityDTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _serviceManager.AuthenticationServices.LoginAsync(loginDTO);
            return Ok(user);
        }

         
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO registerDTO)
        {
            var user = await _serviceManager.AuthenticationServices.RegisterAsync(registerDTO);
            return Ok(user);
        }
    }
}
