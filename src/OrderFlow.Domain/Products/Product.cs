using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFlow.Domain.Products
{
    public sealed class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Sku { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }

        private Product() { }

        public Product(string name, string sku, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Product name is required.");

            if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentNullException("SKU is required.");

            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            Sku = sku.Trim();
            Price = price;
            IsActive = true;
        }

        public void Update(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name is required.", nameof(name));
            }
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
            }
            Name = name.Trim();
            Price = price;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
