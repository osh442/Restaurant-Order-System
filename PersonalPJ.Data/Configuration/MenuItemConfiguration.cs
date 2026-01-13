using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrderSystem.Data.Entities;

namespace RestaurantOrderSystem.Data.Configuration
{
   
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            
            builder.Property(m => m.Price)
                   .HasPrecision(10, 2);

            builder.Property(m => m.IsAvailable)
                   .HasDefaultValue(true);

            builder.HasMany(m => m.OrderItems)
                   .WithOne(oi => oi.MenuItem)
                   .HasForeignKey(oi => oi.MenuItemId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}