using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddress OrderAddress { get; set; } = null!;

        [ForeignKey(nameof(DeliveryMethod))]
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = null!;
        public OrderStatus OrderStatus {  get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
    }
}
