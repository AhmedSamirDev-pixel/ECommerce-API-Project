using ECommerce.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Configurations.ProductConfiguration
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasOne(product => product.Brand)
                   .WithMany()
                   .HasForeignKey(product => product.BrandId);

            builder.HasOne(product => product.Type)
                   .WithMany()
                   .HasForeignKey(product => product.TypeId);


            builder.Property(product => product.Price)
                   .HasColumnType("decimal(10,3)");

        }
    }
}
