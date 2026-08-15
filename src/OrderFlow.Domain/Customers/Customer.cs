using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Domain.Customers;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    private Customer()
    {
        // Required by EF Core
    }

    public Customer(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be null or empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be null or empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Customer phone cannot be null or empty.", nameof(phone));

        Id = Guid.NewGuid();
        Name = name;
        Email = email.Trim().ToLowerInvariant();
        Phone = phone;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be null or empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email cannot be null or empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Customer phone cannot be null or empty.", nameof(phone));
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Phone = phone.Trim();
    }
}

