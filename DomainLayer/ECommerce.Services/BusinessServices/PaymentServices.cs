using AutoMapper;
using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Baskets;
using ECommerce.Domain.Models.Orders;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.BasketDTOs;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _baskedRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentServices(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _baskedRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Install Stripe.Net package from NuGet to use Stripe functionalities
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            
            // Get the basket by Id 
            var basket = await _baskedRepository.GetBasketAsync(basketId) ?? throw new BasketNoFoundException(basketId);

            // Get Amount - Get Product + Delivery Method Prices
            var productRepo = _unitOfWork.GetRepository<ECommerce.Domain.Models.Products.Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFound(item.Id);
                item.Price = product.Price;
            }
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value) ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Price;

            var basketAmount = (long)(basket.Items.Sum(i => i.Quantity * i.Price * 100)+ deliveryMethod.Price ) * 100;

            // Create or Update Payment Intent
            var paymentService = new PaymentIntentService();
            if (basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = basketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await paymentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = basketAmount
                };
                await paymentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            // Save changes to basket
            await _baskedRepository.CreateUpdateBasketAsync(basket);

            // Return the basket DTO
            return _mapper.Map<CustomerBasket, BasketDTO>(basket);
        }
    }
}
