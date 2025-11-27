using ECommerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction.IServices
{
    public interface IPaymentServices
    {
        Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
