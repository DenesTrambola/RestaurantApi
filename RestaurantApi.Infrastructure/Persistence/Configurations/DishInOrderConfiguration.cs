using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Infrastructure.Persistence.Configurations;

public class DishInOrderConfiguration : IEntityTypeConfiguration<DishInOrder>
{
    public void Configure(EntityTypeBuilder<DishInOrder> builder)
    {
        builder.HasKey(dio => new { dio.DishId, dio.OrderId });

        builder.HasOne(dio => dio.Dish)
               .WithMany(d => d.DishInOrders)
               .HasForeignKey(dio => dio.DishId);

        builder.HasOne(dio => dio.Order)
               .WithMany(o => o.DishInOrders)
               .HasForeignKey(dio => dio.OrderId);

        builder.Property(dio => dio.Quantity)
               .IsRequired();
    }
}
