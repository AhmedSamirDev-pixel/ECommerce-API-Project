using ECommerce.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Common
{
    public class ProductQueryParam
    {
        public int? brandID {  get; set; }
        public int? typeID {  get; set; }
        public ProductSortingOptions? sortingOption {  get; set; }

        public string? searchValue { get; set; }
    }
}
