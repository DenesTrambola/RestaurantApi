using ErrorOr;
using RestaurantApi.Application.DTO;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Application.Services;

public interface IOrderService
{
    Task<ErrorOr<OrderDto>> CreateOrderAsync(Order order, IDictionary<Guid, int> dishesInOrder, CancellationToken cancellationToken = default);
    Task<ErrorOr<OrderDto>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<ICollection<OrderDto>>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<OrderDto>> UpdateOrderAsync(Guid id, Order order, IDictionary<Guid, int> dishesInOrder, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<decimal>> CalculateProfitAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<DishDto>> GetMostPopularDishAsync(CancellationToken cancellationToken = default);
}
