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
        Task<OrderToReturnDTO> CreateOrderAsync(OrderDTO orderDTO, string email);
    }
}
