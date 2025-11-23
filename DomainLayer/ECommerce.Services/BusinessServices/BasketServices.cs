using AutoMapper;
using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Baskets;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.BasketDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    // Service responsible for managing customer baskets (shopping carts)
    // Handles creation, updating, retrieval, and deletion of baskets via IBasketRepository
    public class BasketServices : IBasketService
    {
        private readonly IBasketRepository _basketRepository; // Redis-backed repository for basket data
        private readonly IMapper _mapper; // AutoMapper for mapping between domain and DTO objects

        // Constructor injects repository and mapper
        public BasketServices(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        // Creates a new basket or updates an existing one
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            // Map DTO to domain entity
            var customerBasket = _mapper.Map<CustomerBasket>(basket);

            // Save basket using repository
            var saveBasket = await _basketRepository.CreateUpdateBasketAsync(customerBasket);

            // If saved successfully, return the saved basket as DTO
            if (saveBasket is not null)
                return await GetBasketAsync(saveBasket.Id);
            else
                throw new Exception("Something went wrong during basket processing");
        }

        // Deletes a basket by key (usually user ID)
        public async Task<bool> DeleteBasketAsync(string key)
        {
            // Calls repository to remove the basket from Redis
            return await _basketRepository.DeleteBasketAsync(key);
        }

        // Retrieves a basket by key (usually user ID)
        public async Task<BasketDTO> GetBasketAsync(string key)
        {
            // Get basket from repository
            var basket = await _basketRepository.GetBasketAsync(key);

            // If basket exists, map to DTO and return
            if (basket is not null)
                return _mapper.Map<BasketDTO>(basket);
            else
                // Throw custom exception if basket not found
                throw new BasketNoFoundException(key);
        }
    }
}
