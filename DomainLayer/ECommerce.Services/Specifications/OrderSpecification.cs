using ECommerce.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications
{
    public class OrderSpecification : BaseSpecifications<Order,Guid>
    {
        public OrderSpecification(string email) : base(o => o.UserEmail == email)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);

        }

        public OrderSpecification(Guid id) : base(o => o.Id == id)
        {
            AddInclude(o => o.Items);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
