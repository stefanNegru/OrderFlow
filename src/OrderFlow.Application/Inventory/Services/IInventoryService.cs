using OrderFlow.Application.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Application.Inventory.Services;

public interface IInventoryService
{
    Task<InventoryResponse> GetAsync(
        Guid productId,
        CancellationToken cancellation = default);
    Task<InventoryResponse> AddStockAsync(
        Guid productId,
        AddStockRequest request,
        CancellationToken cancellation = default);
    Task<InventoryResponse> RemoveStockAsync(
        Guid productId,
        RemoveStockRequest request,
        CancellationToken cancellation = default);
    Task<IReadOnlyList<StockMovementResponse>> GetMovementsAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}
