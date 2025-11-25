using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public OrderController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _serviceManager.OrderServices.CreateOrderAsync(orderDTO, email);
            return Ok(order);
        }

        [HttpGet("DeliveryMethods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.OrderServices.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }


        [HttpGet("AllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetAllOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _serviceManager.OrderServices.GetAllOrdersAsync(email);
            return Ok(orders);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderToReturnDTO>> GetAllOrdersForUser(Guid orderId)
        {
            var order = await _serviceManager.OrderServices.GetOrderByIdAsync(orderId);
            return Ok(order);
        }


    }
}
