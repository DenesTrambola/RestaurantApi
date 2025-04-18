using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Application.DTO;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Infrastructure.Persistence.Data;

namespace RestaurantApi.Infrastructure.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly RestaurantDbContext _context;

    public OrderService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<OrderDto>> CreateOrderAsync(Order order, IDictionary<Guid, int> dishesInOrder, CancellationToken cancellationToken = default)
    {
        if (!dishesInOrder.Any())
            return Error.Validation("At least one dish must be included in the order");

        order.CreatedAt = DateTime.UtcNow;
        var dishesInOrderDtos = new List<DishesInOrderDto>();

        foreach (var dishInOrder in dishesInOrder)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishInOrder.Key, cancellationToken);
            if (dish is null)
                return Error.NotFound($"Dish with ID {dishInOrder.Key} not found");

            order.DishesInOrders.Add(new DishesInOrder
            {
                DishId = dishInOrder.Key,
                OrderId = order.Id,
                Quantity = dishInOrder.Value
            });

            dishesInOrderDtos.Add(new DishesInOrderDto
            {
                DishId = dishInOrder.Key,
                Quantity = dishInOrder.Value,
                Dish = new DishDto
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Description = dish.Description
                }
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CreatedAt = order.CreatedAt,
            DishesInOrders = dishesInOrderDtos
        };
    }

    public async Task<ErrorOr<OrderDto>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.DishesInOrders)
            .ThenInclude(dio => dio.Dish)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
            return Error.NotFound("Order not found");

        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CreatedAt = order.CreatedAt,
            DishesInOrders = order.DishesInOrders.Select(dio => new DishesInOrderDto
            {
                DishId = dio.DishId,
                Quantity = dio.Quantity,
                Dish = new DishDto
                {
                    Id = dio.Dish.Id,
                    Name = dio.Dish.Name,
                    Price = dio.Dish.Price,
                    Description = dio.Dish.Description
                }
            }).ToList()
        };
    }

    public async Task<ErrorOr<ICollection<OrderDto>>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .Include(o => o.DishesInOrders)
            .ThenInclude(dio => dio.Dish)
            .ToListAsync(cancellationToken);

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            CreatedAt = o.CreatedAt,
            DishesInOrders = o.DishesInOrders.Select(dio => new DishesInOrderDto
            {
                DishId = dio.DishId,
                Quantity = dio.Quantity,
                Dish = new DishDto
                {
                    Id = dio.Dish.Id,
                    Name = dio.Dish.Name,
                    Price = dio.Dish.Price,
                    Description = dio.Dish.Description
                }
            }).ToList()
        }).ToList();
    }

    public async Task<ErrorOr<OrderDto>> UpdateOrderAsync(Guid id, Order updatedOrder, IDictionary<Guid, int> dishesInOrder, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.DishesInOrders)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
            return Error.NotFound("Order not found");

        if (!dishesInOrder.Any())
            return Error.Validation("At least one dish must be included in the order");

        order.CustomerName = updatedOrder.CustomerName;

        _context.DishInOrders.RemoveRange(order.DishesInOrders);
        order.DishesInOrders = new List<DishesInOrder>();

        foreach (var dishInOrder in dishesInOrder)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishInOrder.Key, cancellationToken);
            if (dish is null)
                return Error.NotFound($"Dish with ID {dishInOrder.Key} not found");

            order.DishesInOrders.Add(new DishesInOrder
            {
                DishId = dishInOrder.Key,
                OrderId = order.Id,
                Quantity = dishInOrder.Value
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CreatedAt = order.CreatedAt,
            DishesInOrders = order.DishesInOrders.Select(dio => new DishesInOrderDto
            {
                DishId = dio.DishId,
                Quantity = dio.Quantity,
                Dish = new DishDto
                {
                    Id = dio.Dish.Id,
                    Name = dio.Dish.Name,
                    Price = dio.Dish.Price,
                    Description = dio.Dish.Description
                }
            }).ToList()
        };
    }

    public async Task<ErrorOr<Deleted>> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (order is null)
            return Error.NotFound("Order not found");

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }

    public async Task<ErrorOr<decimal>> CalculateProfitAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .Include(o => o.DishesInOrders)
            .ThenInclude(dio => dio.Dish)
            .ToListAsync(cancellationToken);

        decimal profit = orders
            .SelectMany(o => o.DishesInOrders)
            .Sum(dio => dio.Quantity * dio.Dish.Price);

        return profit;
    }

    public async Task<ErrorOr<DishDto>> GetMostPopularDishAsync(CancellationToken cancellationToken = default)
    {
        var mostPopularDish = await _context.DishInOrders
            .GroupBy(dio => dio.DishId)
            .Select(g => new
            {
                DishId = g.Key,
                TotalQuantity = g.Sum(dio => dio.Quantity)
            })
            .OrderByDescending(g => g.TotalQuantity)
            .Join(_context.Dishes,
                g => g.DishId,
                d => d.Id,
                (g, d) => d)
            .FirstOrDefaultAsync(cancellationToken);

        if (mostPopularDish is null)
            return Error.NotFound("No dishes found");

        return new DishDto
        {
            Id = mostPopularDish.Id,
            Name = mostPopularDish.Name,
            Price = mostPopularDish.Price,
            Description = mostPopularDish.Description
        };
    }
}
