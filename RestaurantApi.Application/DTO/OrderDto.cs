namespace RestaurantApi.Application.DTO;

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ICollection<DishesInOrderDto> DishesInOrders { get; set; } = [];
}
