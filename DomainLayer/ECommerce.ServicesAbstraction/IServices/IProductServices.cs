using ECommerce.Shared.Common;
using ECommerce.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction.IServices
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDTO>> GetAllProductAsync(ProductQueryParam productQueryParam);
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
    }
}
