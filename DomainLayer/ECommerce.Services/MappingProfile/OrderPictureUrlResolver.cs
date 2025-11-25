using AutoMapper;
using ECommerce.Domain.Models.Orders;
using ECommerce.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfile
{
    public class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration to access BaseURL from appsettings.json
        public OrderPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method constructs the full image URL for the product
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            // If the picture URL is empty, return an empty string
            if (string.IsNullOrEmpty(source.ProductItemOrder.PictureUrl))
                return string.Empty;

            // Build the absolute URL:
            // BaseURL + PictureUrl
            var url = $"{_configuration.GetSection("Urls")["BaseURL"]}{source.ProductItemOrder.PictureUrl}";
            return url;
        }
    }

}
