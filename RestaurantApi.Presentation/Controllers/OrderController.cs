using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Application.Services;
using RestaurantApi.Domain.Entities;
using RestaurantApi.Presentation.Models;

namespace RestaurantApi.Presentation.Controllers;

[Route("orders")]
public class OrderController(IOrderService orderService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await orderService.GetAllOrdersAsync(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await orderService.GetOrderByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
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

    [HttpPut("{id:guid}")]
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await orderService.DeleteOrderAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("profit")]
    public async Task<IActionResult> GetProfit(CancellationToken cancellationToken)
    {
        var result = await orderService.CalculateProfitAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("most-popular-dish")]
    public async Task<IActionResult> GetMostPopularDish(CancellationToken cancellationToken)
    {
        var result = await orderService.GetMostPopularDishAsync(cancellationToken);
        return Ok(result);
    }
}
