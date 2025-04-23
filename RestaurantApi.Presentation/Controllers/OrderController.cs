using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Presentation.Models;

namespace RestaurantApi.Presentation.Controllers;

[Route("orders")]
public class OrderController(IOrderService orderService) : ApiController
{
    /// <summary>
    /// Отримати всі замовлення
    /// </summary>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Список усіх замовлень</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await orderService.GetAllOrdersAsync(cancellationToken);
        return Ok(result.Value);
    }

    /// <summary>
    /// Отримати замовлення за ID
    /// </summary>
    /// <param name="id">ID замовлення</param>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Замовлення з вказаним ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await orderService.GetOrderByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Створити нове замовлення
    /// </summary>
    /// <param name="request">Модель створення замовлення</param>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Створене замовлення</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Order), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = request.CustomerName
        };

        var result = await orderService.CreateOrderAsync(order, request.DishesInOrder, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Оновити замовлення
    /// </summary>
    /// <param name="id">ID замовлення</param>
    /// <param name="request">Модель оновлення замовлення</param>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Оновлене замовлення</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = id,
            CustomerName = request.CustomerName
        };

        var result = await orderService.UpdateOrderAsync(id, order, request.DishesInOrder, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Видалити замовлення
    /// </summary>
    /// <param name="id">ID замовлення</param>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Статус видалення</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await orderService.DeleteOrderAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Отримати прибуток з усіх замовлень
    /// </summary>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Прибуток</returns>
    [HttpGet("profit")]
    [ProducesResponseType(typeof(decimal), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetProfit(CancellationToken cancellationToken)
    {
        var result = await orderService.CalculateProfitAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Отримати найпопулярнішу страву у замовленнях
    /// </summary>
    /// <param name="cancellationToken">Токен скасування</param>
    /// <returns>Найпопулярніша страва</returns>
    [HttpGet("most-popular-dish")]
    [ProducesResponseType(typeof(Dish), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetMostPopularDish(CancellationToken cancellationToken)
    {
        var result = await orderService.GetMostPopularDishAsync(cancellationToken);
        return Ok(result);
    }
}
