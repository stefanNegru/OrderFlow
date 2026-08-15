using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Application.Products.Dtos
{
    public sealed record ProductResponse(
        Guid Id,
        string Name,
        string Sku,
        decimal Price,
        bool IsActive
    );
}
