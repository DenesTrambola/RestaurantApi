using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Infrastructure.Persistence.Data;

namespace RestaurantApi.Infrastructure.Persistence.Services;

public class OrderService(RestaurantDbContext dbContext) : IOrderService
{
    public async Task<ErrorOr<Order>> CreateOrderAsync(Order order, ICollection<Guid> dishIds, CancellationToken cancellationToken = default)
    {
        order.CreatedAt = DateTime.UtcNow;
        order.DishInOrders = new List<DishInOrder>();

        foreach (var dishId in dishIds)
        {
            var dish = await dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId, cancellationToken);
            if (dish is null)
                return Error.NotFound($"Dish with ID {dishId} not found");

            order.DishInOrders.Add(new DishInOrder
            {
                DishId = dishId,
                OrderId = order.Id,
                Quantity = 1 // За замовчуванням 1, можна зробити параметром
            });
        }

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<ErrorOr<Order>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .Include(o => o.DishInOrders)
            .ThenInclude(dio => dio.Dish)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return order is null ? Error.NotFound("Order not found") : order;
    }

    public async Task<ErrorOr<ICollection<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .Include(o => o.DishInOrders)
            .ThenInclude(dio => dio.Dish)
            .ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<Order>> UpdateOrderAsync(Guid id, Order updatedOrder, ICollection<Guid> dishIds, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders
            .AsTracking()
            .Include(o => o.DishInOrders)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
            return Error.NotFound("Order not found");

        order.CustomerName = updatedOrder.CustomerName;

        dbContext.DishInOrders.RemoveRange(order.DishInOrders);
        order.DishInOrders = new List<DishInOrder>();

        foreach (var dishId in dishIds)
        {
            var dish = await dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId, cancellationToken);
            if (dish is null)
                return Error.NotFound($"Dish with ID {dishId} not found");

            order.DishInOrders.Add(new DishInOrder
            {
                DishId = dishId,
                OrderId = order.Id,
                Quantity = 1 // За замовчуванням 1
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<ErrorOr<Deleted>> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order is null)
            return Error.NotFound("Order not found");

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }

    public async Task<ErrorOr<decimal>> CalculateProfitAsync(CancellationToken cancellationToken = default)
    {
        var orders = await dbContext.Orders
            .Include(o => o.DishInOrders)
            .ThenInclude(dio => dio.Dish)
            .ToListAsync(cancellationToken);

        decimal profit = orders
            .SelectMany(o => o.DishInOrders)
            .Sum(dio => dio.Quantity * dio.Dish.Price);

        return profit;
    }

    public async Task<ErrorOr<Dish>> GetMostPopularDishAsync(CancellationToken cancellationToken = default)
    {
        var mostPopularDish = await dbContext.DishInOrders
            .GroupBy(dio => dio.DishId)
            .Select(g => new
            {
                DishId = g.Key,
                TotalQuantity = g.Sum(dio => dio.Quantity)
            })
            .OrderByDescending(g => g.TotalQuantity)
            .Join(dbContext.Dishes,
                g => g.DishId,
                d => d.Id,
                (g, d) => d)
            .FirstOrDefaultAsync(cancellationToken);

        return mostPopularDish is null
            ? Error.NotFound("No dishes found")
            : mostPopularDish;
    }
}
