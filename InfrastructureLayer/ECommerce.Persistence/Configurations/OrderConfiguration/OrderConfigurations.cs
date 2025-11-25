using ECommerce.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Configurations.OrderConfiguration
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.Property(o => o.SubTotal).HasColumnType("decimal(8,2)");
            builder.HasOne(o => o.DeliveryMethod).WithMany().HasForeignKey(o => o.DeliveryMethodId);
            builder.OwnsOne(o => o.OrderAddress);
        }
    }
}
