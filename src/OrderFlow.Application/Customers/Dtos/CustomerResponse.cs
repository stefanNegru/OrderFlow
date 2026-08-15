public sealed record CustomerResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    DateTime CreatedAtUtc);