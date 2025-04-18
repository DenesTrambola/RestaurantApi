namespace RestaurantApi.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ICollection<DishInOrder> DishInOrders { get; set; } = [];
}
