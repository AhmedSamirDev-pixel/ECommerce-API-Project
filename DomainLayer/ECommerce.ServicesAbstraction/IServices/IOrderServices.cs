using ECommerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction.IServices
{
    public interface IOrderServices
    {
        // Create Order
        Task<OrderToReturnDTO> CreateOrderAsync(OrderDTO orderDTO, string email);

        // GetAll DeliveryMethod
        Task<IEnumerable<DeliveryMethodDTO>> GetDeliveryMethodAsync();

        // Get All Orders For Current User
        Task<IEnumerable<OrderToReturnDTO>> GetAllOrdersAsync(string email);

        // Get Specific Order For Current User
        Task<OrderToReturnDTO> GetOrderByIdAsync(Guid orderId);



    }
}
