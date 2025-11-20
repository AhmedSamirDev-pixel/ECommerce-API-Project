using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using ECommerce.Domain.Models.Products;
using ECommerce.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfile
{
    public class PictureUrlResolver: IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;
        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, 
            string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;

            else
            {
                var url = $"{_configuration.GetSection("Urls")["BaseURL"]}{source.PictureUrl}";
                return url;
            }
        }
    }
}
