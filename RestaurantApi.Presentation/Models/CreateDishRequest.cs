using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Presentation.Models;

public class CreateDishRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
