using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrderSystem.Data.Entities;

namespace RestaurantOrderSystem.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
 
            builder.Property(o => o.TotalAmount)
                   .HasPrecision(10, 2);

 
            builder.Property(o => o.Status)
                   .HasDefaultValue("Pending");

 
            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}