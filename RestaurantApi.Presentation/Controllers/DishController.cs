using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Presentation.Models;

namespace RestaurantApi.Presentation.Controllers;

[Route("dishes")]
public class DishController(IDishService dishService) : ApiController
{
    /// <summary>
    /// Отримати всі страви
    /// </summary>
    /// <param name="cancellationToken">Токен відміни</param>
    /// <returns>Список всіх страв</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Dish>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await dishService.GetAllDishesAsync(cancellationToken);
        return Ok(result.Value);
    }

    /// <summary>
    /// Отримати страву за ID
    /// </summary>
    /// <param name="id">Ідентифікатор страви</param>
    /// <param name="cancellationToken">Токен відміни</param>
    /// <returns>Страва з вказаним ID або помилка</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Dish), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await dishService.GetDishByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Додати нову страву
    /// </summary>
    /// <param name="request">Модель для створення страви</param>
    /// <param name="cancellationToken">Токен відміни</param>
    /// <returns>Створена страва або помилка</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Dish), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
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

    /// <summary>
    /// Оновити існуючу страву
    /// </summary>
    /// <param name="id">ID страви для оновлення</param>
    /// <param name="request">Модель оновлення</param>
    /// <param name="cancellationToken">Токен відміни</param>
    /// <returns>Оновлена страва або помилка</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Dish), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
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

    /// <summary>
    /// Видалити страву за ID
    /// </summary>
    /// <param name="id">ID страви</param>
    /// <param name="cancellationToken">Токен відміни</param>
    /// <returns>Статус видалення</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await dishService.DeleteDishAsync(id, cancellationToken);
        return Ok(result);
    }
}
