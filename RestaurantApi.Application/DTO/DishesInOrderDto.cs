namespace RestaurantApi.Application.DTO;

public class DishesInOrderDto
{
    public Guid DishId { get; set; }
    public int Quantity { get; set; }
    public DishDto Dish { get; set; } = null!;
}
