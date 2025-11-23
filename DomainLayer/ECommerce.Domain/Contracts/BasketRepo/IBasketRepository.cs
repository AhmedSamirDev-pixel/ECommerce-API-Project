using ECommerce.Domain.Models.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.BasketRepo
{
    // Interface for managing customer baskets (shopping carts)
    // Defines methods for retrieving, creating/updating, and deleting baskets
    public interface IBasketRepository
    {
        // Retrieves a CustomerBasket from storage (Redis) by its key
        // key: usually the user ID
        // Returns the basket if found, otherwise null
        Task<CustomerBasket?> GetBasketAsync(string key);

        // Creates a new basket or updates an existing one
        // basket: the CustomerBasket object to save
        // timeToLive: optional expiration time for the basket in storage
        // Returns the saved CustomerBasket, or null if saving fails
        Task<CustomerBasket?> CreateUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null);

        // Deletes a basket from storage by its key
        // key: usually the user ID
        // Returns true if deletion was successful, false otherwise
        Task<bool> DeleteBasketAsync(string key);
    }
}
