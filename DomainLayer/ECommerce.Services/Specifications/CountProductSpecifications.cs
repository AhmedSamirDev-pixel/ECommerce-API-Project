using ECommerce.Domain.Models.Products;
using ECommerce.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications
{
    public class CountProductSpecifications : BaseSpecifications<Product, int>
    {
        public CountProductSpecifications(ProductQueryParam productQueryParam) : base(product => (!productQueryParam.brandID.HasValue || product.BrandId == productQueryParam.brandID) && (!productQueryParam.typeID.HasValue || product.TypeId == productQueryParam.typeID) && (string.IsNullOrEmpty(productQueryParam.searchValue) || product.Name.ToLower().Contains(productQueryParam.searchValue.ToLower())))
        {

        }
    }
}
