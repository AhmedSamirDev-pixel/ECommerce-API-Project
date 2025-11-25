using AutoMapper;
using ECommerce.Domain.Models.Baskets;
using ECommerce.Domain.Models.Identity;
using ECommerce.Domain.Models.Orders;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.DTOs;
using ECommerce.Shared.DTOs.BasketDTOs;
using ECommerce.Shared.DTOs.IdentityDTOS;
using ECommerce.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfile
{
    public class ProjectProfile : Profile
    {
        private readonly IConfiguration _configuration;
        public ProjectProfile(IConfiguration configuration)
        {
            _configuration = configuration;

            // Mapping configurations
            CreateMap<Product, ProductDTO>()
                // Custom mappings for nested properties
                // Mapping Brand.Name to BrandName in ProductDTO
                .ForMember(destination => destination.BrandName,
                            options => options.MapFrom(source => source.Brand.Name))
                // Mapping Type.Name to TypeName in ProductDTO
                .ForMember(destination => destination.TypeName,
                            options => options.MapFrom(source => source.Type.Name))

                // Mapping PictureUrl with a custom URL format
                .ForMember(destination => destination.PictureUrl,
                            options => options.MapFrom(new PictureUrlResolver(_configuration)));

            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();

            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();    
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();

            CreateMap<Address, AddressDTO>().ReverseMap();

            // Maps AddressDTO to OrderAddress and vice versa (ReverseMap)
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();

            // Maps Order domain model to OrderToReturnDTO
            CreateMap<Order, OrderToReturnDTO>()
                // Map the DeliveryMethod property to the ShortName of the DeliveryMethod entity
                .ForMember(dest => dest.DeliveryMethod,
                           options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(d => d.Address, opt => opt.MapFrom(src => src.OrderAddress));

            // Maps OrderItem domain model to OrderItemDTO
            CreateMap<OrderItem, OrderItemDTO>()
                // Map ProductName from the nested ProductItemOrder object
                .ForMember(dest => dest.ProductName,
                           options => options.MapFrom(src => src.ProductItemOrder.ProductName))

                // Map PictureUrl using a custom resolver (OrderPictureUrlResolver)
                .ForMember(dest => dest.PictureUrl,
                           options => options.MapFrom(new OrderPictureUrlResolver(configuration)));
        }
    }
}
