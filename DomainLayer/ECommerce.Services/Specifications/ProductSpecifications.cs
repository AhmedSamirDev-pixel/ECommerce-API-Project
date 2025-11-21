using ECommerce.Domain.Models.Products;
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
        public ProductSpecifications() : base(null)
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Type);
        }

        public ProductSpecifications(int id) : base(product => product.Id == id)
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Type);
        }
    }
}
