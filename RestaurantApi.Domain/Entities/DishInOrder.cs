namespace RestaurantApi.Domain.Entities;

public class DishInOrder
{
    public Guid DishId { get; set; }
    public Dish Dish { get; set; } = null!;

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int Quantity { get; set; }
}
