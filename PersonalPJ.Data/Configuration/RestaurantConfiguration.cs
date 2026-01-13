using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantOrderSystem.Data.Entities;

namespace RestaurantOrderSystem.Data.Configuration
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasMany(r => r.MenuItems)
                   .WithOne(m => m.Restaurant)
                   .HasForeignKey(m => m.RestaurantId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(r => r.Orders)
                   .WithOne(o => o.Restaurant)
                   .HasForeignKey(o => o.RestaurantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}