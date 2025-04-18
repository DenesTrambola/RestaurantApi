namespace RestaurantApi.Domain.Entities;

public class Dish
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public ICollection<DishInOrder> DishInOrders { get; set; } = [];
}
