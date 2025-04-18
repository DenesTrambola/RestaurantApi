using ErrorOr;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Application.Services;

public interface IOrderService
{
    Task<ErrorOr<Order>> CreateOrderAsync(Order order, ICollection<Guid> dishIds, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<ICollection<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> UpdateOrderAsync(Guid id, Order order, ICollection<Guid> dishIds, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<decimal>> CalculateProfitAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> GetMostPopularDishAsync(CancellationToken cancellationToken = default);
}
