using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Infrastructure.Persistence.Configurations;

public class DishInOrderConfiguration : IEntityTypeConfiguration<DishesInOrder>
{
    public void Configure(EntityTypeBuilder<DishesInOrder> builder)
    {
        builder.HasKey(dio => new { dio.DishId, dio.OrderId });

        builder.HasOne(dio => dio.Dish)
               .WithMany(d => d.DishInOrders)
               .HasForeignKey(dio => dio.DishId);

        builder.HasOne(dio => dio.Order)
               .WithMany(o => o.DishesInOrders)
               .HasForeignKey(dio => dio.OrderId);

        builder.Property(dio => dio.Quantity)
               .IsRequired();
    }
}
