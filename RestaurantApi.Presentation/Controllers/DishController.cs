using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Presentation.Models;

namespace RestaurantApi.Presentation.Controllers;

[Route("dishes")]
public class DishController(IDishService dishService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await dishService.GetAllDishesAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await dishService.GetDishByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDishRequest request, CancellationToken cancellationToken)
    {
        var dish = new Dish
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        var result = await dishService.CreateDishAsync(dish, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateDishRequest request, CancellationToken cancellationToken)
    {
        var dish = new Dish
        {
            Id = id,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        var result = await dishService.UpdateDishAsync(id, dish, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await dishService.DeleteDishAsync(id, cancellationToken);
        return Ok(result);
    }
}
