using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Presentation.Models;

public class CreateOrderRequest
{
    [Required]
    [StringLength(50)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    public ICollection<Guid> DishIds { get; set; } = [];
}
