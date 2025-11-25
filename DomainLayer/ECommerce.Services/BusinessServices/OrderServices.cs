using AutoMapper;
using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Identity;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs.IdentityDTOS;
using ECommerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    public class OrderServices : IOrderServices
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderServices(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OrderToReturnDTO> CreateOrderAsync(OrderDTO orderDTO, string email)
        {
            // Map Address to Order Address
            var orderAddress = _mapper.Map<AddressDTO, OrderAddress>(orderDTO.Address);
            
            // Get Basket
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId) ?? throw new BasketNoFoundException(orderDTO.BasketId);

            // Create OrderItems List 
            List<OrderItem> orderItems = [];

            var productRepo =  _unitOfWork.GetRepository<Product, int>();

            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFound(item.Id);

                var orderItem = new OrderItem()
                {
                    ProductItemOrder = new ProductItemOrder()
                    {
                        ProductId = item.Id,
                        ProductName = item.Name,
                        PictureUrl = item.PictureUrl
                    },

                    Quantity = item.Quantity,
                    Price = item.Price
                };
                
                orderItems.Add(orderItem);
            }


            // Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId) ??  throw new DeliveryMethodNotFoundException(orderDTO.DeliveryMethodId);

            // Sub Total 
            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

            var order = new Order(email, orderAddress, deliveryMethod, orderItems, subTotal);

            _unitOfWork.GetRepository<Order, Guid>().Add(order);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDTO>(order);

        }
    }
}
