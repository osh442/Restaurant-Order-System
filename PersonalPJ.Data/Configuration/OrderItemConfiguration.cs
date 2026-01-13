using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrderSystem.Data.Entities;

namespace RestaurantOrderSystem.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.Property(oi => oi.Price)
                   .HasPrecision(10, 2);


            builder.HasIndex(oi => new { oi.OrderId, oi.MenuItemId });
        }
    }
}