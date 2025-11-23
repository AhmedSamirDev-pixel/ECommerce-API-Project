using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs.BasketDTOs
{
    public class BasketItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        [Range(1, maximum:double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, maximum:100)]
        public int Quantity { get; set; }
    }
}
