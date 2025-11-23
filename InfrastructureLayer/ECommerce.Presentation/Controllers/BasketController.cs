using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.BasketDTOs;
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
    public class BasketController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public BasketController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDTO>> GetBasket(string key)
        {
           var basket = await _serviceManager.BasketServices.GetBasketAsync(key);
           return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<BasketDTO>> CreateUpdateBasket(BasketDTO basket)
        {
           var internalBasket = await _serviceManager.BasketServices.CreateOrUpdateBasketAsync(basket);

            return Ok(internalBasket);
        }


        [HttpDelete("{key}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBasket(string key)
        {
            var result = await _serviceManager.BasketServices.DeleteBasketAsync(key);

            if (result)
                return NoContent(); // 204 No Content
            else
                return NotFound(); // 404 if basket not found
        }

    }
}
