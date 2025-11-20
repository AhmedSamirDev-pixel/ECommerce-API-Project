using AutoMapper;
using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Domain.Models.Products;
using ECommerce.ServicesAbstraction.IServices;
using ECommerce.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.BusinessServices
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            IGenericRepository<ProductBrand, int> repo = _unitOfWork.GetRepository<ProductBrand, int>();
            IEnumerable<ProductBrand> brands = await repo.GetAllAsync();

            // Convert the list of ProductBrand entities into a list of BrandDTO using AutoMapper.
            var brandsDTO = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDTO>>(brands);
            return brandsDTO;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductAsync()
        {
           IGenericRepository<Product, int> repo = _unitOfWork.GetRepository<Product, int>();
           IEnumerable<Product> products = await repo.GetAllAsync();

            // Convert the list of Product entities into a list of ProductDTO using AutoMapper.
            var productDTO = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
           return productDTO;
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
           IGenericRepository<ProductType, int> repo =  _unitOfWork.GetRepository<ProductType, int>();
           IEnumerable<ProductType> types = await repo.GetAllAsync();

           // Convert the list of ProductType entities into a list of TypeDTO using AutoMapper.
            var typesDTO = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDTO>>(types);
           return typesDTO;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
           Product? product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            // Convert the list of Product entities into a list of ProductDTO using AutoMapper.
            return _mapper.Map<Product, ProductDTO>(product!);
        }
    }
}
