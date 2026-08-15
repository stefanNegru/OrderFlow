using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Inventory.Services;
using OrderFlow.Application.Inventory.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    // GET: api/<InventoryController>
    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<InventoryResponse>> Get(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var inventory = await inventoryService.GetAsync(productId, cancellationToken);

        if (inventory is null)
            return NotFound();

        return Ok(inventory);
    }

    // GET api/<InventoryController>/5
    [HttpPost("{productId:guid}/add")]
    public async Task<ActionResult<InventoryResponse>> AddStock(
        Guid productId,
        AddStockRequest request,
        CancellationToken cancellationToken = default)
    {
        var inventory = await inventoryService.AddStockAsync(productId, request, cancellationToken);
        return Ok(inventory);
    }

    // POST api/<InventoryController>
    [HttpPost("{productId:guid}/remove")]
    public async Task<ActionResult<InventoryResponse>> RemoveStock(
        Guid productId,
        RemoveStockRequest request,
        CancellationToken cancellation)
    {
        var inventory = await inventoryService.RemoveStockAsync(productId, request, cancellation);
        return Ok(inventory);
    }

    // PUT api/<InventoryController>/5
    [HttpGet("{productId:guid}/movements")]
    public async Task<ActionResult<IReadOnlyList<StockMovementResponse>>> GetMovements(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var movements = await inventoryService.GetMovementsAsync(productId, cancellationToken);
        return Ok(movements);
    }
}
