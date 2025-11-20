using AutoMapper;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfile
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // Mapping configurations
            CreateMap<Product, ProductDTO>()
                // Custom mappings for nested properties
                // Mapping Brand.Name to BrandName in ProductDTO
                .ForMember(destination => destination.BrandName,
                            options => options.MapFrom(source => source.Brand.Name))
                // Mapping Type.Name to TypeName in ProductDTO
                .ForMember(destination => destination.TypeName,
                            options => options.MapFrom(source => source.Type.Name));
            
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
        }
    }
}
