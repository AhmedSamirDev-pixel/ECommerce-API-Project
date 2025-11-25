using AutoMapper;
using ECommerce.Domain.Contracts.BasketRepo;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Models.Identity;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Services.Specifications;
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
            // Map the incoming AddressDTO to the OrderAddress domain model
            var orderAddress = _mapper.Map<AddressDTO, OrderAddress>(orderDTO.Address);

            // Retrieve the basket using the basket ID
            // Throw BasketNotFoundException if it does not exist
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId)
                ?? throw new BasketNoFoundException(orderDTO.BasketId);

            // Initialize list that will hold OrderItem entities
            List<OrderItem> orderItems = [];

            // Get product repository through Unit of Work
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            // Loop through each basket item and convert it to an OrderItem
            foreach (var item in basket.Items)
            {
                // Fetch the product from DB (for price and validation)
                // Throw ProductNotFound if product does not exist
                var product = await productRepo.GetByIdAsync(item.Id)
                    ?? throw new ProductNotFound(item.Id);

                // Create the OrderItem including nested ProductItemOrder
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

                // Add the order item to the list
                orderItems.Add(orderItem);
            }

            // Get the delivery method by ID
            // Throw DeliveryMethodNotFoundException if not found
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderDTO.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderDTO.DeliveryMethodId);

            // Calculate subtotal = sum of (price * quantity)
            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

            // Create a new Order domain entity using the constructor
            var order = new Order(email, orderAddress, deliveryMethod, orderItems, subTotal);

            // Add the order to the repository
            _unitOfWork.GetRepository<Order, Guid>().Add(order);

            // Save all changes in the database
            await _unitOfWork.SaveChangesAsync();

            // Map the created order to a DTO that will be returned to the client
            return _mapper.Map<Order, OrderToReturnDTO>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDTO>> GetDeliveryMethodAsync()
        {
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(deliveryMethod);
        }

        public async Task<IEnumerable<OrderToReturnDTO>> GetAllOrdersAsync(string email)
        {
            var specification = new OrderSpecification(email);

            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllWithSpecificationsAsync(specification);

            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDTO>>(orders);

        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(Guid orderId)
        {
            var specification = new OrderSpecification(orderId);

            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdWithSpecificationsAsync(specification);

            return _mapper.Map<Order, OrderToReturnDTO>(order);
        }

        
    }

}
