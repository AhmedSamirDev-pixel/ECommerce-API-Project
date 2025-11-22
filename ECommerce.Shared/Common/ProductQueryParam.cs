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
        private const int defaultSize = 5;
        private const int maxSize = 10;
        public int? brandID {  get; set; }
        public int? typeID {  get; set; }
        public ProductSortingOptions? sortingOption {  get; set; }
        public string? searchValue { get; set; }
        public int pageIndex { get; set; } = 1;

        private int PageSize = defaultSize;

        public int pageSize 
        {
            get { 
                return PageSize; 
            } 

            set { 
                PageSize = value > maxSize ? maxSize : value;
            } 
        }
    }
}
