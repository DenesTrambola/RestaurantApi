using Microsoft.EntityFrameworkCore;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Infrastructure.Persistence.Configurations;

namespace RestaurantApi.Infrastructure.Persistence.Data;

public class RestaurantDbContext : DbContext
{
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<DishInOrder> DishInOrders { get; set; }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DishConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new DishInOrderConfiguration());
    }
}
