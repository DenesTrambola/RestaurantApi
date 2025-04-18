using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Infrastructure.Persistence.Data;

namespace RestaurantApi.Infrastructure.Persistence.Services;

public class DishService(RestaurantDbContext dbContext) : IDishService
{
    public async Task<ErrorOr<List<Dish>>> GetAllDishesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Dishes.ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<Dish>> GetDishByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dish = await dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        return dish is null ? Error.NotFound("Dish not found") : dish;
    }

    public async Task<ErrorOr<Dish>> CreateDishAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync(cancellationToken);
        return dish;
    }

    public async Task<ErrorOr<Dish>> UpdateDishAsync(Guid id, Dish dish, CancellationToken cancellationToken = default)
    {
        var existingDish = await dbContext.Dishes.AsTracking().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (existingDish is null)
            return Error.NotFound("Dish not found");

        existingDish.Name = dish.Name;
        existingDish.Price = dish.Price;
        existingDish.Description = dish.Description;

        await dbContext.SaveChangesAsync(cancellationToken);
        return existingDish;
    }

    public async Task<ErrorOr<Deleted>> DeleteDishAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dish = await dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (dish is null)
            return Error.NotFound("Dish not found");

        dbContext.Dishes.Remove(dish);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
