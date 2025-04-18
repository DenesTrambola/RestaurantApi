using ErrorOr;
using RestaurantApi.Domain.Entities;

namespace RestaurantApi.Application.Services;

public interface IDishService
{
    Task<ErrorOr<List<Dish>>> GetAllDishesAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> GetDishByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> CreateDishAsync(Dish dish, CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> UpdateDishAsync(Guid id, Dish dish, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteDishAsync(Guid id, CancellationToken cancellationToken = default);
}
