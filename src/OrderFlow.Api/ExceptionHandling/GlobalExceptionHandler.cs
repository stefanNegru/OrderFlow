using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Customers.Exceptions;
using OrderFlow.Application.Inventory.Exceptions;
using OrderFlow.Application.Products.Exceptions;
using OrderFlow.Domain.Inventory.Exceptions;
using System.Diagnostics;

namespace OrderFlow.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "An exception occurred while processing {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        var (statusCode, title) = exception switch
        {
            CustomerNotFoundException =>
                (StatusCodes.Status404NotFound, "Customer not found"),

            CustomerEmailAlreadyExistsException =>
                (StatusCodes.Status409Conflict, "Customer email already exists"),

            ProductNotFoundException =>
                (StatusCodes.Status404NotFound, "Product not found"),

            InventoryNotFoundException =>
                (StatusCodes.Status404NotFound, "Inventory not found"),

            DuplicateProductSkuException =>
                (StatusCodes.Status409Conflict, "Duplicate product SKU"),

            InsufficientStockException =>
                (StatusCodes.Status409Conflict, "Insufficient stock"),

            ArgumentException =>
                (StatusCodes.Status400BadRequest, "Invalid request"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                    "Internal server error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            Activity.Current?.Id ?? httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}