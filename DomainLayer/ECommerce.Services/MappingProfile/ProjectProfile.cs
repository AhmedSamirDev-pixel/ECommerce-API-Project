using AutoMapper;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.DTOs;
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
        }
    }
}
