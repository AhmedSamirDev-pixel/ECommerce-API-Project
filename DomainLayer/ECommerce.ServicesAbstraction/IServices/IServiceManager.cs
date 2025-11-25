using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction.IServices
{
    public interface IServiceManager
    {
        // Exposes product-related business operations
        IProductServices ProductServices { get; }

        // Exposes basket/cart-related business operations
        IBasketService BasketServices { get; }

        // Exposes authentication and identity-related operations (login, register, tokens)
        IAuthenticationServices AuthenticationServices { get; }

        // Exposes order-related business operations (create order, get orders)
        IOrderServices OrderServices { get; }
    }

}
