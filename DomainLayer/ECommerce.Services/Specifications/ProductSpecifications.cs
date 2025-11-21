using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Common;
using ECommerce.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications
{
    public class ProductSpecifications : BaseSpecifications<Product, int>
    {
        public ProductSpecifications(ProductQueryParam productQueryParam) : base(product => (!productQueryParam.brandID.HasValue || product.BrandId == productQueryParam.brandID) && (!productQueryParam.typeID.HasValue || product.TypeId == productQueryParam.typeID) && (string.IsNullOrEmpty(productQueryParam.searchValue) || product.Name.ToLower().Contains(productQueryParam.searchValue.ToLower())))
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Type);

            switch (productQueryParam.sortingOption)
            {
                case ProductSortingOptions.NameAscending:
                    AddOrderBy(product => product.Name);
                    break;
                case ProductSortingOptions.NameDescending:
                    AddOrderByDesc(product => product.Name);
                    break;
                case ProductSortingOptions.PriceAscending:
                    AddOrderBy(product => product.Price);
                    break;
                case ProductSortingOptions.PriceDescending:
                    AddOrderByDesc(product => product.Price);
                    break;
                default:
                    break;
            }
        }

        public ProductSpecifications(int id) : base(product => product.Id == id)
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Type);
        }
    }
}
